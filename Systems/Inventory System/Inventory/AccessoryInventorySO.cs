using Maledictus.CustomSoap;
using SomniaGames.Persistence;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

namespace Maledictus.Inventory
{
    [CreateAssetMenu(menuName = "Inventory System/Inventory/Cosmetic Inventory")]
    public class AccessoryInventorySO : InventorySO<AccessoryItemSO>
    {
        [Tab("Inventory")]
        public GearInventorySO<AccessoryItemSO> GearInventory = new(1);
        public List<ItemSlot<AccessoryItemSO>> AccessorySlots = new();

        [Tab("Events")]
        [SerializeField] private ScriptableEventAccessorySlot _onAccessorySelected;

        public override GearInventorySO<AccessoryItemSO> GearInventorySO { get => GearInventory; set => GearInventory = value; }
        public override List<ItemSlot<AccessoryItemSO>> ItemSlots { get => AccessorySlots; set => AccessorySlots = value; }

        protected override void OnEnable()
        {
            base.OnEnable();

            _onAccessorySelected.OnRaised += HandleSelectedItem;
        }

        protected override void OnDisable()
        {
            base.OnEnable();

            _onAccessorySelected.OnRaised -= HandleSelectedItem;
        }

        [ContextMenu("Clear Inventory")]
        protected override void ClearInventory()
        {
            base.ClearInventory();

            GearInventory.ClearInventory();
            AccessorySlots.Clear();
        }
    }
}