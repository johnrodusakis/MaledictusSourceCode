using Maledictus.Events;
using Maledictus.Tooltip;
using Obvious.Soap;
using UnityEngine;
using UnityEngine.EventSystems;
using VInspector;

namespace Maledictus.Inventory
{

    public class WeaponGearSlotUI : GearSlotUI<WeaponItemSO>
    {
        [Tab("Events")]
        [SerializeField] private ScriptableEventGearSlotData _onGearSlotSelected;

        [SerializeField] private ScriptableEventItemTooltip _onShowItemTooltip;
        [SerializeField] private ScriptableEventNoParam _onHideItemTooltip;

        public void InitializeSlotUI(GearSlot<WeaponItemSO> slot, GameObject itemListObject)
        {
            _item = slot.SelectedSlot.Item;
            _gearSlot = slot;

            _itemListObject = itemListObject;

            var isSlotAvailable = !slot.SelectedSlot.IsEmpty && !slot.IsLocked;
            var isSlotLocked = slot.IsLocked;

            var weaponItem = slot.SelectedSlot.Item;
            if (weaponItem != null)
                InitializeGearSlotUI(isSlotAvailable, isSlotLocked, weaponItem.Icon, weaponItem.ItemRarity);
            else
                InitializeGearSlotUI(isSlotAvailable, isSlotLocked, null, ItemRarity.Common);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (_item != null)
                _onShowItemTooltip.Raise(new ItemTooltip(transform.position, _item.Name, _item.Level, _item.ItemRarity, _item.WeaponType.ToString()));

            base.OnPointerEnter(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            _onHideItemTooltip.Raise();

            base.OnPointerExit(eventData);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (_gearSlot.IsLocked) return;

            base.OnPointerClick(eventData);

            _onGearSlotSelected.Raise(new GearSlotData(ItemType.Weapon, transform.GetChild(0).gameObject, transform.GetSiblingIndex()));
        }
    }
}