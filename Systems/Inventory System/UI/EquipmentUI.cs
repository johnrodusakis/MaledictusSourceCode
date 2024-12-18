using Maledictus.Events;
using Obvious.Soap;
using UnityEngine;
using static UnityEditor.Progress;

namespace Maledictus.Inventory
{
    public class EquipmentUI : MonoBehaviour
    {
        [SerializeField] private EquipmentSO _equipmentSO;
        [SerializeField] private EquipmentItemSlotUI _itemSlot;

        [Space(5f)]
        [SerializeField] private Sprite _itemPlaceholderSprite;

        [Space(15f)]
        [Header("Events")]
        [SerializeField] private ScriptableEvent<InventorySlot> _onUpdateInventorySlotUI;

        private void Start() => InitializeEquipmentUI();

        private void OnEnable()
        {
            _onUpdateInventorySlotUI.OnRaised += UpdateEquipmentSlotUI;
        }

        private void OnDisable()
        {
            _onUpdateInventorySlotUI.OnRaised -= UpdateEquipmentSlotUI;
        }

        public void InitializeEquipmentUI()
        {
            int childCount = this.transform.childCount;
            int slotCount = _equipmentSO.EquipmentSlots.Count;

            // Create new slots if needed
            if (slotCount > childCount)
            {
                for (int i = childCount; i < slotCount; i++)
                {
                    var slotUI = Instantiate(_itemSlot, this.transform);
                    slotUI.CreateSlot(_equipmentSO.EquipmentSlots[i], this, _itemPlaceholderSprite);
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
                var slotUI = this.transform.GetChild(i).GetComponent<EquipmentItemSlotUI>();
                slotUI.CreateSlot(_equipmentSO.EquipmentSlots[i], this, _itemPlaceholderSprite);
                slotUI.gameObject.SetActive(true);
            }
        }

        private void UpdateEquipmentSlotUI(InventorySlot slot)
        {
            var slotIndex = _equipmentSO.GetEquipmentSlotIndex(slot);
            if (slotIndex >= 0)
            {
                var slotUI = this.transform.GetChild(slotIndex).GetComponent<EquipmentItemSlotUI>();
                slotUI.CreateSlot(_equipmentSO.EquipmentSlots[slotIndex], this, _itemPlaceholderSprite);
            }
        }

        public void EquipItem(InventorySlot draggedSlot, InventorySlot droppedSlot)
        {
            if (draggedSlot.Item.Type != droppedSlot.Item.Type) return;
            _equipmentSO.SwapItems(draggedSlot, droppedSlot);
        }

        public void UnequipItem(ItemSlotUI itemSlotUI, ItemSO item)
        {
            var slotIndex = SelectedSlotIndex(itemSlotUI);
            _equipmentSO.RemoveItemAt(slotIndex, item);
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