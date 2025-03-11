using DG.Tweening;
using Maledictus.CustomUI;
using Obvious.Soap;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace Maledictus.Inventory
{
    public class AccessoryItemController : BaseItemController<AccessoryItemSO>
    {
        [Tab("Controller")]
        [SerializeField] private AccessoryInventorySO _accessoryInventorySO;
        [SerializeField] private AccessoryItemSlotUI _accessoryItemSlotUI;

        public override InventorySO<AccessoryItemSO> InventorySO => _accessoryInventorySO;
        public override ItemSlotUI<AccessoryItemSO> ItemSlotUI => _accessoryItemSlotUI;
        public override string CategoryLabel(int index) => _accessoryInventorySO.AccessorySlots[index].Item.CategoryLabel;
    }
}