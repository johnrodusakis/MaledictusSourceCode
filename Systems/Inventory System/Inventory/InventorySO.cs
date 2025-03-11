using Maledictus.CustomSoap;
using Obvious.Soap;
using SomniaGames.Persistence;
using System.Collections.Generic;
using UnityEngine;
using VInspector;
using VInspector.Libs;

namespace Maledictus.Inventory
{
    public enum InventoryType
    {
        Weapons,
        Necklaces,
        Bracelets,
        Rings
    }

    public enum SortType
    {
        Category,
        LastLooted,
        Level,
        Rarity
    }

    public abstract class InventorySO<T> : ScriptableObject where T : ItemSO
    {
        public System.Action<BaseInventoryData> OnInventoryUpdate { get; set; }

        [Tab("Inventory")]
        [SerializeField] protected InventoryType _inventoryType;
        [SerializeField] protected ItemDatabaseSO<T> _itemDatabaseSO;

        [Space(10f)]
        [SerializeField] protected IntVariable _maxSlotCapacity;

        public abstract GearInventorySO<T> GearInventorySO { get; set; }
        public abstract List<ItemSlot<T>> ItemSlots { get; set; }

        [Tab("Events")]
        [SerializeField] protected ScriptableEventGearSlotData _onGearSlotSelected;
        [SerializeField] protected ScriptableEventNoParam _onItemSelected;
        [SerializeField] protected ScriptableEventNoParam _onItemNewNotification;
        [SerializeField] private ScriptableEventNoParam _onLoadInventoryData;

        [SerializeField] private ScriptableEventString _onSortItems;

        protected InventorySlot<T>[] _inventorySlots = new InventorySlot<T>[0];
        protected double _orderCounter = 0f;

        protected SortType _sortType;

        public ItemDatabaseSO<T> ItemDatabaseSO => _itemDatabaseSO;
        public bool HasNewItem => ItemSlots.FindAll(m => m.IsNew == true).Count > 0;

        protected virtual void OnEnable()
        {
            PersistenceSystem.OnLoadCompleted += HandleLoadGameData;

            _maxSlotCapacity.OnValueChanged += UpdateInventorySlotCapacity;
            _onGearSlotSelected.OnRaised += HandleGearSlotSelected;

            _onItemNewNotification.OnRaised += HandleSaveInventoryData;
            _onItemSelected.OnRaised += HandleSaveInventoryData;
        }

        protected virtual void OnDisable()
        {
            PersistenceSystem.OnLoadCompleted -= HandleLoadGameData;

            _maxSlotCapacity.OnValueChanged -= UpdateInventorySlotCapacity;
            _onGearSlotSelected.OnRaised -= HandleGearSlotSelected;

            _onItemNewNotification.OnRaised -= HandleSaveInventoryData;
            _onItemSelected.OnRaised -= HandleSaveInventoryData;
        }

        private void HandleSaveInventoryData() => OnInventoryUpdate?.Invoke(GetInventoryData());

        public void AddItem(T item)
        {
            foreach (var slot in _inventorySlots)
            {
                if (slot.IsEmpty)
                {
                    slot.SetItem(item);

                    var itemSlot = new ItemSlot<T>();
                    itemSlot.SetItem(item, _orderCounter);
                    ItemSlots.Add(itemSlot);
                    _orderCounter += 0.01;

                    SortItems();

                    _onItemNewNotification.Raise();
                    return;
                }
            }
        }

        private void HandleLoadGameData(GameData data)
        {
            var hasNewItem = false;

            ClearInventory();

            var inventoryData = _inventoryType switch
            {
                InventoryType.Weapons => data.InventoryData.WeaponInventoryData,
                InventoryType.Necklaces => data.InventoryData.NecklaceInventoryData,
                InventoryType.Bracelets => data.InventoryData.BraceletInventoryData,
                InventoryType.Rings => data.InventoryData.RingInventoryData,
                _ => data.InventoryData.WeaponInventoryData,
            };

            InitializeInventorySlots(inventoryData.SlotCapacity);

            GearInventorySO = new(inventoryData.GearSlots.Count);
            for (int i = 0; i < GearInventorySO.Slots.Length; i++)
            {
                var gearSlot = new GearSlot<T>();

                var gearSlotData = inventoryData.GearSlots[i];
                var itemSlotData = gearSlotData.ItemSlotData;

                var itemSlot = new ItemSlot<T>();

                if (itemSlotData != null && !itemSlotData.IsEmpty)
                    itemSlot.SetItem(ItemDatabaseSO.FindItemSO(itemSlotData.Id), itemSlotData.Order, itemSlotData.IsSelected, itemSlotData.IsNew);

                gearSlot.SetSlot(itemSlot, gearSlotData.IsLocked);

                GearInventorySO.Slots[i] = gearSlot;
            }

            ItemSlots = new();
            for (int i = 0; i < inventoryData.ItemSlots.Count; i++)
            {
                var itemSlotData = inventoryData.ItemSlots[i];
                var item = ItemDatabaseSO.FindItemSO(itemSlotData.Id);

                _inventorySlots[i].SetItem(item);

                var itemSlot = new ItemSlot<T>();
                itemSlot.SetItem(item, itemSlotData.Order, itemSlotData.IsSelected, itemSlotData.IsNew);

                _orderCounter += 0.01;

                if (!hasNewItem && itemSlotData.IsNew)
                    hasNewItem = true;

                ItemSlots.Add(itemSlot);
            }

            if (hasNewItem)
                _onItemNewNotification.Raise();

            _onLoadInventoryData.Raise();
            OnInventoryUpdate?.Invoke(GetInventoryData());

            SortItems();
        }

        private BaseInventoryData GetInventoryData()
        {
            var gearSlots = new List<SomniaGames.Persistence.GearSlotData>();
            var itemSlots = new List<ItemSlotData>();

            foreach (var gearSlot in GearInventorySO.Slots)
            {
                var itemSlot = gearSlot.SelectedSlot;
                var item = itemSlot.Item;

                var itemSlotData = new ItemSlotData();
                if(item != null)
                    itemSlotData = new ItemSlotData(item.Id, itemSlot.Order, itemSlot.IsSelected, itemSlot.IsNew);

                var gearSlotData = new SomniaGames.Persistence.GearSlotData(itemSlotData, gearSlot.IsLocked);

                gearSlots.Add(gearSlotData);
            }

            foreach (var itemSlot in ItemSlots)
            {
                var item = itemSlot.Item;

                var itemSlotData = new ItemSlotData(item.Id, itemSlot.Order, itemSlot.IsSelected, itemSlot.IsNew);

                itemSlots.Add(itemSlotData);
            }

            return new BaseInventoryData(_maxSlotCapacity, gearSlots, itemSlots);
        }

        private void InitializeInventorySlots(int newSlotCapacity)
        {
            _inventorySlots = new InventorySlot<T>[newSlotCapacity];

            for (int i = 0; i < newSlotCapacity; i++)
                _inventorySlots[i] = new InventorySlot<T>();
        }

        protected void UpdateInventorySlotCapacity(int newSlotCapacity)
        {
            var inventorySize = Mathf.Min(newSlotCapacity, _inventorySlots.Length);
            var newInventorySlots = new InventorySlot<T>[newSlotCapacity];

            for (int i = 0; i < inventorySize; i++)
                newInventorySlots[i] = _inventorySlots[i];

            _inventorySlots = newInventorySlots;
        }

        private void HandleGearSlotSelected(GearSlotData data)
        {
            GearInventorySO.SetSelectedSlotIndex(data.SlotIndex);
            SortItems();
        }

        protected void HandleSelectedItem(ItemSlot<T> slot)
        {
            var selectedSlot = GearInventorySO.GetSelectedSlot();
            if (selectedSlot.IsSelected)
            {
                var weaponSlot = ItemSlots.Find(s => s.Order == selectedSlot.Order);
                weaponSlot.DeselectSlot();
            }

            var newWeaponSlot = ItemSlots.Find(s => s == slot);
            newWeaponSlot.SelectSlot();

            GearInventorySO.SetSelectedSlot(slot);

            _onItemSelected.Raise();
        }

        public void ToggleSort()
        {
            _sortType = _sortType.GetNext();
            SortItems();
        }

        protected void SortItems()
        {
            SortBy(_sortType);
            _onSortItems.Raise(_sortType.ToSpacedString());
        }

        private void SortBy(SortType type)
        {
            switch (type)
            {
                case SortType.Category:
                    SortByCategory();
                    break;
                case SortType.LastLooted:
                    SortByLastLooted();
                    break;
                case SortType.Level:
                    SortByLevel();
                    break;
                case SortType.Rarity:
                    SortByRarity();
                    break;
                default:
                    SortByCategory();
                    break;
            }
        }

        private void SortByCategory() => ItemSlots.Sort((x, y) => (y.Item.Category).CompareTo(x.Item.Category));
        private void SortByLastLooted() => ItemSlots.Sort((x, y) => y.Order.CompareTo(x.Order));
        private void SortByLevel() => ItemSlots.Sort((x, y) => (y.Item.Level).CompareTo(x.Item.Level));
        private void SortByRarity() => ItemSlots.Sort((x, y) => ((int)y.Item.ItemRarity).CompareTo((int)x.Item.ItemRarity));

        [ContextMenu("Clear Inventory")]
        protected virtual void ClearInventory()
        {
            _sortType = SortType.Category;
            _orderCounter = 0;

            _inventorySlots = new InventorySlot<T>[_maxSlotCapacity.Value];

            GearInventorySO.ClearInventory();
            ItemSlots.Clear();
        }
    }
}