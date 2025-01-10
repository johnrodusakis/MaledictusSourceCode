using System;
using UnityEngine;

namespace Maledictus.Inventory
{
    public enum WeaponType
    {
        Swords,
        Axes,
        Scythes,
        Spears,
        Maces,
    }

    [CreateAssetMenu(menuName = "Inventory System/Item/Weapon")]
    public class WeaponItemSO : ItemSO
    {
        public WeaponType WeaponType;

        [Min(1)] public int Damage;

        private void Awake()
        {
            Category = (int)WeaponType;
            ItemType = ItemType.Weapon;
            Stackable = false; 
        }
    }
}