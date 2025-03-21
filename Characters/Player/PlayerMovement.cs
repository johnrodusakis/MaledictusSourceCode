using Maledictus.AStar;
using Obvious.Soap;
using System.Collections.Generic;
using UnityEngine;

namespace Maledictus.Player.Movement
{
    public class PlayerMovement : BaseGridMovement
    {
        [SerializeField] private float _walkSpeed;
        [SerializeField] private float _runSpeed;

        [SerializeField] private ScriptableVariable<Vector2Int> _playerGridPosition;
        [SerializeField] private ScriptableVariable<Vector3> _playerPosition;

        private Vector2 _moveInput = Vector2.zero;
        private List<Vector2> _moveInputs = new();

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            _playerGridPosition.Value = GridManager.Instance.GetChunkCoordFromWorldPosition(transform.position);

            base.Start();

            _moveInput = Vector2.zero;
            _moveInputs = new List<Vector2>();
        }

        private void OnEnable()
        {
            InputManager.OnMoveStarted += OnMoveKeyDown;
            InputManager.OnMoveEnded += OnMoveKeyUp;

            InputManager.OnSprintStarted += ToggleRunning;
            InputManager.OnSprintEnded += ToggleRunning;
        }

        private void OnDisable()
        {
            InputManager.OnMoveStarted -= OnMoveKeyDown;
            InputManager.OnMoveEnded -= OnMoveKeyUp;

            InputManager.OnSprintStarted -= ToggleRunning;
            InputManager.OnSprintEnded -= ToggleRunning;
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            if (!_canMove) return;

            _moveInput = _moveInputs.Count > 0 ? _moveInputs[^1] : Vector2.zero;

            if (_moveInput != Vector2.zero && _canMove)
            {
                var newPos = _lastPos + _moveInput;
                LookDirection(newPos);

                var gridCoord = GridManager.Instance.GetChunkCoordFromWorldPosition(newPos);

                // Load the new chunk if it's not already loaded
                if (!_playerGridPosition.Equals(gridCoord))
                    _playerGridPosition.Value = gridCoord;

                TryMoveTo(newPos);
            }
        }

        #region Input Events

        private void OnMoveKeyDown(Vector2 input)
        {
            if (!_moveInputs.Contains(input))
                _moveInputs.Add(input);
        }

        private void OnMoveKeyUp(Vector2 input)
        {
            if (_moveInputs.Contains(input))
                _moveInputs.Remove(input);
        }

        protected override void ToggleRunning(bool sprintInput)
        {
            base.ToggleRunning(sprintInput);

            _movementSpeed = sprintInput ? _runSpeed : _walkSpeed;
        }

        #endregion

        protected override void InitializeMovementSpeed() => _movementSpeed = _walkSpeed;
        protected override bool IsMoving() => _moveInput != Vector2.zero || !_canMove;
        public override Vector2Int GetGridPosition() => _playerGridPosition;
    }
}