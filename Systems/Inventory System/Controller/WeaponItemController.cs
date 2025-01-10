using DG.Tweening;
using Maledictus.CustomUI;
using Obvious.Soap;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace Maledictus.Inventory
{
    public class WeaponItemController : MonoBehaviour
    {
        [Tab("Controller")]
        [SerializeField] private WeaponInventorySO _weaponInventorySO;
        [SerializeField] private WeaponItemSlotUI _weaponItemSlot;
        [SerializeField] private CustomText _emptyHeaderSlot;

        [Tab("Events")]
        [SerializeField] private ScriptableEventNoParam _onItemSelected;
        [SerializeField] private ScriptableEventString _onSortItems;

        private ScrollRect _scrollRect;
        private GridLayoutGroup _gridLayoutGroup;
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _scrollRect = transform.parent.parent.GetComponent<ScrollRect>();
            _gridLayoutGroup = GetComponent<GridLayoutGroup>();
            _canvasGroup = _scrollRect.GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            _onItemSelected.OnRaised += UpdateWeaponItemSlots;
            _onSortItems.OnRaised += CreateWeaponItemSlots;
        }

        private void OnDisable()
        {
            _onItemSelected.OnRaised -= UpdateWeaponItemSlots;
            _onSortItems.OnRaised -= CreateWeaponItemSlots;
        }

        [Button]
        private void UpdateWeaponItemSlots()
        {
            var weaponIndex = 0;

            foreach (Transform child in transform)
            {
                if(child.TryGetComponent(out WeaponItemSlotUI weaponGearSelectionItemSlotUI))
                {
                    var weaponSlot = _weaponInventorySO.WeaponSlots[weaponIndex];
                    weaponGearSelectionItemSlotUI.InitializeSlot(weaponSlot);
                    weaponIndex++;
                }
            }
        }

        private void CreateWeaponItemSlots(string sortType)
        {
            _canvasGroup.DOFade(0f, 0.5f).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                RemoveExistingChildren();
                _scrollRect.verticalNormalizedPosition = 1.5f;

                if (sortType.Equals("Category"))
                {
                    _gridLayoutGroup.padding.top = 150;
                    CreateWeaponItemSlotsWithHeaderUI();
                }
                else
                {
                    _gridLayoutGroup.padding.top = 250;
                    CreateWeaponItemSlotsUI();
                }
            });
        }

        private void CreateWeaponItemSlotsUI()
        {
            for (int i = 0; i < _weaponInventorySO.WeaponSlots.Count; i++)
            {
                var inventorySlot = _weaponInventorySO.WeaponSlots[i];

                var slotUI = Instantiate(_weaponItemSlot, this.transform);
                slotUI.InitializeSlot(inventorySlot);
            }

            _canvasGroup.DOFade(1f, 0.5f).SetEase(Ease.InCubic);
        }

        private void CreateWeaponItemSlotsWithHeaderUI()
        {
            var sortedWeapons = new Dictionary<string, List<ItemSlot<WeaponItemSO>>>();

            for (int i = 0; i < _weaponInventorySO.WeaponSlots.Count; i++)
            {
                var inventorySlot = _weaponInventorySO.WeaponSlots[i];

                var weaponCategory = inventorySlot.Item.WeaponType.ToString();
                if (sortedWeapons.ContainsKey(weaponCategory))
                    sortedWeapons[weaponCategory].Add(inventorySlot);
                else
                    sortedWeapons.Add(weaponCategory, new List<ItemSlot<WeaponItemSO>>() { inventorySlot });
            }

            var childIndex = 0;
            foreach (var weapon in sortedWeapons)
            {
                var headerUI = Instantiate(_emptyHeaderSlot, this.transform);
                headerUI.transform.SetSiblingIndex(childIndex);
                headerUI.SetText(weapon.Key);

                childIndex++;
                foreach (var slot in weapon.Value)
                {
                    var slotUI = Instantiate(_weaponItemSlot, this.transform);
                    slotUI.InitializeSlot(slot); 
                    slotUI.transform.SetSiblingIndex(childIndex);

                    childIndex++;
                }
            }

            _canvasGroup.DOFade(1f, 0.5f).SetEase(Ease.InCubic);
        }

        private void RemoveExistingChildren()
        {
            foreach(Transform child in transform)
                Destroy(child.gameObject);
        }
    }
}