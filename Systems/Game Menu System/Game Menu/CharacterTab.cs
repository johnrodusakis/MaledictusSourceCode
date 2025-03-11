using UnityEngine;

namespace Maledictus.GameMenu
{
    using Maledictus.StateMachine;

    public partial class GameMenuController
    {
        internal class CharacterTab : BaseGameMenuState
        {
            private readonly CharacterTabUI _characterTabUI;

            protected override CanvasGroup CanvasGroup => _characterTabUI.GetComponent<CanvasGroup>();

            public CharacterTab(CharacterTabUI characterTabUI, GameMenuTabUI menuTabUI)
            {
                _characterTabUI = characterTabUI;
                _gameMenuTabUI = menuTabUI;

                _characterTabUI.OnNewNotification += HandleNotificationPopUp;
            }

            protected override void RegisterEvents()
            {
                _characterTabUI.OnNewNotification += HandleNotificationPopUp;
            }

            protected override void UnregisterEvents()
            {
                _characterTabUI.OnNewNotification -= HandleNotificationPopUp;
            }

            public override void OnEnter()
            {
                base.OnEnter();
            }

            public override void OnExit()
            {
                base.OnExit();
            }

            public override void Tick()
            {

            }
        }
    }
}