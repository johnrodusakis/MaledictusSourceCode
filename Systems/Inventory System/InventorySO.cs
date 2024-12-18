using Obvious.Soap;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Maledictus.Inventory
{
    [CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Inventory")]
    public class InventorySO : ScriptableObject
    {
        [SerializeField] private EquipmentSO _equipmentSO;

        [Space(15f)]
        public ItemType ItemType;
        public int NumberOfItemsPerRow = 3;

        [Space(15f)]
        [Header("Events")]
        [SerializeField] private ScriptableEvent<InventorySlot> _onUpdateInventorySlotUI;

        [Space(15f)]
        public List<InventorySlot> InventorySlots = new();

        private void OnValidate()
        {
            foreach (var slot in InventorySlots)
            {
                if (slot.IsEmpty)
                    slot.Amount = 0;
            }
        }

        public bool TryEquipItem(ItemSO item) => _equipmentSO != null && _equipmentSO.TryEquipItem(item);

        public void SwapItems(InventorySlot slot1, InventorySlot slot2)
        {
            var slot1Index = GetInventorySlotIndex(slot1);
            var slot2Index = GetInventorySlotIndex(slot2);

            if(slot1Index < 0 || slot2Index < 0)
            {
                _equipmentSO.EquipItem(slot2, slot1);
                return;
            }

            var item1 = slot1.Item;
            var amount1 = slot1.Amount;

            var item2 = slot2.Item;
            var amount2 = slot2.Amount;

            AddItemAt(slot1Index, item2, amount2);
            AddItemAt(slot2Index, item1, amount1);
        }

        public void AddItemAt(int index, ItemSO item, int amount)
        {
            var slot = InventorySlots[index];

            slot.SetItem(item, amount);
            _onUpdateInventorySlotUI.Raise(slot);
        }

        public bool AddItem(ItemSO item, int amount)
        {
            if(ItemType == item.Type)
            {
                var slot = FindItemInInventory(item);
                if (slot != null && item.Stackable)
                {
                    slot.AddAmount(amount);
                    _onUpdateInventorySlotUI.Raise(slot);
                    return true;
                }

                if (HasEmptySlot())
                {
                    var emptySlot = SetEmptySlot(item, amount);
                    _onUpdateInventorySlotUI.Raise(emptySlot);
                    return true;
                }
                else
                {
                    var newSlot = new InventorySlot(item, amount);
                    InventorySlots.Add(newSlot);
                    _onUpdateInventorySlotUI.Raise(newSlot);

                    for (int i = 1; i < NumberOfItemsPerRow; i++)
                    {
                        var emptySlot = new InventorySlot();
                        InventorySlots.Add(emptySlot);
                        _onUpdateInventorySlotUI.Raise(emptySlot);
                    }
                }
            }

            return false;
        }

        public int GetInventorySlotIndex(InventorySlot inventorySlot)
        {
            for (int i = 0; i < InventorySlots.Count; i++)
            {
                if(inventorySlot == InventorySlots[i])
                    return i;
            }

            return -1;
        }

        public void RemoveItem(ItemSO item, int amount)
        {
            var slot = FindItemInInventory(item);
            if (slot != null)
            {
                if (slot.Amount > amount)
                    slot.RemoveAmount(amount);
                else 
                    slot.ClearSlot();

                _onUpdateInventorySlotUI.Raise(slot);
            }
        }

        public void RemoveItem(ItemSO item)
        {
            var slot = FindItemInInventory(item);
            if (slot != null)
                ClearInventorySlot(slot);
        }

        public void RemoveItemAt(int index)
        {
            var slot = InventorySlots[index];
            if (slot != null)
                ClearInventorySlot(slot);
        }

        private void ClearInventorySlot(InventorySlot slot)
        {
            slot.ClearSlot();
            _onUpdateInventorySlotUI.Raise(slot);
        }

        public bool HasEmptySlot()
        {
            foreach (var slot in InventorySlots)
            {
                if (slot.IsEmpty && !slot.IsLocked)
                    return true;
            }

            return false;
        }

        private InventorySlot FindItemInInventory(ItemSO item)
        {
            foreach (var slot in InventorySlots)
            {
                if (slot.Item == item)
                    return slot;
            }
            return null;
        }

        private InventorySlot SetEmptySlot(ItemSO item, int amount)
        {
            foreach (var slot in InventorySlots)
            {
                if (slot.IsEmpty && !slot.IsLocked)
                {
                    slot.SetItem(item, amount);
                    return slot;
                }
            }

            return null;
        }

        [ContextMenu(nameof(ClearInventory))]
        public void ClearInventory()
        {
            foreach (var slot in InventorySlots)
                slot.ClearSlot();
        }
    }
}