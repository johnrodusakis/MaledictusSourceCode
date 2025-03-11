using Obvious.Soap;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

namespace Maledictus.Inventory
{
    public abstract class BaseGearController<T> : MonoBehaviour where T : ItemSO
    {
        [Tab("Events")]
        [SerializeField] private ScriptableEventNoParam _onItemSelected;
        [SerializeField] private ScriptableEventNoParam _onItemNewNotification;
        [SerializeField] private ScriptableEventNoParam _onLoadInventoryData;

        public abstract InventorySO<T> InventorySO { get; }
        public abstract BaseItemController<T> ItemController { get; }

        private readonly List<GearSlotUI<T>> _slotsUI = new();

        private void OnEnable()
        {
            _onLoadInventoryData.OnRaised += InitializeWeaponSlots;

            _onItemNewNotification.OnRaised += DisplayNotification;
            _onItemSelected.OnRaised += UpdateGearSlotUI;
        }

        private void OnDisable()
        {
            _onLoadInventoryData.OnRaised -= InitializeWeaponSlots;

            _onItemNewNotification.OnRaised -= DisplayNotification;
            _onItemSelected.OnRaised -= UpdateGearSlotUI;
        }

        private void Update()
        {
            if (ItemController.IsActive && Input.GetKeyDown(KeyCode.Space))
                InventorySO.ToggleSort();
        }

        private void InitializeWeaponSlots()
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                var slotUI = this.transform.GetChild(i).GetComponent<GearSlotUI<T>>();
                slotUI.InitializeSlotUI(InventorySO.GearInventorySO.Slots[i], ItemController);
                slotUI.DisplayNotification(InventorySO.HasNewItem);

                _slotsUI.Add(slotUI);
            }
        }

        private void UpdateGearSlotUI()
        {
            ItemController.DeactivateItemList();

            for (int i = 0; i < _slotsUI.Count; i++)
                _slotsUI[i].InitializeSlotUI(InventorySO.GearInventorySO.Slots[i], ItemController);
        }

        private void DisplayNotification()
        {
            for (int i = 0; i < _slotsUI.Count; i++)
                _slotsUI[i].DisplayNotification(InventorySO.HasNewItem);
        }
    }
}