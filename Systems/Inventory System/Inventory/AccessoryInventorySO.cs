using Maledictus.Events;
using Obvious.Soap;
using System;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

namespace Maledictus.Inventory
{

    [CreateAssetMenu(menuName = "Inventory System/Inventory/Cosmetic Inventory")]
    public class AccessoryInventorySO : InventorySO<AccessoryItemSO>
    {
        [Tab("Inventory")]
        public GearInventorySO<AccessoryItemSO> GearInventory = new(1);
        public List<ItemSlot<AccessoryItemSO>> AccessorySlots = new();

        [Tab("Events")]
        [SerializeField] private ScriptableEventAccessorySlot _onAccessorySelected;

        protected override void OnEnable()
        {
            base.OnEnable();

            _onGearSlotSelected.OnRaised += HandleGearSlotSelected;
            _onAccessorySelected.OnRaised += HandleSelectedAccessory;
        }

        protected override void OnDisable()
        {
            base.OnEnable();

            _onGearSlotSelected.OnRaised -= HandleGearSlotSelected;
            _onAccessorySelected.OnRaised -= HandleSelectedAccessory;
        }

        private void HandleGearSlotSelected(GearSlotData data)
        {
            GearInventory.SetSelectedSlotIndex(data.SlotIndex);
            SortItems();
        }

        private void HandleSelectedAccessory(ItemSlot<AccessoryItemSO> slot)
        {
            if (slot.Item.AccessoryType != AccessorySlots[0].Item.AccessoryType) return;

            var selectedSlot = GearInventory.GetSelectedSlot();
            var selectedIndex = GearInventory.SelectedSlotIndex;
            if (selectedSlot.IsSelected)
            {
                var accessorySlot = AccessorySlots.Find(s => s.Order == selectedSlot.Order);
                accessorySlot.DeselectSlot();
            }

            var newAccessorySlot = AccessorySlots.Find(s => s == slot);
            newAccessorySlot.SelectSlot();

            GearInventory.SetSelectedSlot(slot);

            _onItemSelected.Raise();
        }
        
        protected override List<ItemSlot<AccessoryItemSO>> GetListOfItems() => AccessorySlots;

        protected override void OnItemCollected(AccessoryItemSO item)
        {
            var accessorySlot = new ItemSlot<AccessoryItemSO>();
            accessorySlot.SetItem(item, _orderCounter);

            AccessorySlots.Add(accessorySlot);
            SortItems();
        }

        [ContextMenu(nameof(ClearInventory))]
        protected override void ClearInventory()
        {
            base.ClearInventory();

            GearInventory.ClearInventory();
            AccessorySlots.Clear();
        }
    }
}