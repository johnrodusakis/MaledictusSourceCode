using DG.Tweening;
using Maledictus.CustomUI;
using Obvious.Soap;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace Maledictus.Inventory
{
    public class AccessoryItemController : MonoBehaviour
    {
        [Tab("Controller")]
        [SerializeField] private AccessoryInventorySO _accessoryInventorySO;
        [SerializeField] private AccessoryItemSlotUI _accessoryItemSlot;
        [SerializeField] private CustomText _emptyHeaderSlot;

        [Tab("Events")]
        [SerializeField] private ScriptableEventString _onSortItems;
        [SerializeField] private ScriptableEventNoParam _onItemSelected;

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
            _onItemSelected.OnRaised += UpdateAccessoryItemSlots;
            _onSortItems.OnRaised += CreateAccessoryItemSlots;
        }

        private void OnDisable()
        {
            _onItemSelected.OnRaised -= UpdateAccessoryItemSlots;
            _onSortItems.OnRaised -= CreateAccessoryItemSlots;
        }

        [Button]
        private void UpdateAccessoryItemSlots()
        {
            var accessoryIndex = 0;

            foreach (Transform child in transform)
            {
                if (child.TryGetComponent(out AccessoryItemSlotUI accessorySlotUI))
                {
                    var accessorySlot = _accessoryInventorySO.AccessorySlots[accessoryIndex];
                    accessorySlotUI.InitializeSlotUI(accessorySlot);
                    accessoryIndex++;
                }
            }
        }

        private void CreateAccessoryItemSlots(string sortType)
        {
            _canvasGroup.DOFade(0f, 0.5f).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                RemoveExistingChildren();
                _scrollRect.verticalNormalizedPosition = 1.5f;

                if (sortType.Equals("Category"))
                {
                    _gridLayoutGroup.padding.top = 150;
                    CreateAccessoryItemSlotsWithHeaderUI();
                }
                else
                {
                    _gridLayoutGroup.padding.top = 250;
                    CreateAccessoryItemSlotsUI();
                }
            });
        }

        private void CreateAccessoryItemSlotsUI()
        {
            for (int i = 0; i < _accessoryInventorySO.AccessorySlots.Count; i++)
            {
                var inventorySlot = _accessoryInventorySO.AccessorySlots[i];

                var slotUI = Instantiate(_accessoryItemSlot, this.transform);
                slotUI.InitializeSlotUI(inventorySlot);
            }

            _canvasGroup.DOFade(1f, 0.5f).SetEase(Ease.InCubic);
        }

        private void CreateAccessoryItemSlotsWithHeaderUI()
        {
            var sortedItems = new Dictionary<string, List<ItemSlot<AccessoryItemSO>>>();

            for (int i = 0; i < _accessoryInventorySO.AccessorySlots.Count; i++)
            {
                var inventorySlot = _accessoryInventorySO.AccessorySlots[i];

                var accessoryCategory = inventorySlot.Item.AccessoryType.ToString();
                if (sortedItems.ContainsKey(accessoryCategory))
                    sortedItems[accessoryCategory].Add(inventorySlot);
                else
                    sortedItems.Add(accessoryCategory, new List<ItemSlot<AccessoryItemSO>>() { inventorySlot });
            }

            var childIndex = 0;
            foreach (var weapon in sortedItems)
            {
                var headerUI = Instantiate(_emptyHeaderSlot, this.transform);
                headerUI.transform.SetSiblingIndex(childIndex);
                headerUI.SetText(weapon.Key);

                childIndex++;
                foreach (var slot in weapon.Value)
                {
                    var slotUI = Instantiate(_accessoryItemSlot, this.transform);
                    slotUI.InitializeSlotUI(slot);
                    slotUI.transform.SetSiblingIndex(childIndex);

                    childIndex++;
                }
            }

            _canvasGroup.DOFade(1f, 0.5f).SetEase(Ease.InCubic);
        }

        private void RemoveExistingChildren()
        {
            foreach (Transform child in transform)
                Destroy(child.gameObject);
        }
    }
}