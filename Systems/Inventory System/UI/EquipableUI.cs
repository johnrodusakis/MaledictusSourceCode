using Obvious.Soap;
using UnityEngine;
using VInspector;

namespace Maledictus.Inventory
{
    public class EquipableUI : MonoBehaviour
    {
        [SerializeField] private InventorySO _inventorySO;
        [SerializeField] private EquipableItemSlotUI _itemSlot;

        [Space(15f)]
        [Header("Events")]
        [SerializeField] private ScriptableEvent<InventorySlot> _onUpdateInventorySlotUI;

        private void Start() => InitializeInventoryUI();

        private void InitializeInventoryUI()
        {
            RemoveExistingChildren();

            for (int i = 0; i < _inventorySO.InventorySlots.Count; i++)
            {
                var inventorySlot = _inventorySO.InventorySlots[i];

                if (!inventorySlot.IsEmpty)
                {
                    var slotUI = Instantiate(_itemSlot, this.transform);
                    slotUI.CreateSlot(inventorySlot, this);
                }
            }
        }

        private void RemoveExistingChildren()
        {
            foreach(Transform child in transform)
                Destroy(child.gameObject);
        }
    }
}