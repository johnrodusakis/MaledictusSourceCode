using UnityEngine;

namespace Maledictus.GameMenu
{
    using Maledictus.StateMachine;

    public partial class GameMenuController
    {
        internal class CharacterTab : BaseState
        {
            private GameObject _characterGO;

            public CharacterTab(GameObject go)
            {
                _characterGO = go;
            }

            public override void OnEnter()
            {
                _characterGO.SetActive(true);
            }

            public override void OnExit()
            {
                _characterGO.SetActive(false);
            }

            public override void Tick()
            {

            }
        }
    }
}