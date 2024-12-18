namespace Maledictus.StateMachine.EnemyAI
{
    public partial class EnemyAI
    {
        internal class Challenge : BaseState
        {
            private readonly EnemyAI _enemy;

            public Challenge(EnemyAI enemy)
            {
                _enemy = enemy;
            }

            public override void OnEnter()
            {
                _enemy.SetEnemyState(EnemyState.Challenge);
            }
        }
    }
}