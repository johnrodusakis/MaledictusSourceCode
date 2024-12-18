using System.Collections.Generic;
using UnityEngine;

namespace Maledictus
{
    public class AIMovement : BaseMovement
    {
        [SerializeField] private float _walkSpeed = 3f;

        public void MoveTo(Vector2 target, int offset = 0)
        {
            SetTarget(target);

            var path = Pathfinding.Instance.FindPath(transform.position, _targetPos);

            if(offset > 0 && path.Count > 1)
            {
                var pathLength = path.Count - 1;

                for (int i = pathLength; i > pathLength - offset; i--)
                    path.RemoveAt(i);

                SetTarget(path[^1]);
            }

            if (!IsValidTile(path[^1]))
                path = ChooseTheClosestValidTile(path);

            if (path?.Count > 1)
                TryMoveTo(path);
        }


        private List<Vector3> ChooseTheClosestValidTile(List<Vector3> path)
        {
            var validPath = path;

            for (int i = path.Count - 1; i > 0; i--)
            {
                if (!IsValidTile(path[i]))
                    validPath.RemoveAt(i);
                else
                {
                    _targetPos = validPath[i];
                    break;
                }
            }
            return validPath;
        }

        protected override void InitializeMovementSpeed() => _movementSpeed = _walkSpeed;
    }
}