using TMPro;
using UnityEngine;
using VInspector;

namespace Maledictus.CustomUI
{
    public class CustomText : CustomUIComponent
    {
        [SerializeField] private CustomTextSO _textData;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private ThemeStyle _style;

        [Space(15f)]
        [Header("Override UI")]
        [SerializeField] private bool _override = false;

        [ShowIf(nameof(_override))]

        [Space(10f)]
        [SerializeField] private string _overrideText;

        [EndIf]


        protected override void Setup()
        {
            _text = GetComponentInChildren<TMP_Text>();
        }

        protected override void Configure()
        {
            _text.font = _textData.Theme.GetTextFont(_style);

            if (_override)
            {
                SetText(_overrideText);
                // SetColor(_textData.EnabledColor);
            }
        }

        public void SetText(string newText) => _text.text = newText;
        public void SetColor(Color newColor) => _text.color = newColor;

        public void SetSelectedTextColor(bool isSelected) => SetColor(isSelected ? _textData.EnabledColor : _textData.DisabledColor);
    }
}