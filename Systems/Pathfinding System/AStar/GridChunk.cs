using UnityEngine;

namespace Maledictus.AStar
{
    public class GridChunk
    {
        public Vector2Int ChunkPosition { get; private set; } // The chunk's position in the grid system.
        private Node[,] _nodes; // 100x100 array of nodes.
        private int _chunkSize = 100;

        public GridChunk(Vector2Int chunkPosition)
        {
            ChunkPosition = chunkPosition;
            _nodes = new Node[_chunkSize, _chunkSize];
            GenerateNodes();
        }

        private void GenerateNodes()
        {
            Vector3 worldBottomLeft = new Vector3(ChunkPosition.x * _chunkSize, ChunkPosition.y * _chunkSize);
            for (int x = 0; x < _chunkSize; x++)
            {
                for (int y = 0; y < _chunkSize; y++)
                {
                    Vector3 worldPoint = worldBottomLeft + new Vector3(x, y);
                    bool walkable = DetermineWalkability(worldPoint);
                    _nodes[x, y] = new Node(x, y, worldPoint, walkable);
                }
            }
        }

        private bool DetermineWalkability(Vector3 worldPoint)
        {
            // Procedural generation logic or obstacle detection.
            return !(Physics2D.OverlapPoint(worldPoint, LayerMask.GetMask("Unwalkable")));
        }

        public Node GetNode(Vector2Int localPosition)
        {
            return _nodes[localPosition.x, localPosition.y];
        }
    }

}