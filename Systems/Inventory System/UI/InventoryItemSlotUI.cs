using Maledictus.CustomUI;
using Maledictus.Events;
using Maledictus.Tooltip;
using Obvious.Soap;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Maledictus.Inventory
{
    public class InventoryItemSlotUI : ItemSlotUI
    {
        [SerializeField] private CustomText _itemAmountText;
        [SerializeField] private CustomImage _itemBlocked;

        [Space(15f)]
        [Header("Events")]
        [SerializeField] private ScriptableEventItemTooltip _onShowItemTooltip;
        [SerializeField] private ScriptableEventNoParam _onHideItemTooltip;

        private InventoryUI _inventoryUI;

        protected void OnEnable()
        {
            InputManager.OnEquipItem += TryEquipItem;
        }

        protected void OnDisable()
        {
            InputManager.OnEquipItem -= TryEquipItem;
        }

        private void TryEquipItem()
        {
            if (!_isSelected || !_isHovered) return;

            if(_inventoryUI.TryRemoveItem(this, _inventorySlot.Item))
                _onHideItemTooltip.Raise();
        }

        public void CreateSlot(InventorySlot inventorySlot, InventoryUI inventoryUI)
        {
            _inventoryUI = inventoryUI;
            CreateSlot(inventorySlot);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            var item = _inventorySlot.Item;

            if (item != null)
                _onShowItemTooltip.Raise(new ItemTooltip(transform.position, item.name, 0, item.Rarity, item.name));


            base.OnPointerEnter(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            _onHideItemTooltip.Raise();

            base.OnPointerExit(eventData);
        }

        protected override void HandleOccupiedSlot()
        {
            _itemBlocked.EnableImage(false);

            var amountText = _inventorySlot.Item.Stackable ? AbbreviateNumbers.AbbreviateNumber(_inventorySlot.Amount) : string.Empty;
            _itemAmountText.SetText(amountText);
        }

        protected override void HandleLockedSlot()
        {
            _itemBlocked.EnableImage(false);

            _itemAmountText.SetText(string.Empty);
        }

        protected override void HandleEmptySlot()
        {
            _itemBlocked.EnableImage(false);

            _itemAmountText.SetText(string.Empty);
        }
    }
}