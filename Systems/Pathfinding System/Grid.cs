using System;
using System.Collections.Generic;
using UnityEngine;

namespace Maledictus
{
    public class Grid<T>
    {
        public Action<int, int> OnGridValueChanged;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public float CellSize { get; private set; }
        public Vector2 OriginPosition { get; private set; }

        private readonly T[,] gridArray;

        public Grid(int width, int height, Vector2 originPosition, float cellSize, Func<Grid<T>, int, int, T> initializeGridObject)
        {
            this.Width = width;
            this.Height = height;

            this.OriginPosition = originPosition;
            this.CellSize = cellSize;

            this.gridArray = new T[width, height];

            InitializeGrid(initializeGridObject);

            //DrawDebugGrid();
        }

        private void DrawDebugGrid()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, Height), GetWorldPosition(Width, Height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(Width, 0), GetWorldPosition(Width, Height), Color.white, 100f);
        }

        private void InitializeGrid(Func<Grid<T>, int, int, T> initializeGridObject)
        {
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                    gridArray[x, y] = initializeGridObject(this, x, y);
            }
        }

        public Vector2 GetWorldPosition(int x, int y)
        {
            var worldPosition = new Vector2(x, y) * CellSize + OriginPosition;
            return worldPosition;
        }

        public Vector2 GetWorldPosition(Vector2 worldPosition)
        {
            var (x, y) = GetXY(worldPosition);
            var gridPos = GetWorldPosition(x, y) + Vector2.one * 0.5f;
            return gridPos;
        }

        public (int, int) GetXY(Vector2 worldPosition)
        {
            var x = Mathf.FloorToInt((worldPosition.x - OriginPosition.x) / CellSize);
            var y = Mathf.FloorToInt((worldPosition.y - OriginPosition.y) / CellSize);

            return (x, y);
        }

        public T GetGridObject(int x, int y)
        {
            return IsValidGridPosition(x, y)
                ? gridArray[x, y]
                : default;
        }

        public T GetGridObject(Vector2 worldPosition)
        {
            var (x, y) = GetXY(worldPosition);
            return GetGridObject(x, y);
        }

        public void SetGridObject(int x, int y, T value)
        {
            if (!IsValidGridPosition(x, y)) return;


            if (!EqualityComparer<T>.Default.Equals(gridArray[x, y], value))
            {
                gridArray[x, y] = value;
                OnGridValueChanged?.Invoke(x, y);
            }
        }

        public void SetGridObject(Vector2 worldPosition, T value)
        {
            var (x, y) = GetXY(worldPosition);
            SetGridObject(x, y, value);
        }

        public void ChangeGridValue(int x, int y)
        {
            if (IsValidGridPosition(x, y))
                OnGridValueChanged?.Invoke(x, y);
        }

        public bool IsValidGridPosition(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < Width && y < Height) return true;

            Debug.Log($"Invalid grid position ({x},{y}) -> max bounds [Width: (min: {0}, max: {Width})], [Height: (min: {0}, max: {Height})]");
            return false;
        }
    }
}