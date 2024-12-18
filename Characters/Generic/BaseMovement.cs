using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Maledictus
{
    public abstract class BaseMovement : MonoBehaviour
    {
        protected float _movementSpeed;

        protected Vector2 _lastPos;
        protected Vector2 _moveToPos;
        protected Vector2 _targetPos = new(-0.5f, -0.5f);
        protected Vector2 _unitDirection = Vector2.down;

        protected Queue<Vector2> _currentPath;

        protected bool _canMove = true;
        protected bool _isRunning = false;

        protected Coroutine _movementCoroutine;

        protected Rigidbody2D _rb;
        protected AnimationController _animationController;

        public float MovementSpeed => _movementSpeed;

        public Vector2 TargetPos => _targetPos;
        public Vector2 LastPos => _lastPos;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animationController = GetComponent<AnimationController>();
        }

        protected virtual void Start()
        {
            _currentPath = new Queue<Vector2>();

            InitializeMovementSpeed();
            InitializePosition();
        }

        private void LateUpdate()
        {
            HandleMovementAnimations();
        }

        private void InitializePosition()
        {
            var worldPos = Pathfinding.Instance.Grid.GetWorldPosition(transform.position);

            _targetPos = worldPos;
            _lastPos = worldPos;
            _moveToPos = worldPos;
        }

        protected bool IsValidTile(Vector2 targetPos)
        {
            var isValidTile = Pathfinding.Instance.IsWalkable(targetPos);

            if (!isValidTile)
                InitializePosition();

            return isValidTile;
        }

        protected void TryMoveTo(List<Vector3> path)
        {
            if (path == null || path.Count == 0) return;

            _unitDirection = LookDirection(path[0]);

            if (!IsValidTile(_targetPos)) return;

            _currentPath.Clear();

            if (_movementCoroutine != null)
            {
                StopCoroutine(_movementCoroutine);
                _movementCoroutine = null;
            }

            foreach (var point in path)
            {
                _currentPath.Enqueue(point);
            }

            _movementCoroutine = StartCoroutine(MoveCoroutine());
        }

        private IEnumerator MoveCoroutine()
        {
            while (_currentPath.Count > 0)
            {
                _moveToPos = _currentPath.Dequeue();

                _unitDirection = LookDirection(_moveToPos);
                _canMove = false;

                while (!ReachedDestination(_moveToPos))
                {
                    transform.position = Vector3.MoveTowards(transform.position, _moveToPos, _movementSpeed * Time.deltaTime);
                    yield return null;
                }

                _lastPos = _moveToPos;
                transform.position = _moveToPos;
                _canMove = true;

                yield return null;
            }
        }

        private void HandleMovementAnimations()
        {;
            var multiplier = _isRunning ? 3f : IsMoving() ? 2f : 1f;
            _animationController.HandleWalkDirectionAnimation(_unitDirection * multiplier);
        }

        private Vector2 LookDirection(Vector2 target)
        {
            var direction = target - (Vector2)transform.position;

            // Calculate the weighted direction
            var weightX = Mathf.Abs(direction.x) * 0.5f;
            var weightY = Mathf.Abs(direction.y);

            if (weightY >= weightX)
                return direction.y > 0 ? Vector2.up : Vector2.down;
            else
                return direction.x > 0 ? Vector2.right : Vector2.left;
        }

        public bool ReachedDestination(Vector2 destination) => (destination - (Vector2)transform.position).sqrMagnitude < Mathf.Epsilon;
        protected void SetTarget(Vector2 target) => _targetPos = Pathfinding.Instance.Grid.GetWorldPosition(target);

        protected abstract void InitializeMovementSpeed();
        protected virtual void ToggleRunning(bool sprintInput) => _isRunning = sprintInput;
        protected virtual bool IsMoving() => !ReachedDestination(_targetPos);

        private void OnDrawGizmos()
        {
            Handles.DrawDottedLine(transform.position, _moveToPos, 2f);

            if (_currentPath?.Count > 0)
            {
                var path = _currentPath.ToArray();
                Handles.DrawDottedLine(_moveToPos, path[0], 2f);

                for (int i = _currentPath.Count - 1; i > 0; i--)
                    Handles.DrawDottedLine(path[i], path[i - 1], 2f);
            }

            Handles.DrawWireDisc(_targetPos, Vector3.forward, 0.25f);
        }
    }
}