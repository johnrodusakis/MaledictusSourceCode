using Maledictus.Interaction;
using System.Collections.Generic;
using UnityEngine;

namespace Maledictus.Inventory
{
    public class BaseItem : MonoBehaviour, IInteractable
    {
        [SerializeField] private List<ItemSO> _itemSOs;

        private ItemSO SelectRandomItem() => _itemSOs[Random.Range(0, _itemSOs.Count)];

        public void Interact(BaseInventory inventory)
        {
            if (_itemSOs.Count < 1) return;

            inventory.AddItem(SelectRandomItem());
        }

        public string InteractionMessage() => "Collect";
    }
}