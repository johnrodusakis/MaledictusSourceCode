using System.Collections.Generic;
using UnityEngine;
using VInspector;

namespace Maledictus.AStar
{
    public class GridChunk
    {
        public Vector2Int ChunkPosition { get; private set; } // The chunk's position in the grid system.

        private Node[,] _grid;

        private LayerMask _whatIsUnwalkable;
        private readonly int _chunkSize;
        private readonly float _nodeRadius = 0.5f;

        public int MaxSize => _chunkSize * _chunkSize;

        public GridChunk(Vector2Int chunkPosition, int chunkSize, LayerMask whatIsUnwalkable)
        {
            ChunkPosition = chunkPosition;
            _chunkSize = chunkSize;
            _whatIsUnwalkable = whatIsUnwalkable;

            CreateGrid();
        }

        [Button]
        private void CreateGrid()
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
                    _grid[x, y] = new Node(x, y, worldPoint, walkable);
                    DrawSquare(worldPoint, 1f, Color.white, -0.1f);
                }
            }

            DrawSquare(centerPos, _chunkSize, Color.yellow);
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
            Debug.DrawLine(worldBottomLeft, worldTopLeft, color, 500f);
            Debug.DrawLine(worldTopLeft, worldTopRight, color, 500f);
            Debug.DrawLine(worldTopRight, worldBottomRight, color, 500f);
            Debug.DrawLine(worldBottomRight, worldBottomLeft, color, 500f);
        }

        public List<Node> GetNeighbours(Node node)
        {
            var neighbours = new List<Node>();

            int[,] directions = new int[,]
            {
                { -1,  0 },     // Left
                {  1,  0 },     // Right
                {  0, -1 },     // Down
                {  0,  1 }      // Up
            };

            for (int i = 0; i < directions.GetLength(0); i++)
            {
                int checkX = node.X + directions[i, 0];
                int checkY = node.Y + directions[i, 1];

                if (checkX >= 0 && checkX < _chunkSize && checkY >= 0 && checkY < _chunkSize)
                    neighbours.Add(_grid[checkX, checkY]);
            }

            return neighbours;
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

        public Node GetNode(int x, int y) => _grid[x, y];

        public int GetDistance(Vector3 pointA, Vector3 pointB)
        {
            var nodeA = GetNodeFromWorldPoint(pointA);
            var nodeB = GetNodeFromWorldPoint(pointB);

            return GetDistance(nodeA, nodeB);
        }

        public int GetDistance(Node nodeA, Node nodeB)
        {
            var distanceX = Mathf.Abs(nodeA.X - nodeB.X);
            var distanceY = Mathf.Abs(nodeA.Y - nodeB.Y);
            return 10 * (distanceX + distanceY);
        }
    }
}