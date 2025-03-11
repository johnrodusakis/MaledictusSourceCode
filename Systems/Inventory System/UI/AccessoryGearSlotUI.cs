using Maledictus.CustomSoap;
using UnityEngine;
using UnityEngine.EventSystems;
using VInspector;

namespace Maledictus.Inventory
{
    public class AccessoryGearSlotUI : GearSlotUI<AccessoryItemSO>
    {
        [Tab("Events")]
        [SerializeField] private ScriptableEventGearSlotData _onGearSlotSelected;

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (_gearSlot.IsLocked) return;

            base.OnPointerClick(eventData);

            _onGearSlotSelected.Raise(new GearSlotData(ItemType.Accessory, transform.GetChild(0).gameObject, 0));
        }
    }
}