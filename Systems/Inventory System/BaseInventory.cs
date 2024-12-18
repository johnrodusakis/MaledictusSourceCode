using Obvious.Soap;
using System.Collections.Generic;
using UnityEngine;

namespace Maledictus.Inventory
{
    public class BaseInventory : MonoBehaviour
    {
        [SerializeField] private List<InventorySO> _inventorySOs;

        public void AddItem(ItemSO itemSO)
        {
            foreach (var inventory in _inventorySOs)
                inventory.AddItem(itemSO, 1);
        }
    }
}