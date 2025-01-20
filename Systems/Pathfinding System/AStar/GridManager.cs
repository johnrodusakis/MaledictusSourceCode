using Obvious.Soap;
using System.Collections.Generic;
using UnityEngine;

namespace Maledictus.AStar
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance { get; private set; }

        [SerializeField] private int _activeChunkRadius = 2; // Number of chunks to keep active around the player.
        [SerializeField] private int _chunkSize = 100; // Match the chunk size in GridChunk.

        [SerializeField] private LayerMask _whatIsObstacle;

        [SerializeField] private ScriptableVariable<Vector2Int> _playerGridPosition;

        private List<GridChunkData> _loadedChunks = new();

        public int ChunkSize => _chunkSize;


        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            LoadChunksAroundPlayer(_playerGridPosition.Value);
        }

        private void OnEnable()
        {
            _playerGridPosition.OnValueChanged += LoadChunksAroundPlayer;
        }

        private void OnDisable()
        {
            _playerGridPosition.OnValueChanged -= LoadChunksAroundPlayer;
        }

        public Vector2Int GetChunkCoordFromWorldPosition(Vector3 position)
        {
            int x = Mathf.RoundToInt(position.x / _chunkSize);
            int y = Mathf.RoundToInt(position.y / _chunkSize);
            //Debug.Log($"Pos: {position}\nX: {x}, Y: {y}");
            return new Vector2Int(x, y);
        }

        private void LoadChunksAroundPlayer(Vector2Int centerChunkCoord)
        {
            for (int x = -_activeChunkRadius; x <= _activeChunkRadius; x++)
            {
                for (int y = -_activeChunkRadius; y <= _activeChunkRadius; y++)
                {
                    var chunkCoord = centerChunkCoord + new Vector2Int(x, y);

                    if (!CheckForCoordinates(chunkCoord).Item1)
                        LoadChunk(chunkCoord);
                }
            }
        }

        private (bool, GridChunk) CheckForCoordinates(Vector2Int coordinates)
        {
            foreach (var chunk in _loadedChunks)
            {
                if(chunk.GridPosition.Equals(coordinates))
                    return (true, chunk.GridChunk);
            }

            return (false, null);
        }

        public GridChunk GetGridChunk(Vector2Int gridPos)
        {
            var (result, chunk) = CheckForCoordinates(gridPos);
            return result ? chunk : null;
        }

        private void LoadChunk(Vector2Int chunkCoord)
        {
            var newChunk = new GridChunk(chunkCoord, _chunkSize + 4, _whatIsObstacle);
            _loadedChunks.Add(new GridChunkData(chunkCoord, newChunk));
        }

        public (Vector2Int, Node) GetNodeFromWorldPoint(Vector3 worldPosition)
        {
            var chunkCoord = GetChunkCoordFromWorldPosition(worldPosition);
            var (result, chunk) = CheckForCoordinates(chunkCoord);

            if (result)
                return (chunkCoord, chunk.GetNodeFromWorldPoint(worldPosition));

            return (chunkCoord, null);
        }

        public GridChunk MergedGridChunk(GridChunk chunk1, GridChunk chunk2)
        {
            return new GridChunk(chunk2.ChunkPosition, _chunkSize * 2, _whatIsObstacle);
        }

    }

    [System.Serializable]
    public class GridChunkData
    {
        public Vector2Int GridPosition;
        public GridChunk GridChunk;

        public GridChunkData(Vector2Int pos, GridChunk chunk)
        {
            GridPosition = pos;
            GridChunk = chunk;
        }
    }
}