using Maledictus.Events;
using Obvious.Soap;
using UnityEngine;
using VInspector;

namespace Maledictus.Inventory
{
    public class AccessoryGearController : MonoBehaviour
    {
        [Tab("Controller")]
        [SerializeField] private AccessoryInventorySO[] _accessoryInventorySOs = new AccessoryInventorySO[3];
        [SerializeField] private GameObject[] _accessoryListObjects = new GameObject[3];

        [Tab("Events")]
        [SerializeField] private ScriptableEventNoParam _onItemSelected;

        private void Start() => UpdateGearSlotUI();

        private void OnEnable()
        {
            _onItemSelected.OnRaised += UpdateGearSlotUI;
        }

        private void OnDisable()
        {
            _onItemSelected.OnRaised -= UpdateGearSlotUI;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) 
                SortAccessories();
        }

        private void SortAccessories()
        {
            foreach (var accessoryInventory in _accessoryInventorySOs)
                accessoryInventory.ToggleSort();
        }

        public void UpdateGearSlotUI()
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                _accessoryListObjects[i].SetActive(false);

                var slotUI = this.transform.GetChild(i).GetComponent<AccessoryGearSlotUI>();
                slotUI.InitializeSlotUI(_accessoryInventorySOs[i].GearInventory.Slots[0], _accessoryListObjects[i]);
            }
        }
    }
}