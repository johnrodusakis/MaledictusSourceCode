using Maledictus.CustomSoap;
using Maledictus.Tooltip;
using Obvious.Soap;
using UnityEngine;
using UnityEngine.EventSystems;
using VInspector;

namespace Maledictus.Inventory
{
    public class AccessoryItemSlotUI : ItemSlotUI<AccessoryItemSO>
    {
        [Tab("Events")]
        [SerializeField] private ScriptableEventAccessorySlot _onAccessorySelected;

        public override string StatusLabel => "Armor";
        public override float StatusValue => _itemSlot.Item.Armor;

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            _onAccessorySelected.Raise(_itemSlot);
        }
    }
}