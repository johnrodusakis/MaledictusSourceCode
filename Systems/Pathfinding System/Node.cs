using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Maledictus.AStar
{
    public class Node : IHeapItem<Node>
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public int G { get; set; }
        public int H { get; set; }

        public int F => G + H;

        public Node PreviousNode { get; set; }
        public Vector3 WorldPosition { get; private set; }
        public bool IsWalkable { get; private set; }
        public int HeapIndex { get => _heapIndex; set => _heapIndex = value; }

        private int _heapIndex;

        public Node()
        {
            Reset();
        }

        public Node(int gridX, int gridY, Vector3 worldPosition, bool isWalkable)
        {
            Initialize(gridX, gridY, worldPosition, isWalkable);
        }

        // Method to reinitialize a pooled node
        public void Initialize(int gridX, int gridY, Vector3 worldPosition, bool isWalkable)
        {
            X = gridX;
            Y = gridY;
            WorldPosition = worldPosition;
            IsWalkable = isWalkable;
            G = H = 0;
            PreviousNode = null;
        }

        // Method to reset node state when returned to pool
        public void Reset()
        {
            X = Y = 0;
            WorldPosition = Vector3.zero;
            IsWalkable = true;
            G = H = 0;
            PreviousNode = null;
        }

        public int CompareTo(Node nodeToCompare)
        {
            var compare = F.CompareTo(nodeToCompare.F);

            if(compare == 0)
                compare = H.CompareTo(nodeToCompare.H);

            return -compare;
        }
    }
}