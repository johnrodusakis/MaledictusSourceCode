using System.Collections.Generic;
using UnityEngine;
using VInspector;

namespace Maledictus.AStar
{
    public class Grid : MonoBehaviour
    {
        public static Grid Instance { get; private set; }

        [SerializeField] private bool _displayGridGizmos;

        [SerializeField] private LayerMask _whatIsUnwalkable;
        [SerializeField] private Vector2 _gridWorldSize;
        [SerializeField] private float _nodeRadius = 0.495f;

        private Node[,] _grid;

        private float _nodeDiameter;
        private int _gridSizeX;
        private int _gridSizeY;

        public int MaxSize => _gridSizeX * _gridSizeY;

        private void Awake()
        {
            Instance = this;

        }

        private void Start()
        {
            _nodeDiameter = _nodeRadius * 2;
            _gridSizeX = Mathf.RoundToInt(_gridWorldSize.x / _nodeDiameter);
            _gridSizeY = Mathf.RoundToInt(_gridWorldSize.y / _nodeDiameter);
            CreateGrid();
        }

        [Button]
        private void CreateGrid()
        {
            _grid = new Node[_gridSizeX, _gridSizeY];
            var worldBottomLeft = transform.position - Vector3.right * _gridWorldSize.x/2 - Vector3.up * _gridWorldSize.y/2;

            for (int x = 0; x < _gridSizeX; x++)
            {
                for (int y = 0; y < _gridSizeY; y++)
                {
                    var worldPoint = worldBottomLeft + Vector3.right * (x * _nodeDiameter + _nodeRadius) + Vector3.up * (y * _nodeDiameter + _nodeRadius);
                    var walkable = !(Physics2D.OverlapPoint(worldPoint, _whatIsUnwalkable));
                    _grid[x,y] = new Node(x, y, worldPoint, walkable);
                }
            }
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

                if (checkX >= 0 && checkX < _gridSizeX && checkY >= 0 && checkY < _gridSizeY)
                    neighbours.Add(_grid[checkX, checkY]);
            }

            return neighbours;
        }

        public Node GetNodeFromWorldPoint(Vector3 worldPosition)
        {
            var percentX = (worldPosition.x + (_gridWorldSize.x * 0.5f)) / _gridWorldSize.x;
            var percentY = (worldPosition.y + (_gridWorldSize.y * 0.5f)) / _gridWorldSize.y;

            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            var x = Mathf.RoundToInt((_gridSizeX - 1) * percentX);
            var y = Mathf.RoundToInt((_gridSizeY - 1) * percentY);

            return _grid[x, y];
        }

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

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(_gridWorldSize.x, _gridWorldSize.y));

            if (_displayGridGizmos && _grid != null)
            {
                foreach (var node in _grid)
                {
                    Gizmos.color = node.IsWalkable ? Color.white : Color.red;
                    Gizmos.DrawWireCube(node.WorldPosition, Vector3.one * (_nodeDiameter - 0.1f));
                }
            }
        }
    }
}