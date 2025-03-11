using DG.Tweening;
using Maledictus.GameMenu;
using UnityEngine;

namespace Maledictus.StateMachine
{
    public abstract class BaseState : IState
    {
        protected virtual void RegisterEvents() { }
        protected virtual void UnregisterEvents() { }

        public virtual void OnEnter() => RegisterEvents();
        public virtual void OnExit() => UnregisterEvents();
        public virtual void Tick() { }
    }

    public abstract class BaseGameMenuState : BaseState
    {
        protected GameMenuTabUI _gameMenuTabUI;

        protected abstract CanvasGroup CanvasGroup { get; }

        public override void OnEnter()
        {
            base.OnEnter();

            CanvasGroup.DOFade(1f, 0.2f).OnComplete(() =>
            {
                CanvasGroup.alpha = 1f;
                CanvasGroup.interactable = true;
                CanvasGroup.blocksRaycasts = true;
            });
        }

        public override void OnExit()
        {
            base.OnExit();

            CanvasGroup.DOFade(0f, 0.2f);
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
        }

        protected void HandleNotificationPopUp(bool showAlert) => _gameMenuTabUI.DisplayNotification(showAlert);
    }
}