using UnityEngine;

namespace Maledictus.Inventory
{
    public enum ItemType
    {
        Weapons,
        Cosmetics,
        Techniques,
        Monsters,
        Gold,
        Trash,
    }

    public enum Rarity
    {
        Common,     // White
        Uncommon,   // Green
        Rare,       // Blue
        Mythic,     // Purple
        Legendary   // Orange
    }

    public abstract class ItemSO : ScriptableObject
    {
        public string Name;
        public int Level;
            
        public Sprite Icon;
        public Rarity Rarity;

        public ItemType Type { get; protected set; }
        public bool Stackable { get; protected set; }
    }
}