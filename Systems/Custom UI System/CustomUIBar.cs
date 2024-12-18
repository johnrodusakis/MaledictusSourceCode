using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace Maledictus.CustomUI
{
    public class CustomUIBar : CustomUIComponent
    {
        [SerializeField] private Image _backgroundBarImage;
        [SerializeField] private Image _barImage;
        [SerializeField] private Image _foregroundBarImage;

        [Space(15f)]
        [Header("Override UI")]
        [SerializeField] private bool _override;

        [ShowIf(nameof(_override), true)]
        [Space(10f)]
        [SerializeField] private Color _backgroundBarColor;
        [SerializeField] private Color _barColor;
        [SerializeField] private Color _foregroundBarColor;

        [EndIf]


        protected override void Configure()
        {
            if (_override)
            {
                _backgroundBarImage.color = _backgroundBarColor;
                _barImage.color = _barColor;
                _foregroundBarImage.color = _foregroundBarColor;
            }
        }
    }
}