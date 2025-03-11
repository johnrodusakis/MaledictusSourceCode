using Obvious.Soap;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

namespace Maledictus.Inventory
{
    public class AccessoryGearController : BaseGearController<AccessoryItemSO>
    {
        [Tab("Controller")]
        [SerializeField] private AccessoryInventorySO _accessoryInventorySO;
        [SerializeField] private AccessoryItemController _accessoryItemController;

        public override InventorySO<AccessoryItemSO> InventorySO => _accessoryInventorySO;
        public override BaseItemController<AccessoryItemSO> ItemController => _accessoryItemController;
    }
}