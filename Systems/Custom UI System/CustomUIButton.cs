using DG.Tweening;
using Maledictus.GameMenu;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using VInspector;

namespace Maledictus.CustomUI
{
    public class CustomUIButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        public Action OnClick { get; set; }

        [Header("UI Binding")]
        [SerializeField] private CustomText _buttonText;
        [SerializeField] private NotificationUI _notificationUI;

        public CustomText ButtonText => _buttonText;

        [Space(15f)]
        [Header("Override UI")]
        [SerializeField] private bool _override = true;

        [ShowIf(nameof(_override), true)]

        [Space(10f)]
        [SerializeField] public string _buttonName;

        [EndIf]

        public bool IsSelected { get; private set; } = false;

        private void OnValidate()
        {
            if (_override)
            {
                _buttonText.SetText(_buttonName);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (IsSelected) return;

            _buttonText.SetSelectedTextColor(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (IsSelected) return;

            _buttonText.SetSelectedTextColor(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (IsSelected) return;

            HandleSelect();
            OnClick?.Invoke();
        }

        public void HandleSelect()
        {
            IsSelected = true;
            _buttonText.SetSelectedTextColor(true);
        }

        public void HandleDeselect()
        {
            IsSelected = false;
            _buttonText.SetSelectedTextColor(false);
        }

        public void DisplayNotification(bool visible) => _notificationUI.DisplayNotification(visible);
    }
}