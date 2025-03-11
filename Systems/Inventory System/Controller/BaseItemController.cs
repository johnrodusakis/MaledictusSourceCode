using DG.Tweening;
using Maledictus.CustomUI;
using Obvious.Soap;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace Maledictus.Inventory
{
    public abstract class BaseItemController<T> : MonoBehaviour where T : ItemSO
    {
        [Tab("Controller")]
        [SerializeField] protected CustomText _emptyHeaderSlot;

        [Space(10f)]
        [Tab("Events")]
        [SerializeField] protected ScriptableEventNoParam _onItemSelected;
        [SerializeField] protected ScriptableEventString _onSortItems;

        public bool IsActive { get; private set; } = false;

        public abstract InventorySO<T> InventorySO { get; }
        public abstract ItemSlotUI<T> ItemSlotUI { get; }
        public abstract string CategoryLabel(int index);


        protected ScrollRect _scrollRect;
        protected CanvasGroup _canvasGroup;

        protected GridLayoutGroup _childGridLayoutGroup;
        protected CanvasGroup _childCanvasGroup;

        private bool _isInitialized = false;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();

            _scrollRect = this.transform.GetChild(0).GetComponent<ScrollRect>();

            var child = this.transform.GetChild(0).GetChild(0).GetChild(0);
            _childGridLayoutGroup = child.GetComponent<GridLayoutGroup>();
            _childCanvasGroup = child.GetComponent<CanvasGroup>();

            DeactivateItemList();
        }

        private void OnEnable()
        {
            _onItemSelected.OnRaised += UpdateItemSlots;
            _onSortItems.OnRaised += CreateItemSlots;
        }

        private void OnDisable()
        {
            _onItemSelected.OnRaised -= UpdateItemSlots;
            _onSortItems.OnRaised -= CreateItemSlots;
        }

        public void ActivateItemList()
        {
            _canvasGroup.DOFade(1f, 0.25f).OnComplete(() =>
            {
                IsActive = true;

                _canvasGroup.blocksRaycasts = IsActive;
                _canvasGroup.interactable = IsActive;

                if(!_isInitialized)
                    CreateItemSlots("Category");
            });
        }

        public void DeactivateItemList()
        {
            IsActive = false;

            _canvasGroup.blocksRaycasts = IsActive;
            _canvasGroup.interactable = IsActive;
            _canvasGroup.alpha = 0;
        }

        private void UpdateItemSlots()
        {
            var index = 0;

            foreach (Transform child in _childGridLayoutGroup.transform)
            {
                if (child.TryGetComponent(out ItemSlotUI<T> gearSelectionItemSlotUI))
                {
                    var slot = InventorySO.ItemSlots[index];
                    gearSelectionItemSlotUI.InitializeSlotUI(slot);
                    index++;
                }
            }
        }

        private void CreateItemSlots(string sortType)
        {
            if (!IsActive) return;

            if (!_isInitialized)
                _isInitialized = true;

            _childCanvasGroup.DOFade(0f, 0.25f).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                RemoveExistingChildren();
                _scrollRect.verticalNormalizedPosition = 1.5f;

                if (sortType.Equals("Category"))
                {
                    _childGridLayoutGroup.padding.top = 150;
                    CreateItemSlotsWithHeaderUI();
                }
                else
                {
                    _childGridLayoutGroup.padding.top = 250;
                    CreateItemSlotsUI();
                }
            });
        }

        private void CreateItemSlotsUI()
        {
            _childCanvasGroup.alpha = 0f;

            for (int i = 0; i < InventorySO.ItemSlots.Count; i++)
            {
                var inventorySlot = InventorySO.ItemSlots[i];

                var slotUI = Instantiate(ItemSlotUI, _childGridLayoutGroup.transform);
                slotUI.InitializeSlotUI(inventorySlot);
            }

            _childCanvasGroup.DOFade(1f, 0.25f).SetEase(Ease.InCubic);
        }

        private void CreateItemSlotsWithHeaderUI()
        {
            _childCanvasGroup.alpha = 0f;

            var sortedItems = new Dictionary<string, List<ItemSlot<T>>>();

            for (int i = 0; i < InventorySO.ItemSlots.Count; i++)
            {
                var inventorySlot = InventorySO.ItemSlots[i];

                if (sortedItems.ContainsKey(CategoryLabel(i)))
                    sortedItems[CategoryLabel(i)].Add(inventorySlot);
                else
                    sortedItems.Add(CategoryLabel(i), new List<ItemSlot<T>>() { inventorySlot });
            }

            var childIndex = 0;
            foreach (var item in sortedItems)
            {
                var headerUI = Instantiate(_emptyHeaderSlot, _childGridLayoutGroup.transform);
                headerUI.transform.SetSiblingIndex(childIndex);
                headerUI.SetText(item.Key);

                childIndex++;
                foreach (var slot in item.Value)
                {
                    var slotUI = Instantiate(ItemSlotUI, _childGridLayoutGroup.transform);
                    slotUI.InitializeSlotUI(slot);
                    slotUI.transform.SetSiblingIndex(childIndex);

                    childIndex++;
                }
            }

            _childCanvasGroup.DOFade(1f, 0.5f).SetEase(Ease.InCubic);
        }

        private void RemoveExistingChildren()
        {
            foreach (Transform child in _childGridLayoutGroup.transform)
                Destroy(child.gameObject);
        }
    }
}