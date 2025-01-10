using UnityEngine;

namespace Maledictus.Inventory
{
    public enum AccessoryType
    {
        Necklace,
        Bracelet,
        Ring,
    }

    [CreateAssetMenu(menuName = "Inventory System/Item/Accessory")]
    public class AccessoryItemSO : ItemSO
    {
        public AccessoryType AccessoryType;

        [Min(1)] public int Armor;

        private void Awake()
        {
            Category = (int)AccessoryType;
            ItemType = ItemType.Accessory;
            Stackable = false;
        }
    }
}