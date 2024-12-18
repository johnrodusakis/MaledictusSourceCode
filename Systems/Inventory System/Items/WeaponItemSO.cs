using UnityEngine;

namespace Maledictus.Inventory
{
    [CreateAssetMenu(menuName = "Inventory/Item/Weapon")]
    public class WeaponItemSO : ItemSO
    {
        public int Damage;

        private void Awake() 
        { 
            Type = ItemType.Weapons; 
            Stackable = false; 
        }
    }
}