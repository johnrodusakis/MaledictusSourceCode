using Maledictus.Enemy;
using Maledictus.CustomSoap;
using UnityEngine;
using System.Collections;
using Maledictus.StateMachine.EnemyAI;

namespace Maledictus.Codex
{
    public class Book : MonoBehaviour
    {
        [SerializeField] private BasePage[] _pages = new BasePage[2];

        [SerializeField] private ScriptableEventEnemySO _onEnemySelected;

        private bool _isBookOpen = false;
        private EnemySO _activeEnemySO;

        private Coroutine _displayEnemyCoroutine;

        private void OnEnable()
        {
            _onEnemySelected.OnRaised += DisplayEnemyOnBook;
        }

        private void OnDisable()
        {
            _onEnemySelected.OnRaised -= DisplayEnemyOnBook;
        }

        public void ResetPages()
        {
            foreach (var page in _pages)
                page.ResetPage();

            if (_displayEnemyCoroutine != null)
            {
                StopCoroutine(_displayEnemyCoroutine);
                _displayEnemyCoroutine = null;
            }
        }

        public void DisplayActiveEnemy()
        {
            if(_activeEnemySO != null)
                StartCoroutine(DisplayEnemyCoroutine(_activeEnemySO));
        }

        private void DisplayEnemyOnBook(EnemySO enemySO) => _displayEnemyCoroutine = StartCoroutine(DisplayEnemyCoroutine(enemySO));

        private IEnumerator DisplayEnemyCoroutine(EnemySO enemySO)
        {
            if (!_isBookOpen)
                _isBookOpen = true;

            _activeEnemySO = enemySO;
            yield return null;

            foreach (var page in _pages)
                page.InitializeEnemy(enemySO);
        }
    }
}