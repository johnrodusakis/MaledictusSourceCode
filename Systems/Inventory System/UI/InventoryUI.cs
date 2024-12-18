using Maledictus.Events;
using Obvious.Soap;
using System.Collections.Generic;
using UnityEngine;

namespace Maledictus.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private InventorySO _inventorySO;
        [SerializeField] private InventoryItemSlotUI _itemSlot;

        [Space(15f)]
        [Header("Events")]
        [SerializeField] private ScriptableEvent<InventorySlot> _onUpdateInventorySlotUI;

        private void Start() => InitializeInventoryUI();

        private void OnEnable()
        {
            _onUpdateInventorySlotUI.OnRaised += UpdateInventorySlotUI;
        }

        private void OnDisable()
        {
            _onUpdateInventorySlotUI.OnRaised -= UpdateInventorySlotUI;
        }

        private void UpdateInventorySlotUI(InventorySlot slot)
        {
            var slotIndex = _inventorySO.GetInventorySlotIndex(slot);
            if(slotIndex >= 0)
            {
                InventoryItemSlotUI slotUI;

                if(slotIndex < transform.childCount)
                    slotUI = this.transform.GetChild(slotIndex).GetComponent<InventoryItemSlotUI>();
                else
                    slotUI = Instantiate(_itemSlot, this.transform);

                slotUI.CreateSlot(_inventorySO.InventorySlots[slotIndex], this);
            }
        }

        public void InitializeInventoryUI()
        {
            int childCount = this.transform.childCount;
            int slotCount = _inventorySO.InventorySlots.Count;

            // Create new slots if needed
            if (slotCount > childCount)
            {
                for (int i = childCount; i < slotCount; i++)
                {
                    var slotUI = Instantiate(_itemSlot, this.transform);
                    slotUI.CreateSlot(_inventorySO.InventorySlots[i], this);
                }
            }
            // Disable extra slots if needed
            else if (slotCount < childCount)
            {
                for (int i = slotCount; i < childCount; i++)
                    this.transform.GetChild(i).gameObject.SetActive(false);
            }

            // Update existing slots
            for (int i = 0; i < slotCount; i++)
            {
                var slotUI = this.transform.GetChild(i).GetComponent<InventoryItemSlotUI>();
                slotUI.CreateSlot(_inventorySO.InventorySlots[i], this);
                slotUI.gameObject.SetActive(true);
            }
        }

        public void SwapItems(InventorySlot draggedSlot, InventorySlot droppedSlot)
        {
            if (draggedSlot.Item.Type != droppedSlot.Item.Type) return;
            _inventorySO.SwapItems(draggedSlot, droppedSlot);
        }

        public bool TryRemoveItem(ItemSlotUI itemSlotUI, ItemSO item)
        {
            if (_inventorySO.TryEquipItem(item))
            {
                var slotIndex = SelectedSlotIndex(itemSlotUI);
                _inventorySO.RemoveItemAt(slotIndex);

                return true;
            }

            return false;
        }

        private int SelectedSlotIndex(ItemSlotUI itemSlotUI)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).TryGetComponent(out ItemSlotUI slotUI))
                {
                    if (itemSlotUI == slotUI)
                        return i;
                }
            }

            Debug.LogError($"The Selected Slot {itemSlotUI.name} does not exists in the Inventory");
            return 0;
        }
    }
}