using Obvious.Soap;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using VInspector;

namespace Maledictus.AStar
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance { get; private set; }

        [Tab("Settings")]
        [SerializeField] private int _activeChunkRadius = 2;
        [SerializeField] private int _chunkSize = 100;
        [SerializeField] private float _unloadDelay = 5f;

        [SerializeField] private LayerMask _whatIsObstacle;

        [SerializeField] private ScriptableVariable<Vector2Int> _playerGridPosition; 


        [Tab("Visuals")]
        [SerializeField] private bool _displayChunk = true;
        [SerializeField] private bool _displayGridChunk = true;

        [Space(10f)]
        [SerializeField] private Color _chunkColor = Color.yellow;
        [SerializeField] private Color _gridChunkColor = Color.white;
        [SerializeField] private Color _mainGridChunkColor = Color.green;
        [SerializeField] private Color _obstacleGridChunkColor = Color.red;

        private readonly Dictionary<Vector2Int, GridChunk> _activeChunks = new();
        private readonly Dictionary<Vector2Int, float> _delayedUnloadChunks = new();

        public int MaxChunkSize => _chunkSize * _chunkSize;

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            _playerGridPosition.OnValueChanged += LoadChunksAroundUnit;
        }

        private void OnDisable()
        {
            _playerGridPosition.OnValueChanged -= LoadChunksAroundUnit;
        }

        private void Update()
        {
            if(_delayedUnloadChunks.Count > 0)
                CleanupDelayedUnloads();
        }

        private void CleanupDelayedUnloads()
        {
            var chunksToRemove = new List<Vector2Int>();

            foreach (var delayedChunk in _delayedUnloadChunks)
            {
                if (Time.time >= delayedChunk.Value)
                    chunksToRemove.Add(delayedChunk.Key);
            }

            foreach (var chunkToRemove in chunksToRemove)
            {
                if (_activeChunks.ContainsKey(chunkToRemove))
                {
                    UnloadChunk(chunkToRemove);
                    _delayedUnloadChunks.Remove(chunkToRemove);
                }
            }
        }

        public void LoadChunksAroundUnit(Vector2Int centerChunkCoord)
        {
            var requiredChunks = new HashSet<Vector2Int>();

            for (int x = -_activeChunkRadius; x <= _activeChunkRadius; x++)
            {
                for (int y = -_activeChunkRadius; y <= _activeChunkRadius; y++)
                {
                    var chunkCoord = centerChunkCoord + new Vector2Int(x, y);
                    requiredChunks.Add(chunkCoord);


                    if (!_activeChunks.ContainsKey(chunkCoord))
                        LoadChunk(chunkCoord);
                    else
                    {
                        // Cancel delayed unload if chunk is still required
                        if (_delayedUnloadChunks.ContainsKey(chunkCoord))
                            _delayedUnloadChunks.Remove(chunkCoord);
                    }
                }

                var chunksToRemove = new List<Vector2Int>();
                foreach (var loadedChunk in _activeChunks)
                {
                    if (!requiredChunks.Contains(loadedChunk.Key))
                        chunksToRemove.Add(loadedChunk.Key);
                }

                foreach (var chunkToRemove in chunksToRemove)
                {
                    if(!_delayedUnloadChunks.ContainsKey(chunkToRemove))
                        _delayedUnloadChunks.Add(chunkToRemove, Time.time + _unloadDelay);
                    else if(Time.time >= _delayedUnloadChunks[chunkToRemove])
                    {
                        UnloadChunk(chunkToRemove);
                        _delayedUnloadChunks.Remove(chunkToRemove);
                    }
                }
            }
        }

        private void LoadChunk(Vector2Int chunkCoord)
        {
            var newChunk = new GridChunk(chunkCoord, _chunkSize, _whatIsObstacle);
            _activeChunks.Add(chunkCoord, newChunk);
        }

        private void UnloadChunk(Vector2Int chunkCoord)
        {
            if (_activeChunks.TryGetValue(chunkCoord, out var chunk))
            {
                _activeChunks.Remove(chunkCoord);
                chunk.ClearGrid();
            }
        }

        public Vector2Int GetChunkCoordFromWorldPosition(Vector3 position)
        {
            int x = Mathf.RoundToInt(position.x / _chunkSize);
            int y = Mathf.RoundToInt(position.y / _chunkSize);

            return new Vector2Int(x, y);
        }

        public (Vector2Int, Node) GetNodeFromWorldPoint(Vector3 worldPosition)
        {
            var chunkCoord = GetChunkCoordFromWorldPosition(worldPosition);

            if(!_activeChunks.TryGetValue(chunkCoord, out _))
                LoadChunksAroundUnit(chunkCoord);

            return (chunkCoord, _activeChunks[chunkCoord].GetNodeFromWorldPoint(worldPosition));
        }

        public List<Node> GetNeighbours(Node node)
        {
            var neighbours = new List<Node>();

            int[,] directions = new int[,]
            {
                { -1,  0 },  // Left
                {  1,  0 },  // Right
                {  0, -1 },  // Down
                {  0,  1 }   // Up
            };

            for (int i = 0; i < directions.GetLength(0); i++)
            {
                var neighbourPos = node.WorldPosition + new Vector3(directions[i, 0], directions[i, 1], 0);

                var (_, neighborNode) = GetNodeFromWorldPoint(neighbourPos);
                if (neighborNode != null)
                    neighbours.Add(neighborNode);
            }

            return neighbours;
        }

        public int GetDistance(Node nodeA, Node nodeB)
        {
            int dx = Mathf.Abs(nodeA.X - nodeB.X);
            int dy = Mathf.Abs(nodeA.Y - nodeB.Y);

            return (dx > dy) ? (14 * dy + 10 * (dx - dy)) : (14 * dx + 10 * (dy - dx));
        }

        private void OnDrawGizmos()
        {
            foreach(var chunkCoord in _activeChunks.Keys)
            {
                if (_activeChunks.TryGetValue(chunkCoord, out var chunk))
                {
                    if(_displayChunk)
                        chunk.DisplayChunkVisuals(_chunkColor);

                    if (_displayGridChunk)
                    {
                        var color = (chunkCoord == _playerGridPosition) ? _mainGridChunkColor : _gridChunkColor;
                        chunk.DisplayGridChunkVisuals(color, _obstacleGridChunkColor);
                    }
                }
            }
        }
    }
}