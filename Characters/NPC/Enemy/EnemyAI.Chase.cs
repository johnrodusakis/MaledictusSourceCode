using Maledictus.AStar;
using UnityEngine;

namespace Maledictus.StateMachine.EnemyAI
{
    public partial class EnemyAI
    {
        internal class Chase : BaseState
        {
            private readonly EnemyAI _enemy;
            private readonly AIMovement _enemyMovement;

            private Transform _player;

            private float _elapsedTimer = 0f;

            public bool InRangeToChallenge { get; private set; } = false;

            public Chase(EnemyAI enemy)
            {
                _enemy = enemy;
                _enemyMovement = enemy._movement;
            }

            public override void OnEnter()
            {
                _player = _enemy._player;

                _enemy.SetEnemyState(EnemyState.Chase);

                _elapsedTimer = 1f / _enemyMovement.MovementSpeed;
                MoveToPlayerPosition();
            }


            public override void Tick()
            {
                if (_player == null) return;

                if (_elapsedTimer > 0f)
                    _elapsedTimer -= Time.deltaTime;
                else
                {
                    _elapsedTimer = 1f / _enemyMovement.MovementSpeed;
                    MoveToPlayerPosition();

                    if (AStar.Grid.Instance.GetDistance(_enemyMovement.LastPos, _player.transform.position) == 1)
                        InRangeToChallenge = true;
                }
            }

            private void MoveToPlayerPosition()
            {
                InRangeToChallenge = false;
                _enemyMovement.MoveTo(_player.transform.position, 1);
            }
        }
    }
}