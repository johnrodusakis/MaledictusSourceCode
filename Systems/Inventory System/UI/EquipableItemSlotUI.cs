using Maledictus.CustomUI;
using Maledictus.Events;
using Maledictus.Tooltip;
using Obvious.Soap;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Maledictus.Inventory
{
    public class EquipableItemSlotUI : ItemSlotUI
    {
        [SerializeField] private CustomText _itemNameText;
        [SerializeField] private CustomText _itemDamageText;
        [SerializeField] private CustomText _itemDamageDifferenceText;
        [SerializeField] private CustomImage _itemDamageArrowImage;

        [SerializeField] private CustomImage _itemBlocked;

        [Space(15f)]
        [Header("Events")]
        [SerializeField] private ScriptableEventItemTooltip _onShowItemTooltip;
        [SerializeField] private ScriptableEventNoParam _onHideItemTooltip;

        private EquipableUI _equipableUI;

        public void CreateSlot(InventorySlot inventorySlot, EquipableUI equipableUI)
        {
            _equipableUI = equipableUI;
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
        }

        protected override void HandleLockedSlot()
        {
            _itemBlocked.EnableImage(false);
        }

        protected override void HandleEmptySlot()
        {
            _itemBlocked.EnableImage(false);
        }
    }
}