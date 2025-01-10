using UnityEngine;

namespace Maledictus.Inventory
{
    [System.Serializable]
    public class GearSlotData
    {
        public ItemType ItemType;
        public GameObject SlotObject;
        public int SlotIndex;

        public GearSlotData(ItemType type, GameObject go, int index)
        {
            ItemType = type;
            SlotObject = go;
            SlotIndex = index;
        }
    }
}