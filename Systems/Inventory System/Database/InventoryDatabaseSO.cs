using UnityEngine;

namespace Maledictus.Inventory
{
    [CreateAssetMenu(menuName = "Inventory System/Database/Inventory Database")]
    public class InventoryDatabaseSO : ScriptableObject
    {
        public WeaponInventorySO WeaponInventorySO;

        public AccessoryInventorySO NecklaceInventorySO;
        public AccessoryInventorySO BraceletInventorySO;
        public AccessoryInventorySO RingInventorySO;
    }
}