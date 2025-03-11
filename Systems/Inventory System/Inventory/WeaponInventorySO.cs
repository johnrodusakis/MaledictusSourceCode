using Maledictus.CustomSoap;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

namespace Maledictus.Inventory
{
    [CreateAssetMenu(menuName = "Inventory System/Inventory/Weapon Inventory")]
    public class WeaponInventorySO : InventorySO<WeaponItemSO>
    {
        [Tab("Inventory")]
        public GearInventorySO<WeaponItemSO> GearInventory = new(2);
        public List<ItemSlot<WeaponItemSO>> WeaponSlots = new();

        [Tab("Events")]
        [SerializeField] private ScriptableEventWeaponSlot _onWeaponSelected;

        public override GearInventorySO<WeaponItemSO> GearInventorySO { get => GearInventory; set => GearInventory = value; }
        public override List<ItemSlot<WeaponItemSO>> ItemSlots { get => WeaponSlots; set => WeaponSlots = value; }

        protected override void OnEnable()
        {
            base.OnEnable();

            _onWeaponSelected.OnRaised += HandleSelectedItem;
        }

        protected override void OnDisable()
        {
            base.OnEnable();

            _onWeaponSelected.OnRaised -= HandleSelectedItem;
        }

        [ContextMenu("Clear Inventory")]
        protected override void ClearInventory()
        {
            base.ClearInventory();

            GearInventory.ClearInventory();
            WeaponSlots.Clear();
        }
    }
}