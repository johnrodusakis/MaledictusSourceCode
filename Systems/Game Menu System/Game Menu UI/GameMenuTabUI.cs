using DG.Tweening;
using Maledictus.CustomUI;
using Maledictus.CustomSoap;
using UnityEngine;
using UnityEngine.EventSystems;
using VInspector;

namespace Maledictus.GameMenu
{
    public class GameMenuTabUI : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        [SerializeField] private GameMenuTab _gameMenuTab; 

        [Space(15f)]
        [Header("UI Binding")]
        [SerializeField] private CustomText _tabText;
        [SerializeField] private NotificationUI _notificationUI;

        [Space(15f)]
        [Header("Events")]
        [SerializeField] private ScriptableEventGameMenuTab _onGameMenuTabSelected;

        [Space(15f)]
        [Header("Override UI")]
        [SerializeField] private bool _override = true;

        [ShowIf(nameof(_override), true)]

        [Space(10f)]
        [SerializeField] public string _tabName;

        [EndIf]

        private bool _isSelected = false;

        private void OnValidate()
        {
            _tabText.SetText(_gameMenuTab.ToSpacedString());

            if (_override)
                _tabText.SetText(_tabName);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_isSelected) return;

            _tabText.SetSelectedTextColor(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isSelected) return;

            _tabText.SetSelectedTextColor(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isSelected) return;

            transform.DOScale(0.9f, 0.05f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                transform.DOScale(1f, 0.05f).SetEase(Ease.InBack).OnComplete(() =>
                {
                    _onGameMenuTabSelected.Raise(_gameMenuTab);
                });
            });

        }

        public void DisplayNotification(bool visible) => _notificationUI.DisplayNotification(visible);
        public void SelectTab() => HandleSelectedTab(true);
        public void DeselectTab() => HandleSelectedTab(false);

        private void HandleSelectedTab(bool isSelected)
        {
            _isSelected = isSelected;
            var scaleMultiplier = isSelected ? 1.2f : 1f;
            transform.localScale = Vector3.one * scaleMultiplier;
            _tabText.SetSelectedTextColor(isSelected);
        }
    }
}