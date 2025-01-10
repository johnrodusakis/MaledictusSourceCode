using Maledictus.Events;
using Obvious.Soap;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

namespace Maledictus.Inventory
{
    public enum SortType
    {
        Category,
        LastLooted,
        Level,
        Rarity
    }

    public abstract class InventorySO<T> : ScriptableObject where T : ItemSO
    {
        [Tab("Inventory")]
        [SerializeField] protected IntVariable _maxSlotCapacity;

        [Tab("Events")]
        [SerializeField] protected ScriptableEventGearSlotData _onGearSlotSelected;
        [SerializeField] protected ScriptableEventNoParam _onItemSelected;

        [SerializeField] private ScriptableEventString _onSortItems;

        protected InventorySlot<T>[] _inventorySlots = new InventorySlot<T>[0];
        protected double _orderCounter = 0f;

        protected SortType _sortType;

        protected virtual void OnEnable()
        {
            UpdateInventorySlotCapacity(_maxSlotCapacity.Value);

            _maxSlotCapacity.OnValueChanged += UpdateInventorySlotCapacity;
        }

        protected virtual void OnDisable()
        {
            _maxSlotCapacity.OnValueChanged += UpdateInventorySlotCapacity;
        }

        public void AddItem(T item)
        {
            foreach (var slot in _inventorySlots)
            {
                if (slot.IsEmpty)
                {
                    slot.SetItem(item);

                    OnItemCollected(item);
                    _orderCounter += double.Epsilon;
                    return;
                }
            }
        }

        protected abstract void OnItemCollected(T item);

        private void UpdateInventorySlotCapacity(int newSlotCapacity)
        {
            var inventorySize = Mathf.Min(newSlotCapacity, _inventorySlots.Length);
            var newInventorySlots = new InventorySlot<T>[newSlotCapacity];

            for (int i = 0; i < inventorySize; i++)
                newInventorySlots[i] = _inventorySlots[i];

            _inventorySlots = newInventorySlots;
        }

        [Button]
        public void ToggleSort()
        {
            _sortType = _sortType.GetNext();
            SortItems();
        }

        protected abstract List<ItemSlot<T>> GetListOfItems();

        protected void SortItems() => SortBy(_sortType);

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
            }

            Debug.Log($"Sorted By: {type.ToSpacedString()}");
            _onSortItems.Raise(_sortType.ToSpacedString());
        }

        private void SortByCategory() => GetListOfItems().Sort((x, y) => (y.Item.Category).CompareTo(x.Item.Category));
        private void SortByLastLooted() => GetListOfItems().Sort((x, y) => y.Order.CompareTo(x.Order));
        private void SortByLevel() => GetListOfItems().Sort((x, y) => (y.Item.Level).CompareTo(x.Item.Level));
        private void SortByRarity() => GetListOfItems().Sort((x, y) => ((int)y.Item.ItemRarity).CompareTo((int)x.Item.ItemRarity));

        protected virtual void ClearInventory()
        {
            _sortType = 0;
            _inventorySlots = new InventorySlot<T>[_maxSlotCapacity.Value];
        }
    }
}