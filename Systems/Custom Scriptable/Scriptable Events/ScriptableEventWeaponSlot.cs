using Maledictus.Inventory;
using Obvious.Soap;
using UnityEngine;

namespace Maledictus.CustomSoap
{
    [CreateAssetMenu(menuName = "Soap/ScriptableEvents/Inventory/ItemSlot/WeaponItemSO")]
    public class ScriptableEventWeaponSlot : ScriptableEvent<ItemSlot<WeaponItemSO>>
    {

    }
}