using Maledictus.Player.Movement;
using System;
using UnityEditor;
using UnityEngine;

namespace Maledictus.StateMachine.EnemyAI
{
    [RequireComponent(typeof(AIMovement))]
    public partial class EnemyAI : MonoBehaviour
    {
        protected enum EnemyState { Idle, Wander, Chase, Challenge };
        [SerializeField] private EnemyState _currentEnemyState;

        [SerializeField] private float _detectionRadius;
        [SerializeField] private Color _detectionColor = Color.yellow;

        public float ExtendedDetectionRadius => _detectionRadius + 2f;
        public LayerMask WhatIsPlayer => 1 << LayerMask.NameToLayer("Player");

        private Transform _player;

        private AIMovement _movement;
        private EnemySpawner _spawner;
        private StateMachine _stateMachine;

        private void Awake()
        {
            _movement = GetComponent<AIMovement>();
        }

        private void Start()
        {
            _spawner = transform.GetComponentInParent<EnemySpawner>();

            _stateMachine = new StateMachine();

            var idle = new Idle(this);
            var wander = new Wander(this);
            var chase = new Chase(this);
            var challenge = new Challenge(this);

            HandleTransitions(idle, wander, chase, challenge);

            _stateMachine.SetState(idle);
        }

        private void Update()
        {
            CheckIfPlayerInRange();

            _stateMachine.Tick();
        }

        private void CheckIfPlayerInRange()
        {
            var hits = new Collider2D[1];
            var numOfColliders = 0;

            if (_player != null)
            {
                numOfColliders = Physics2D.OverlapCircleNonAlloc(transform.position, ExtendedDetectionRadius, hits, WhatIsPlayer);

                if (numOfColliders > 0) return;
                else _player = null;
            }

            numOfColliders = Physics2D.OverlapCircleNonAlloc(transform.position, _detectionRadius, hits, WhatIsPlayer);

            if (numOfColliders > 0 && _player == null)
            {
                var collider = hits[0];
                if (collider.CompareTag("Player"))
                {
                    _player = collider.transform;
                    Debug.Log("Player detected!");
                }
            }
        }

        private void HandleTransitions(Idle idle, Wander wander, Chase chase, Challenge challenge)
        {
            void AddStateTransition(IState from, IState to, Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);
            void AddAnyStateTransition(IState to, Func<bool> condition) => _stateMachine.AddTransition(to, condition);

            #region Transitions

            AddStateTransition(idle, wander, StartWandering());
            AddStateTransition(wander, idle, StoppedMoving());

            AddStateTransition(chase, idle, StopChasingPlayer());

            AddStateTransition(chase, challenge, ChallengePlayer());

            #endregion

            #region Any Transitions

            AddAnyStateTransition(chase, ChasePlayer());

            #endregion

            #region Conditions

            Func<bool> StartWandering() => () => _currentEnemyState != EnemyState.Wander && idle.ReadyToMove;
            Func<bool> StoppedMoving() => () => _currentEnemyState != EnemyState.Idle && !wander.IsMoving;

            Func<bool> ChasePlayer() => () => _currentEnemyState != EnemyState.Chase && _player != null && !chase.InRangeToChallenge;
            Func<bool> StopChasingPlayer() => () => _currentEnemyState != EnemyState.Idle && _player == null;

            Func<bool> ChallengePlayer() => () => _currentEnemyState != EnemyState.Challenge && chase.InRangeToChallenge;

            #endregion
        }

        protected void SetEnemyState(EnemyState state) => _currentEnemyState = state;


        private void OnDrawGizmos()
        {

            _detectionColor.a = _player ? 0.1f : 0.5f;
            Handles.color = _detectionColor;
            Handles.DrawWireDisc(transform.position, Vector3.forward, _detectionRadius,2f);

            _detectionColor.a = _player ? 0.5f : 0.1f;
            Handles.color = _detectionColor;
            Handles.DrawWireDisc(transform.position, Vector3.forward, ExtendedDetectionRadius, 2f);
        }
    }
}