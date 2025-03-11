using System.Collections.Generic;
using UnityEngine;
using VInspector;

namespace Maledictus.Inventory
{

    public class WeaponItemController : BaseItemController<WeaponItemSO>
    {
        [Tab("Controller")]
        [SerializeField] private WeaponInventorySO _weaponInventorySO;
        [SerializeField] private WeaponItemSlotUI _weaponItemSlotUI;

        public override InventorySO<WeaponItemSO> InventorySO => _weaponInventorySO;
        public override ItemSlotUI<WeaponItemSO> ItemSlotUI => _weaponItemSlotUI;
        public override string CategoryLabel(int index) => _weaponInventorySO.ItemSlots[index].Item.WeaponLabel;
    }
}