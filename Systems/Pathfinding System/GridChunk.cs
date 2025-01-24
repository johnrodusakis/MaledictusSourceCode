using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Maledictus.AStar
{
    public class GridChunk
    {
        public Vector2Int ChunkPosition { get; private set; }

        private Node[,] _grid;
        private ObjectPool<Node> _nodePool;

        private LayerMask _whatIsUnwalkable;
        private readonly int _chunkSize;
        private readonly float _nodeRadius = 0.5f;

        public GridChunk(Vector2Int chunkPosition, int chunkSize, LayerMask whatIsUnwalkable)
        {
            ChunkPosition = chunkPosition;
            _chunkSize = chunkSize;
            _whatIsUnwalkable = whatIsUnwalkable;

            InitializeNodePool();

            CreateGrid();
        }

        private void InitializeNodePool()
        {
            _nodePool = new ObjectPool<Node>(
                createFunc: () => new Node(0, 0, Vector3.zero, true),
                actionOnGet: node => { },
                actionOnRelease: node => node.Reset(),
                actionOnDestroy: node => Debug.Log("Node destroyed"),
                collectionCheck: false,
                defaultCapacity: _chunkSize * _chunkSize,
                maxSize: _chunkSize * _chunkSize * 2
            );
        }

        public void CreateGrid()
        {
            _grid = new Node[_chunkSize, _chunkSize];
            var centerPos = new Vector3(ChunkPosition.x * _chunkSize, ChunkPosition.y * _chunkSize, 0f);
            var worldBottomLeft = centerPos - Vector3.right * (_chunkSize * 0.5f) - Vector3.up * (_chunkSize * 0.5f);

            for (int x = 0; x < _chunkSize; x++)
            {
                for (int y = 0; y < _chunkSize; y++)
                {
                    var worldPoint = worldBottomLeft + Vector3.right * (x + _nodeRadius) + Vector3.up * (y + _nodeRadius);
                    var walkable = !(Physics2D.OverlapPoint(worldPoint, _whatIsUnwalkable));

                    var newNode = _nodePool.Get();
                    newNode.Initialize(x, y, worldPoint, walkable);

                    _grid[x, y] = newNode;
                }
            }
        }

        public void ClearGrid()
        {
            for (int x = 0; x < _chunkSize; x++)
            {
                for (int y = 0; y < _chunkSize; y++)
                {
                    _nodePool.Release(_grid[x, y]);
                    _grid[x, y] = null;
                }
            }
            _grid = null;
        }

        public Node GetNodeFromWorldPoint(Vector3 worldPosition)
        {
            // Adjust world position relative to the chunk
            float adjustedX = worldPosition.x - (ChunkPosition.x * _chunkSize);
            float adjustedY = worldPosition.y - (ChunkPosition.y * _chunkSize);

            var percentX = (adjustedX + (_chunkSize * 0.5f)) / _chunkSize;
            var percentY = (adjustedY + (_chunkSize * 0.5f)) / _chunkSize;

            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            var x = Mathf.RoundToInt((_chunkSize - 1) * percentX);
            var y = Mathf.RoundToInt((_chunkSize - 1) * percentY);

            return _grid[x, y];
        }

        public void DisplayGridChunkVisuals(Color gridChunkColor, Color obstacleChunkColor)
        {
            for (int x = 0; x < _chunkSize; x++)
            {
                for (int y = 0; y < _chunkSize; y++)
                {
                    var color = _grid[x, y].IsWalkable ? gridChunkColor : obstacleChunkColor;
                    DrawSquare(_grid[x, y].WorldPosition, 1f, color, -0.1f);
                }
            }
        }

        public void DisplayChunkVisuals(Color chunkColor)
        {
            var centerPos = new Vector3(ChunkPosition.x * _chunkSize, ChunkPosition.y * _chunkSize, 0f);
            DrawSquare(centerPos, _chunkSize, chunkColor);
        }

        private void DrawSquare(Vector3 centerPos, float size, Color color, float offset = 0f)
        {
            var rightOffset = Vector3.right * (size * (0.5f + offset));
            var upOffset = Vector3.up * (size * (0.5f + offset));

            var worldBottomLeft = centerPos - rightOffset - upOffset;
            var worldBottomRight = centerPos + rightOffset - upOffset;
            var worldTopLeft = centerPos - rightOffset + upOffset;
            var worldTopRight = centerPos + rightOffset + upOffset;

            //var color = Color.white;
            Debug.DrawLine(worldBottomLeft, worldTopLeft, color);
            Debug.DrawLine(worldTopLeft, worldTopRight, color);
            Debug.DrawLine(worldTopRight, worldBottomRight, color);
            Debug.DrawLine(worldBottomRight, worldBottomLeft, color);
        }
    }
}