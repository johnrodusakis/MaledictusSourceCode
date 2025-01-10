using UnityEngine;

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

        public Node(int gridX, int gridY, Vector3 worldPosition, bool isWalkable)
        {
            X = gridX;
            Y = gridY;
            WorldPosition = worldPosition;
            IsWalkable = isWalkable;
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