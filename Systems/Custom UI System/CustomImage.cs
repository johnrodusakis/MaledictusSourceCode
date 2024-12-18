using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Maledictus.CustomUI
{
    public class CustomImage : CustomUIComponent
    {
        [SerializeField] private CustomImageSO _imageData;
        [SerializeField] private Image _image;

        [Space(15f)]
        [Header("Image Settings")]
        [SerializeField] private bool _enableImage = true;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private Color _color = Color.white;

        protected override void Setup()
        {
            _image = transform.GetComponentInChildren<Image>();
        }

        protected override void Configure()
        {
            SetImage(_sprite);
            SetColor(_color);
            EnableImage(_enableImage);
        }

        public void SetImage(Sprite sprite) => _image.sprite = sprite;
        public void SetColor(Color color) => _image.color = color;
        public void SetAlpha(float alpha) => _image.DOFade(alpha, 0f);
        public void EnableImage(bool enable) => _image.enabled = enable;


        public void FadeIn(float alpha = 1f)
        {
            EnableImage(true);

            SetAlpha(0f);
            _image.DOFade(alpha, 0.1f).SetEase(Ease.OutCirc);
        }

        public void FadeOut()
        {
            _image.DOFade(0f, 0.1f).SetEase(Ease.InCirc).OnComplete(() =>
            {
                EnableImage(false);
            });
        }
    }
}