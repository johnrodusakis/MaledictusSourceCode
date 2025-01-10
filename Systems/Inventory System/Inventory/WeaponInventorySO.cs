using Maledictus.Events;
using Obvious.Soap;
using System;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

namespace Maledictus.Inventory
{
    [CreateAssetMenu(menuName = "Inventory System/Inventory/Weapon Inventory")]
    public class WeaponInventorySO : InventorySO<WeaponItemSO>
    {
        [Tab("Inventory")]
        public GearInventorySO<WeaponItemSO> GearInventory = new(3);
        public List<ItemSlot<WeaponItemSO>> WeaponSlots = new();

        [Tab("Events")]
        [SerializeField] private ScriptableEventWeaponSlot _onWeaponSelected;


        protected override void OnEnable()
        {
            base.OnEnable();

            _onGearSlotSelected.OnRaised += HandleGearSlotSelected;
            _onWeaponSelected.OnRaised += HandleSelectedWeapon;
        }

        protected override void OnDisable()
        {
            base.OnEnable();

            _onGearSlotSelected.OnRaised += HandleGearSlotSelected;
            _onWeaponSelected.OnRaised += HandleSelectedWeapon;
        }

        private void HandleGearSlotSelected(GearSlotData data)
        {
            GearInventory.SetSelectedSlotIndex(data.SlotIndex);
            SortItems();
        }

        private void HandleSelectedWeapon(ItemSlot<WeaponItemSO> slot)
        {
            var selectedSlot = GearInventory.GetSelectedSlot();
            if(selectedSlot.IsSelected)
            {
                var weaponSlot = WeaponSlots.Find(s => s.Order == selectedSlot.Order);
                weaponSlot.DeselectSlot();
            }

            var newWeaponSlot = WeaponSlots.Find(s => s == slot);
            newWeaponSlot.SelectSlot();

            GearInventory.SetSelectedSlot(slot);

            _onItemSelected.Raise();
        }

        protected override List<ItemSlot<WeaponItemSO>> GetListOfItems() => WeaponSlots;

        protected override void OnItemCollected(WeaponItemSO item)
        {
            var weaponSlot = new ItemSlot<WeaponItemSO>();
            weaponSlot.SetItem(item, _orderCounter);

            WeaponSlots.Add(weaponSlot);
            SortItems();
        }


        [ContextMenu(nameof(ClearInventory))]
        protected override void ClearInventory()
        {
            base.ClearInventory();

            GearInventory.ClearInventory();
            WeaponSlots.Clear();
        }
    }
}