﻿using Maledictus.AStar;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Maledictus.StateMachine.EnemyAI
{
    public partial class EnemyAI
    {
        internal class Wander : BaseState
        {
            private readonly EnemyAI _enemy;
            private readonly AIMovement _enemyMovement;
            private readonly EnemySpawner _spawner;

            private readonly float stuckTime = 1f;
            private float timeStuck;

            public bool IsMoving { get; private set; } = true;

            public Wander(EnemyAI enemy)
            {
                _enemy = enemy;

                _enemyMovement = enemy._movement;
                _spawner = enemy._spawner;
            }

            public override void OnEnter()
            {
                _enemy.SetEnemyState(EnemyState.Wander);
                ChooseRandomPointToMove();
            }

            public override void Tick()
            {
                if (!_enemyMovement.ReachedDestination(_enemyMovement.TargetPos))
                    CheckIfEnemyStuck();
                else
                    IsMoving = false;
            }

            private void CheckIfEnemyStuck()
            {
                if (_enemyMovement.ReachedDestination(_enemyMovement.LastPos))
                {
                    if (timeStuck > 0f)
                        timeStuck -= Time.deltaTime;
                    else
                        ChooseRandomPointToMove();
                }
            }

            private void ChooseRandomPointToMove()
            {
                IsMoving = true;
                timeStuck = stuckTime;

                var attempts = 0;
                var validPosition = false;

                var (_, node) = GridManager.Instance.GetNodeFromWorldPoint(_enemyMovement.LastPos);

                while (!validPosition && attempts < 100)
                {
                    var dirX = Random.Range(-_enemy.ExtendedDetectionRadius, _enemy.ExtendedDetectionRadius);
                    var dirY = Random.Range(-_enemy.ExtendedDetectionRadius, _enemy.ExtendedDetectionRadius);

                    var targetPosition = _enemy.transform.position + new Vector3(dirX, dirY);
                    (_, node) = GridManager.Instance.GetNodeFromWorldPoint(targetPosition);
                    if (_spawner != null)
                    {
                        if (_spawner.IsInsideBounds(targetPosition) && node.IsWalkable)
                            validPosition = true;
                    }
                    else if(node.IsWalkable)
                        validPosition = true;

                    attempts++;
                }

                if (validPosition)
                    _enemyMovement.MoveTo(node.WorldPosition);
                else if (_spawner != null)
                    _enemyMovement.MoveTo(_spawner.transform.position);
                else
                    _enemyMovement.MoveTo(_enemyMovement.LastPos);
            }
        }
    }
}