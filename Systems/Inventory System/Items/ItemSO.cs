using UnityEngine;

namespace Maledictus.Inventory
{
    public enum ItemType
    {
        Weapon,
        Accessory,
        Technique,
        Monster,
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
        public string Id { get; private set; }

        public string Name;
        public Sprite Icon;            
        public string Description;
        public Rarity ItemRarity;
        [Min(1)] public int Level;

        public int Category { get; protected set; }
        public ItemType ItemType { get; protected set; }
        public bool Stackable { get; protected set; }

        protected virtual void Awake()
        {
            Id = name;
        }
    }
}