using Maledictus.CustomSoap;
using Obvious.Soap;
using UnityEngine;
using UnityEngine.EventSystems;
using VInspector;

namespace Maledictus.Inventory
{

    public class WeaponItemSlotUI : ItemSlotUI<WeaponItemSO>
    {
        [Tab("Events")]
        [SerializeField] private ScriptableEventWeaponSlot _onWeaponSelected;

        public override string StatusLabel => "Atk Dmg";
        public override float StatusValue => _itemSlot.Item.Damage;

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            _onWeaponSelected.Raise(_itemSlot);
        }
    }
}