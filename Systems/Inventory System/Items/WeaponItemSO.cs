using System;
using UnityEngine;

namespace Maledictus.Inventory
{
    public enum WeaponType
    {
        Short,
        Long,
        Great,
        Curved
    }

    [CreateAssetMenu(menuName = "Inventory System/Item/Weapon")]
    public class WeaponItemSO : ItemSO
    {
        public WeaponType WeaponType;

        [Min(1)] public int Damage;

        public string WeaponLabel => string.Concat(WeaponType, " Blades");

        protected override void Awake()
        {
            base.Awake();

            Category = (int)WeaponType;
            ItemType = ItemType.Weapon;
            Stackable = false; 
        }
    }
}