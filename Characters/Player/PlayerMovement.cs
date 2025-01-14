using System.Collections.Generic;
using UnityEngine;

namespace Maledictus.Player.Movement
{
    public class PlayerMovement : BaseMovement
    {
        [SerializeField] private float _walkSpeed;
        [SerializeField] private float _runSpeed;

        private Vector2 _moveInput = Vector2.zero;
        private List<Vector2> _moveInputs = new();

        protected override void Start()
        {
            base.Start();

            _moveInput = Vector2.zero;
            _moveInputs = new List<Vector2>();

            //TryMoveTo(_targetPos + new Vector2(5f, 0f));
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
            if(!_canMove) return;

            _moveInput = _moveInputs.Count > 0 ? _moveInputs[^1] : Vector2.zero;

            if (_moveInput != Vector2.zero && _canMove)
            {
                var newPos = _lastPos + _moveInput;
                LookDirection(newPos);
                
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
    }
}