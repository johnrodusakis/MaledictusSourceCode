using UnityEngine;

namespace Maledictus.StateMachine.EnemyAI
{
    public partial class EnemyAI
    {
        internal class Idle : BaseState
        {
            private readonly EnemyAI _enemy;

            private readonly float restingTime = 2f;
            private float restingTimer;

            public bool ReadyToMove { get; private set; }

            public Idle(EnemyAI enemy)
            {
                _enemy = enemy;
            }

            public override void OnEnter()
            {
                _enemy.SetEnemyState(EnemyState.Idle);

                ReadyToMove = false;
                restingTimer = restingTime;
            }

            public override void Tick()
            {
                if (restingTimer > 0f)
                    restingTimer -= Time.deltaTime;
                else
                    ReadyToMove = true;
            }
        }
    }
}