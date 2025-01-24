using Maledictus.AStar;
using Maledictus.StateMachine.EnemyAI;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VInspector;

namespace Maledictus
{
    public class EnemySpawner : MonoBehaviour
    {
        private const string LEFT_DOWN = "LeftDown";
        private const string LEFT_UP = "LeftUp";
        private const string RIGHT_DOWN = "RightDown";
        private const string RIGHT_UP = "RightUp";

        [SerializeField, Min(2)] private int _width = 1;
        [SerializeField, Min(2)] private int _height = 1;

        [SerializeField] private EnemyAI _enemy;
        [SerializeField] private bool _spawnOnStart = true;

        public float HalfWidth => _width * 0.5f;
        public float HalfHeight => _height * 0.5f;

        public Dictionary<string, Vector2> Bounds => new()
        {
            { LEFT_DOWN,    new Vector2(transform.position.x - HalfWidth, transform.position.y - HalfHeight) },
            { LEFT_UP,      new Vector2(transform.position.x - HalfWidth, transform.position.y + HalfHeight) },
            { RIGHT_UP,     new Vector2(transform.position.x + HalfWidth, transform.position.y + HalfHeight) },
            { RIGHT_DOWN,   new Vector2(transform.position.x + HalfWidth, transform.position.y - HalfHeight) },
        };

        private void Start()
        {
            if (_spawnOnStart)
                SpawnUnit();
        }


        [Button]
        private void SpawnUnit()
        {
            if (GridManager.Instance == null) return;

            var attempts = 0;
            var validPosition = false;
            var targetPos = Vector3.zero;
            while (!validPosition && attempts < 100)
            {
                var posX = Random.Range(-HalfWidth, HalfWidth);
                var posY = Random.Range(-HalfHeight, HalfHeight);

                var (chunkCoord, node) = GridManager.Instance.GetNodeFromWorldPoint(transform.position + new Vector3(posX, posY));

                targetPos = node.WorldPosition;

                if (IsInsideBounds(targetPos) && node.IsWalkable)
                    validPosition = true;

                attempts++;
            }

            if (validPosition)
            {
                var unit = Instantiate(_enemy, targetPos, Quaternion.identity);
                unit.transform.SetParent(this.transform);
            }
        }

        public bool IsInsideBounds(Vector2 targetPos)
        {
            return targetPos.x > Bounds[LEFT_DOWN].x && targetPos.x < Bounds[RIGHT_DOWN].x
                && targetPos.y > Bounds[LEFT_DOWN].y && targetPos.y < Bounds[RIGHT_UP].y;
        }

        #region Debug

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0f, 0f, 1f, 0.05f);
            Gizmos.DrawCube(transform.position, new Vector3(_width, _height));

            Handles.color = Color.blue;
            Handles.DrawDottedLine(Bounds[LEFT_DOWN], Bounds[LEFT_UP], 7.5f);
            Handles.DrawDottedLine(Bounds[LEFT_UP], Bounds[RIGHT_UP], 7.5f);
            Handles.DrawDottedLine(Bounds[RIGHT_UP], Bounds[RIGHT_DOWN], 7.5f);
            Handles.DrawDottedLine(Bounds[RIGHT_DOWN], Bounds[LEFT_DOWN], 7.5f);
        }

        #endregion
    }
}