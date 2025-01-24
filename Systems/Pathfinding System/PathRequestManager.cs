using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Maledictus.AStar
{
    [RequireComponent(typeof(Pathfinding))]
    public class PathRequestManager : MonoBehaviour
    {
        private static PathRequestManager _instance;

        private Queue<PathResult> _results = new();

        private Pathfinding _pathfinding;

        private void Awake()
        {
            _instance = this;
            
            _pathfinding = GetComponent<Pathfinding>();
        }

        private void Update()
        {
            if(_results.Count > 0)
            {
                var itemsInQueue = _results.Count;
                lock (_results)
                {
                    for (int i = 0; i < itemsInQueue; i++)
                    {
                        var result = _results.Dequeue();
                        result.Callback(result.Path, result.Success);
                    }
                }
            }
        }

        public static void RequestPath(PathRequest request)
        {
            ThreadStart threadStart = delegate {
                _instance._pathfinding.FindPath(request, _instance.FinishedProcessingPath); 
            };

            threadStart.Invoke();
        }

        public void FinishedProcessingPath(PathResult result)
        {
            lock (_results)
                _results.Enqueue(result);
        }
    }

    public struct PathResult
    {
        public Vector3[] Path;
        public bool Success;
        public Action<Vector3[], bool> Callback;

        public PathResult(Vector3[] path, bool success, Action<Vector3[], bool> callback)
        {
            Path = path;
            Success = success;
            Callback = callback;
        }
    }

    public struct PathRequest
    {
        public Vector3 PathStart;
        public Vector3 PathEnd;
        public Action<Vector3[], bool> Callback;

        public PathRequest(Vector3 start, Vector3 end, Action<Vector3[], bool> action)
        {
            PathStart = start;
            PathEnd = end;
            Callback = action;
        }
    }
}