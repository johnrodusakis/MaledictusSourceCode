using DG.Tweening;
using Maledictus;
using Maledictus.Enemy;
using System;
using UnityEngine;

namespace Maledictus.Codex
{
    public abstract class BasePage : MonoBehaviour
    {
        public RectTransform PageRectTransform { get; private set; }
        public EnemySO EnemySO { get; private set; }

        protected CanvasGroup _canvasGroup;
        
        private void Awake()
        {
            PageRectTransform = GetComponent<RectTransform>(); 
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        public void ResetPage()
        {
            _canvasGroup.DOKill();

            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        public void InitializeEnemy(EnemySO enemySO)
        {
            _canvasGroup.DOFade(0f, 0.2f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                EnemySO = enemySO;

                switch (enemySO.Discovery.Value)
                {
                    case DiscoveryStage.Unknown:
                        InitializeUnknown();
                        break;
                    case DiscoveryStage.Encountered:
                        InitializeEncountered();
                        break;
                    case DiscoveryStage.Captured:
                        InitializeCaptured();
                        break;
                }
                _canvasGroup.DOFade(1f, 0.2f).SetEase(Ease.InOutSine).OnComplete(() =>
                {
                    _canvasGroup.interactable = true;
                    _canvasGroup.blocksRaycasts = true;
                });
            });
        }

        public abstract void InitializeUnknown();
        public abstract void InitializeEncountered();
        public abstract void InitializeCaptured();
    }
}