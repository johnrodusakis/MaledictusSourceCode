using UnityEngine;
using VInspector;

namespace Maledictus.Inventory
{

    public class WeaponGearController : BaseGearController<WeaponItemSO>
    {
        [Tab("Controller")]
        [SerializeField] private WeaponInventorySO _weaponInventorySO;
        [SerializeField] private WeaponItemController _weaponItemController;

        public override InventorySO<WeaponItemSO> InventorySO => _weaponInventorySO;
        public override BaseItemController<WeaponItemSO> ItemController => _weaponItemController;
    }
}