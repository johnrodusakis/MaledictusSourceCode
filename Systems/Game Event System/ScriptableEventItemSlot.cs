using Maledictus.Inventory;
using Obvious.Soap;
using UnityEngine;

namespace Maledictus.Events
{
    //[CreateAssetMenu(menuName = "Soap/ScriptableEvents/Inventory/Gear Slot")]
    //public class ScriptableEventGearSlot<T> : ScriptableEvent<GearSlot<T>>
    //{

    //}

    [CreateAssetMenu(menuName = "Soap/ScriptableEvents/Inventory/ItemSlot/ItemSO")]
    public class ScriptableEventItemSlot : ScriptableEvent<ItemSlot<ItemSO>>
    {

    }
}