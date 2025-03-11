using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using VInspector;

namespace Maledictus.CustomUI
{
    public class CustomUIFoldout : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        public System.Action OnClick { get; set; }


        [Header("UI Binding")]
        [SerializeField] private CustomImage _arrowImage;
        [SerializeField] private CustomText _buttonText;
        [SerializeField] private CustomImage _notificationImage;

        public CustomText ButtonText => _buttonText;
        public CustomImage NotificationImage => _notificationImage;

        [Space(15f)]
        [Header("Override UI")]
        [SerializeField] private bool _override = true;

        [ShowIf(nameof(_override), true)]

        [Space(10f)]
        [SerializeField] private string _buttonName;

        [EndIf]

        private bool _isSelected = false;

        private void OnValidate()
        {
            if (_override)
            {
                _buttonText.SetText(_buttonName);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_isSelected) return;

            _buttonText.SetSelectedTextColor(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isSelected) return;

            _buttonText.SetSelectedTextColor(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke();
        }

        public void Toggle(GameObject go)
        {
            _isSelected = !_isSelected;
            _arrowImage.RotateImage(_isSelected ? -90f : 0f);
            go.SetActive(_isSelected);

            _buttonText.SetSelectedTextColor(false);
        }
    }
}