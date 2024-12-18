using Obvious.Soap;
using System.Collections.Generic;
using UnityEngine;

namespace Maledictus.Inventory
{
    [CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
    public class EquipmentSO : ScriptableObject
    {
        [SerializeField] private InventorySO _inventorySO;

        [Space(15f)]
        public ItemType ItemType;

        [Space(15f)]
        [Header("Events")]
        [SerializeField] private ScriptableEvent<InventorySlot> _onUpdateInventorySlotUI;

        [Space(15f)]
        public List<InventorySlot> EquipmentSlots = new();

        private void OnValidate()
        {
            foreach (var slot in EquipmentSlots)
            {
                if(slot.IsEmpty)
                    slot.Amount = 0;
            }
        }

        public void SwapItems(InventorySlot slot1, InventorySlot slot2)
        {
            var slot1Index = GetEquipmentSlotIndex(slot1);
            var slot2Index = GetEquipmentSlotIndex(slot2);

            if (slot1Index < 0 || slot2Index < 0)
            {
                EquipItem(slot1, slot2);
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
            var slot = EquipmentSlots[index];

            slot.SetItem(item, amount);
            _onUpdateInventorySlotUI.Raise(slot);
        }

        public void EquipItem(InventorySlot slotToEquip, InventorySlot slotToUnequip)
        {
            var equipSlotIndex = _inventorySO.GetInventorySlotIndex(slotToEquip);
            var unequipSlotIndex = GetEquipmentSlotIndex(slotToUnequip);

            if(equipSlotIndex < 0 || unequipSlotIndex < 0)
            {
                EquipItem(slotToUnequip, slotToEquip);
                return;
            }

            var item1 = slotToEquip.Item;
            var item2 = slotToUnequip.Item;

            if (TryEquipAt(unequipSlotIndex, item1))
            {
                if (item2 == null)
                    ClearInventorySlot(slotToEquip);
                else
                    _inventorySO.AddItemAt(equipSlotIndex, item2, 1);
            }
        }

        public bool TryEquipAt(int index, ItemSO item)
        {
            var slot = EquipmentSlots[index];
            if(!slot.IsLocked)
            {
                slot.SetItem(item, 1);
                _onUpdateInventorySlotUI.Raise(slot);
                return true;
            }

            return false;
        }

        public bool TryEquipItem(ItemSO item)
        {
            foreach (var slot in EquipmentSlots)
            {
                if (slot.IsEmpty && !slot.IsLocked)
                {
                    slot.SetItem(item, 1);
                    _onUpdateInventorySlotUI.Raise(slot);
                    return true;
                }
            }

            return false;
        }

        public void RemoveItemAt(int index, ItemSO item)
        {
            var slot = EquipmentSlots[index];

            if (slot != null)
            {
                ClearInventorySlot(slot);
                _inventorySO.AddItem(item, 1);
            }
        }

        private void ClearInventorySlot(InventorySlot slot)
        {
            slot.ClearSlot();
            _onUpdateInventorySlotUI.Raise(slot);
        }

        public int GetEquipmentSlotIndex(InventorySlot inventorySlot)
        {
            for (int i = 0; i < EquipmentSlots.Count; i++)
            {
                if (inventorySlot == EquipmentSlots[i])
                    return i;
            }

            return -1;
        }
    }
}