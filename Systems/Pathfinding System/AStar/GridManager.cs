using System.Collections.Generic;
using UnityEngine;

namespace Maledictus.AStar
{
    public class GridManager : MonoBehaviour
    {
        private Dictionary<Vector2Int, GridChunk> _loadedChunks = new Dictionary<Vector2Int, GridChunk>();
        public int ActiveChunkRadius = 2; // Number of chunks to keep active around the player.
        private int ChunkSize = 100; // Match the chunk size in GridChunk.

        public Transform Player;

        private void Update()
        {
            Vector2Int playerChunkCoord = GetChunkCoordFromWorldPosition(Player.position);
            LoadChunksAroundPlayer(playerChunkCoord);
        }

        private Vector2Int GetChunkCoordFromWorldPosition(Vector3 position)
        {
            int x = Mathf.FloorToInt(position.x / ChunkSize);
            int y = Mathf.FloorToInt(position.y / ChunkSize);
            return new Vector2Int(x, y);
        }

        private void LoadChunksAroundPlayer(Vector2Int centerChunkCoord)
        {
            HashSet<Vector2Int> requiredChunks = new HashSet<Vector2Int>();

            for (int x = -ActiveChunkRadius; x <= ActiveChunkRadius; x++)
            {
                for (int y = -ActiveChunkRadius; y <= ActiveChunkRadius; y++)
                {
                    Vector2Int chunkCoord = centerChunkCoord + new Vector2Int(x, y);
                    requiredChunks.Add(chunkCoord);

                    if (!_loadedChunks.ContainsKey(chunkCoord))
                    {
                        LoadChunk(chunkCoord);
                    }
                }
            }

            // Unload chunks no longer needed.
            foreach (var chunkCoord in _loadedChunks.Keys)
            {
                if (!requiredChunks.Contains(chunkCoord))
                {
                    UnloadChunk(chunkCoord);
                }
            }
        }

        private void LoadChunk(Vector2Int chunkCoord)
        {
            GridChunk newChunk = new GridChunk(chunkCoord);
            _loadedChunks.Add(chunkCoord, newChunk);
        }

        private void UnloadChunk(Vector2Int chunkCoord)
        {
            _loadedChunks.Remove(chunkCoord);
        }

        public Node GetNodeFromWorldPosition(Vector3 worldPosition)
        {
            Vector2Int chunkCoord = GetChunkCoordFromWorldPosition(worldPosition);
            if (!_loadedChunks.ContainsKey(chunkCoord)) return null;

            Vector2Int localPosition = new Vector2Int(
                Mathf.FloorToInt(worldPosition.x) % ChunkSize,
                Mathf.FloorToInt(worldPosition.y) % ChunkSize
            );

            return _loadedChunks[chunkCoord].GetNode(localPosition);
        }
    }

}