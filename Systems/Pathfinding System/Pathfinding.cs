using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Maledictus
{
    public class Pathfinding
    {
        //public static Pathfinding Instance { get; private set; }

        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;

        public Grid<PathNode> Grid { get; private set; }
        private List<PathNode> openList;
        private List<PathNode> closedList;

        public LayerMask WhatIsObstacle { get; private set; }
        private readonly bool moveDiagonal = false;

        public Pathfinding(int width, int height, Vector3 originPosition, LayerMask whatIsObstacle)
        {
            //Instance = this;
            this.WhatIsObstacle = whatIsObstacle;
            Vector2 adjustedOriginPosition = (Vector2)originPosition - new Vector2(width / 2f, height / 2f);
            Grid = new Grid<PathNode>(width, height, adjustedOriginPosition, .99f, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));

            // Initialize walkability
            InitializeWalkability();
        }

        private void InitializeWalkability()
        {
            for (int x = 0; x < Grid.Width; x++)
            {
                for (int y = 0; y < Grid.Height; y++)
                {
                    // Get the world position of the current grid node
                    var worldPosition = Grid.GetWorldPosition(x, y) + Vector2.one * 0.5f;

                    // Check if there's an obstacle at this position using Physics2D
                    var collider = Physics2D.OverlapPoint(worldPosition, WhatIsObstacle);

                    if (collider != null)  // If a collider is found on the obstacle layer, it's non-walkable
                    {
                        PathNode node = Grid.GetGridObject(x, y);
                        node.IsWalkable = false;
                    }
                }
            }
        }

        public void PaintPath(List<Vector3> path, Color color, float duration = 1f)
        {
            if (path.Count == 0) return;

            var offset = 0.15f;

            Debug.DrawLine(new Vector2(path[0].x - offset, path[0].y - offset), new Vector2(path[0].x + offset, path[0].y + offset), Color.yellow, duration);
            Debug.DrawLine(new Vector2(path[0].x - offset, path[0].y + offset), new Vector2(path[0].x + offset, path[0].y - offset), Color.yellow, duration);

            for (int i = 1; i < path.Count; i++)
                Debug.DrawLine(path[i - 1], path[i], color, duration);

            Debug.DrawLine(new Vector2(path.Last().x - offset, path.Last().y - offset), new Vector2(path.Last().x + offset, path.Last().y + offset), Color.red, duration);
            Debug.DrawLine(new Vector2(path.Last().x - offset, path.Last().y + offset), new Vector2(path.Last().x + offset, path.Last().y - offset), Color.red, duration);
        }

        public bool IsWalkable(Vector2 targetPos)
        {
            var (x, y) = Grid.GetXY(targetPos);
            if (Grid.IsValidGridPosition(x, y))
            {
                PathNode node = Grid.GetGridObject(x, y);
                return node.IsWalkable;
            }

            return false;
        }

        public int Distance(Vector3 startWorldPosition, Vector3 endWorldPosition)
        {
            var path = FindPath(startWorldPosition, endWorldPosition);

            //PaintPath(path, Color.magenta, 1f);

            return path.Count - 1;
        }

        public List<Vector3> FindNearPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
        {
            var vectorPath = FindPath(startWorldPosition, endWorldPosition);
            vectorPath.Remove(vectorPath.Last());

            return vectorPath;
        }

        public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
        {
            var (startX, startY) = Grid.GetXY(startWorldPosition);
            var (endX, endY) = Grid.GetXY(endWorldPosition);

            if (startX == endX && startY == endY)
                return new List<Vector3> { startWorldPosition };

            // Check if the target position is walkable
            if (!IsWalkable(endWorldPosition))
            {
                // Find the closest walkable tile
                var closestWalkableNode = FindClosestWalkableNode(endX, endY);
                if (closestWalkableNode != null)
                {
                    endX = closestWalkableNode.X;
                    endY = closestWalkableNode.Y;
                }
                else
                {
                    // No walkable tile found, return null
                    return null;
                }
            }

            var path = FindPath(startX, startY, endX, endY);

            if (path == null)
                return null;

            var vectorPath = new List<Vector3>();

            foreach (var pathNode in path)
                vectorPath.Add((new Vector3(pathNode.X, pathNode.Y) + (Vector3)Grid.OriginPosition) * Grid.CellSize + 0.5f * Grid.CellSize * Vector3.one);

            return vectorPath;
        }

        private PathNode FindClosestWalkableNode(int endX, int endY)
        {
            var searchRadius = 1;
            while (searchRadius < Mathf.Max(Grid.Width, Grid.Height))
            {
                for (int x = endX - searchRadius; x <= endX + searchRadius; x++)
                {
                    for (int y = endY - searchRadius; y <= endY + searchRadius; y++)
                    {
                        if (Grid.IsValidGridPosition(x, y))
                        {
                            var node = Grid.GetGridObject(x, y);

                            if (node.IsWalkable)
                                return node;
                        }
                    }
                }
                searchRadius++;
            }
            return null;
        }

        private List<PathNode> FindPath(int startX, int startY, int endX, int endY)
        {
            var startNode = Grid.GetGridObject(startX, startY);
            var endNode = Grid.GetGridObject(endX, endY);

            openList = new List<PathNode> { startNode };
            closedList = new List<PathNode>();

            for (int x = 0; x < Grid.Width; x++)
            {
                for (int y = 0; y < Grid.Height; y++)
                {
                    var pathNode = Grid.GetGridObject(x, y);
                    pathNode.G = int.MaxValue;
                    pathNode.CalculateF();
                    pathNode.PreviousNode = null;
                }
            }

            startNode.G = 0;
            startNode.H = CalculateDistance(startNode, endNode);
            startNode.CalculateF();

            while (openList.Count > 0)
            {
                var currentNode = GetLowestFNode(openList);

                if (currentNode == endNode)
                {
                    // Reached Final Node
                    return CalculatePath(endNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach (var neighborNode in GetNeighborList(currentNode))
                {
                    if (closedList.Contains(neighborNode)) continue;

                    if (!neighborNode.IsWalkable)
                    {
                        closedList.Add(neighborNode);
                        continue;
                    }

                    var tentativeG = currentNode.G + CalculateDistance(currentNode, neighborNode);
                    if (tentativeG < neighborNode.G)
                    {
                        neighborNode.PreviousNode = currentNode;
                        neighborNode.G = tentativeG;
                        neighborNode.H = CalculateDistance(neighborNode, endNode);
                        neighborNode.CalculateF();

                        if (!openList.Contains(neighborNode))
                            openList.Add(neighborNode);
                    }
                }
            }

            // Out of nodes on the openList
            return null;
        }

        private List<PathNode> GetNeighborList(PathNode currentNode)
        {
            var neighborList = new List<PathNode>();

            if (currentNode.X - 1 >= 0)
            {
                // Left
                neighborList.Add(GetNode(currentNode.X - 1, currentNode.Y));

                if (moveDiagonal)
                {
                    // Left Down
                    if (currentNode.Y - 1 >= 0) neighborList.Add(GetNode(currentNode.X - 1, currentNode.Y - 1));

                    // Left Up
                    if (currentNode.Y + 1 < Grid.Height) neighborList.Add(GetNode(currentNode.X - 1, currentNode.Y + 1));
                }
            }

            if (currentNode.X + 1 < Grid.Width)
            {
                // Right
                neighborList.Add(GetNode(currentNode.X + 1, currentNode.Y));

                if (moveDiagonal)
                {
                    // Right Down
                    if (currentNode.Y - 1 >= 0) neighborList.Add(GetNode(currentNode.X + 1, currentNode.Y - 1));

                    // Right Up
                    if (currentNode.Y + 1 < Grid.Height) neighborList.Add(GetNode(currentNode.X + 1, currentNode.Y + 1));
                }
            }

            // Down
            if (currentNode.Y - 1 >= 0) neighborList.Add(GetNode(currentNode.X, currentNode.Y - 1));

            // Up
            if (currentNode.Y + 1 < Grid.Height) neighborList.Add(GetNode(currentNode.X, currentNode.Y + 1));

            return neighborList;
        }

        private PathNode GetNode(int x, int y) => Grid.GetGridObject(x, y);

        private List<PathNode> CalculatePath(PathNode endNode)
        {
            var path = new List<PathNode> { endNode };
            var currentNode = endNode;
            while (currentNode.PreviousNode != null)
            {
                path.Add(currentNode.PreviousNode);
                currentNode = currentNode.PreviousNode;
            }

            path.Reverse();
            return path;
        }

        private int CalculateDistance(PathNode a, PathNode b)
        {
            var xDistance = Mathf.Abs(a.X - b.X);
            var yDistance = Mathf.Abs(a.Y - b.Y);

            var remaining = Mathf.Abs(xDistance - yDistance);

            return moveDiagonal
            ? MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining
            : Random.Range(18, 29) * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;

            //return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        }

        private PathNode GetLowestFNode(List<PathNode> pathNodeList)
        {
            var lowestFNode = pathNodeList[0];
            for (int i = 1; i < pathNodeList.Count; i++)
            {
                if (pathNodeList[i].F < lowestFNode.F)
                    lowestFNode = pathNodeList[i];
            }

            return lowestFNode;
        }

        public void DrawGrid(Color walkableColor, Color obstacleColor)
        {
            if (Grid == null) return;

            for (int x = 0; x < Grid.Width; x++)
            {
                for (int y = 0; y < Grid.Height; y++)
                {
                    var worldPosition = Grid.GetWorldPosition(x, y) + Vector2.one * 0.5f;
                    var node = Grid.GetGridObject(x, y);

                    Gizmos.color = node.IsWalkable ? walkableColor : obstacleColor;
                    Gizmos.DrawWireCube(worldPosition, 0.9f * Grid.CellSize * Vector3.one);
                }
            }
        }
    }

    public class PathNode
    {
        private Grid<PathNode> grid;
        public int X { get; private set; }
        public int Y { get; private set; }

        public int G { get; set; }
        public int H { get; set; }
        public int F { get; private set; }

        public bool IsWalkable { get; set; }
        public PathNode PreviousNode { get; set; }

        public PathNode(Grid<PathNode> grid, int x, int y)
        {
            this.grid = grid;
            this.X = x;
            this.Y = y;

            IsWalkable = true;
        }

        public void CalculateF() => F = G + H;

        public override string ToString() => string.Concat(X, ",", Y);
    }
}