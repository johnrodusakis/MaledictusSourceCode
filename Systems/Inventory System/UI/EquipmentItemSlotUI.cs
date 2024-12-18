using Maledictus.CustomUI;
using Maledictus.Events;
using Maledictus.Tooltip;
using Obvious.Soap;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Maledictus.Inventory
{
    public class EquipmentItemSlotUI : ItemSlotUI
    {
        [SerializeField] private CustomImage _itemLocked;

        [Space(15f)]
        [Header("Events")]
        [SerializeField] private ScriptableEventGameObject _onEquipmentItemSlotSelected;

        [SerializeField] private ScriptableEventItemTooltip _onShowItemTooltip;
        [SerializeField] private ScriptableEventNoParam _onHideItemTooltip;

        private EquipmentUI _equipmentUI;
        private Sprite _itemPlaceholderSprite;

        protected void OnEnable()
        {
            InputManager.OnUnequipItem += UnequipItem;
        }

        protected void OnDisable()
        {
            InputManager.OnUnequipItem -= UnequipItem;
        }

        private void UnequipItem()
        {
            if (!_isSelected || !_isHovered) return;

            _equipmentUI.UnequipItem(this, _inventorySlot.Item);
            _onHideItemTooltip.Raise();
        }

        public void CreateSlot(InventorySlot inventorySlot, EquipmentUI equipmentUI, Sprite itemPlaceholderImage)
        {
            _equipmentUI = equipmentUI;
            _itemPlaceholderSprite = itemPlaceholderImage;

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

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            _onEquipmentItemSlotSelected?.Raise(transform.GetChild(0).gameObject);
        }

        protected override void HandleOccupiedSlot()
        {
            _itemLocked.EnableImage(false);
        }

        protected override void HandleLockedSlot()
        {
            _itemIconImage.SetImage(_itemPlaceholderSprite);
            _itemIconImage.SetAlpha(0.01f);
            _itemIconImage.EnableImage(true);

            _itemLocked.EnableImage(true);
        }

        protected override void HandleEmptySlot()
        {
            _itemIconImage.SetImage(_itemPlaceholderSprite);
            _itemIconImage.SetAlpha(0.01f);
            _itemIconImage.EnableImage(true);

            _itemLocked.EnableImage(false);
        }
    }
}