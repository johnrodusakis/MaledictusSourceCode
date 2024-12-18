using UnityEditor;
using UnityEngine;
using VInspector;

namespace Maledictus
{
    public class PathfinderSystem : MonoBehaviour
    {
        [Tab("Grid")]
        [SerializeField] private int _gridWidth = 40;
        [SerializeField] private int _gridHeight = 20;

        [Space(15f)]
        [SerializeField] private LayerMask _whatIsObstacle;

        [Tab("Visuals")]
        [SerializeField] private bool _displayDebugGrid = true;
        [SerializeField] private Color _walkableColor = Color.green;
        [SerializeField] private Color _obstacleColor = Color.red;

        private void Awake()
        {
            InitializePathfinding();
        }

        [Button]
        private void InitializePathfinding()
        {
            _ = new Pathfinding(_gridWidth, _gridHeight, this.transform.position, _whatIsObstacle);
        }

        private void OnDrawGizmos()
        {
            if (!_displayDebugGrid) return;

            Pathfinding.Instance?.DrawGrid(_walkableColor, _obstacleColor);
        }
    }
}