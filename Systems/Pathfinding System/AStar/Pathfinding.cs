using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maledictus.AStar
{
    [RequireComponent(typeof(Grid))]
    public class Pathfinding : MonoBehaviour
    {
        private Grid _grid;
        //GridManager _gridManager;

        private void Awake()
        {
            _grid = GetComponent<Grid>();
            //_gridManager = GetComponent<GridManager>();
        }

        public void FindPath(PathRequest request, Action<PathResult> callback)
        {
            var waypoints = new Vector3[0];
            var pathSuccess = false;

            var startNode = _grid.GetNodeFromWorldPoint(request.PathStart);
            var targetNode = _grid.GetNodeFromWorldPoint(request.PathEnd);

            if (startNode.IsWalkable && targetNode.IsWalkable)
            {
                var openSet = new Heap<Node>(_grid.MaxSize);
                var closedSet = new HashSet<Node>();

                openSet.Add(startNode);

                while (openSet.Count > 0)
                {
                    var currentNode = openSet.RemoveFirst();

                    closedSet.Add(currentNode);

                    if (currentNode == targetNode)
                    {
                        pathSuccess = true;
                        break;
                    }

                    foreach (var neighbour in _grid.GetNeighbours(currentNode))
                    {
                        if (!neighbour.IsWalkable || closedSet.Contains(neighbour))
                            continue;

                        var newMovementCostToNeighbour = currentNode.G + _grid.GetDistance(currentNode, neighbour);

                        if(newMovementCostToNeighbour < neighbour.G || !openSet.Contains(neighbour))
                        {
                            neighbour.G = newMovementCostToNeighbour;
                            neighbour.H = _grid.GetDistance(neighbour, targetNode);
                            neighbour.PreviousNode = currentNode;

                            if(!openSet.Contains(neighbour))
                                openSet.Add(neighbour);
                            else
                                openSet.UpdateItem(neighbour);
                        }
                    }
                }
            }

            if (pathSuccess)
            {
                waypoints = RetracePath(startNode, targetNode);
                pathSuccess = waypoints.Length > 0;
            }

            callback(new PathResult(waypoints, pathSuccess, request.Callback));
        }

        private Vector3[] RetracePath(Node startNode, Node endNode)
        {
            var path = new List<Node>();
            var waypoints = new List<Vector3>();
            var currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.PreviousNode;
            }

            foreach (var node in path)
                waypoints.Add(node.WorldPosition);

            waypoints.Reverse();
            return waypoints.ToArray();
        }
    }
}