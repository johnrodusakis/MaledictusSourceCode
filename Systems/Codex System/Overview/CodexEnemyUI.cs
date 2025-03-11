using Maledictus.CustomSoap;
using Maledictus.CustomUI;
using Maledictus.Enemy;
using Maledictus.StateMachine.EnemyAI;
using System;
using UnityEngine;

namespace Maledictus.Codex
{
    public class CodexEnemyUI :MonoBehaviour
    {
        public Action<EnemySO, bool> OnNewNotification { get; set; }


        [Header("Events")]
        [SerializeField] private ScriptableEventEnemySO _onEnemySelected;

        private CustomUIButton _customButton;
        private EnemySO _enemySO;

        private void Awake()
        {
            _customButton = GetComponent<CustomUIButton>();
        }

        public void InitializeEnemyUI(EnemySO enemySO)
        {
            _enemySO = enemySO;

            _customButton.ButtonText.SetText(enemySO.Discovery == DiscoveryStage.Unknown ? "???" : _enemySO.Name);

            _enemySO.Discovery.OnValueChanged += HandleDiscoveryValueChange;

            _onEnemySelected.OnRaised += DisableButton;
            _customButton.OnClick += SelectEnemy;
        }

        private void OnDestroy()
        {
            _enemySO.Discovery.OnValueChanged -= HandleDiscoveryValueChange;

            _onEnemySelected.OnRaised -= DisableButton;
            _customButton.OnClick -= SelectEnemy;
        }

        private void DisableButton(EnemySO enemySO)
        {
            if (_enemySO != enemySO && _customButton.IsSelected)
                _customButton.HandleDeselect();
        }

        private void SelectEnemy()
        {
            _onEnemySelected.Raise(_enemySO);
            DisplayNotification(false);
        }

        private void HandleDiscoveryValueChange(DiscoveryStage discoveryStage)
        {
            if (discoveryStage == DiscoveryStage.Unknown) return;

            _customButton.ButtonText.SetText(_enemySO.Name);
            DisplayNotification(true);
        }

        public void DisplayNotification(bool isNew)
        {
            _customButton.DisplayNotification(isNew);
            OnNewNotification?.Invoke(_enemySO, isNew);
        }
    }
}