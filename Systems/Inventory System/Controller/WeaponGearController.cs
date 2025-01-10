using Maledictus.Events;
using Obvious.Soap;
using UnityEngine;
using VInspector;

namespace Maledictus.Inventory
{
    public class WeaponGearController : MonoBehaviour
    {
        [Tab("Controller")]
        [SerializeField] private WeaponInventorySO _weaponInventorySO;
        [SerializeField] private GameObject _weaponListObject;

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
            if(Input.GetKeyDown(KeyCode.Space))
                _weaponInventorySO.ToggleSort();
        }

        private void UpdateGearSlotUI()
        {
            _weaponListObject.SetActive(false);

            for (int i = 0; i < this.transform.childCount; i++)
            {
                var slotUI = this.transform.GetChild(i).GetComponent<WeaponGearSlotUI>();
                slotUI.InitializeSlotUI(_weaponInventorySO.GearInventory.Slots[i], _weaponListObject);
            }
        }
    }
}