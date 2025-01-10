using DG.Tweening;
using Maledictus.Events;
using Maledictus.Inventory;
using Obvious.Soap;
using System;
using UnityEngine;

namespace Maledictus.GameMenu
{

    public class CharacterTabUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _characterGearCanvasGroup;
        [SerializeField] private CanvasGroup _equipNewGearCanvasGroup;

        [SerializeField] private RectTransform _targetItemSlotUI;

        [Space(15f)]
        [Header("Events")]
        [SerializeField] private ScriptableEventGearSlotData _onGearSlotSelected;
        [SerializeField] private ScriptableEventNoParam _onItemSelected;

        private Transform _itemSlotParent;

        private void Start()
        {
            InitializeCanvaGroups();
        }

        private void OnEnable()
        {
            _onGearSlotSelected.OnRaised += HandleSelectedGearSlot;
            _onItemSelected.OnRaised += ItemSelected;
        }

        private void OnDisable()
        {
            _onGearSlotSelected.OnRaised -= HandleSelectedGearSlot;
            _onItemSelected.OnRaised -= ItemSelected;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && _itemSlotParent)
                _onItemSelected.Raise();
        }

        private void HandleSelectedGearSlot(GearSlotData data) => HandleGearSlotMovement(data.SlotObject);

        private void HandleGearSlotMovement(GameObject itemSlotGO)
        {
            _itemSlotParent = itemSlotGO.transform.parent;
            itemSlotGO.transform.SetParent(this.transform);

            EquipNewGearTransition();
            itemSlotGO.GetComponent<RectTransform>().DOMove(_targetItemSlotUI.position, 0.6f).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                itemSlotGO.transform.SetParent(_targetItemSlotUI);
            });
        }

        private void ItemSelected()
        {
            var itemSlotGO = _targetItemSlotUI.GetChild(0).gameObject;
            itemSlotGO.transform.SetParent(this.transform);

            CharacterGearTransition();
            itemSlotGO.GetComponent<RectTransform>().DOMove(_itemSlotParent.position, 0.6f).SetEase(Ease.InCubic).OnComplete(() =>
            {
                itemSlotGO.transform.SetParent(_itemSlotParent);
                _itemSlotParent = null;
            });
        }

        private void InitializeCanvaGroups()
        {
            _characterGearCanvasGroup.alpha = 1f;
            EnableCanvasGroup(_characterGearCanvasGroup, true);

            _equipNewGearCanvasGroup.alpha = 0f;
            EnableCanvasGroup(_equipNewGearCanvasGroup, false);
        }

        private void EquipNewGearTransition()
        {
            EnableCanvasGroup(_characterGearCanvasGroup, false);
            _characterGearCanvasGroup.DOFade(0.05f, 0.25f).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                _equipNewGearCanvasGroup.DOFade(1f, 0.25f).SetEase(Ease.InCubic).OnComplete(() =>
                {
                    EnableCanvasGroup(_equipNewGearCanvasGroup, true);
                });
            });
        }

        private void CharacterGearTransition()
        {
            EnableCanvasGroup(_equipNewGearCanvasGroup, false);
            _equipNewGearCanvasGroup.DOFade(0f, 0.25f).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                _characterGearCanvasGroup.DOFade(1f, 0.25f).SetEase(Ease.InCubic).OnComplete(() =>
                {
                    EnableCanvasGroup(_characterGearCanvasGroup, true);
                });
            });
        }

        private void EnableCanvasGroup(CanvasGroup canvasGroup, bool enable)
        {
            if (enable)
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
            else
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
        }
    }
}