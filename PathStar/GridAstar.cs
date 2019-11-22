using System;
using System.Collections.Generic;
using System.Drawing;

namespace YonatanMankovich.PathStar
{
    public class GridAstar
    {
        public Size GridSize { get; }
        public Point StartPoint { get; }
        public Point EndPoint { get; }
        public bool IsDone { get; private set; }

        private GridPoint[,] Grid { get; set; }
        private List<GridPoint> Path { get; set; } = new List<GridPoint>();
        private List<GridPoint> OpenSet { get; set; } = new List<GridPoint>();
        private List<GridPoint> ClosedSet { get; set; } = new List<GridPoint>();

        public GridAstar(Size gridSize, Point startPoint, Point endPoint, List<Point> wallPoints)
        {
            GridSize = gridSize;
            StartPoint = startPoint;
            EndPoint = endPoint;

            OpenSet.Add(new GridPoint(StartPoint));
            Grid = new GridPoint[GridSize.Height, GridSize.Width];

            for (int y = 0; y < GridSize.Height; y++)
                for (int x = 0; x < GridSize.Width; x++)
                    Grid[y, x] = new GridPoint(x, y);

            for (int y = 0; y < GridSize.Height; y++)
                for (int x = 0; x < GridSize.Width; x++)
                    Grid[y, x].AddNeighbors(Grid);

            foreach (Point wallPoint in wallPoints)
                Grid[wallPoint.Y, wallPoint.X].IsWall = true;
        }

        public void FindPath()
        {
            while (OpenSet.Count > 0 || !IsDone)
                MakeStep();
            IsDone = true;
            //if (OpenSet.Count == 0) return "No solution"
        }

        public void MakeStep()
        {
            GridPoint currentPoint = GetCurrentPoint();
            if (currentPoint.X == EndPoint.X && currentPoint.Y == EndPoint.Y)
                IsDone = true;
            OpenSet.Remove(currentPoint);
            ClosedSet.Add(currentPoint);
            foreach (GridPoint neighbor in currentPoint.Neighbors)
            {
                if (!ClosedSet.Contains(neighbor) && !neighbor.IsWall)
                {
                    int tempCost = currentPoint.CostFromStart + Manhattan(neighbor, currentPoint);
                    bool newPath = false;
                    if (OpenSet.Contains(neighbor))
                    {
                        if (tempCost < neighbor.CostFromStart)
                        {
                            neighbor.CostFromStart = tempCost;
                            newPath = true;
                        }
                    }
                    else
                    {
                        neighbor.CostFromStart = tempCost;
                        newPath = true;
                        OpenSet.Add(neighbor);
                    }
                    if (newPath)
                    {
                        neighbor.CostFromStart = Manhattan(neighbor, new GridPoint(EndPoint));
                        neighbor.Previous = currentPoint;
                    }
                }
            }

            GridPoint walker = currentPoint;
            Path.Add(walker);
            while (walker.Previous != null)
            {
                Path.Add(walker.Previous);
                walker = walker.Previous;
            }
        }

        private int Manhattan(GridPoint pointA, GridPoint pointB)
        {
            return Math.Abs(pointA.X - pointB.X) + Math.Abs(pointA.Y - pointB.Y);
        }

        private GridPoint GetCurrentPoint()
        {
            int indexOfMinVal = 0;
            for (int i = 0; i < OpenSet.Count; i++)
                if (OpenSet[i].GetF() < OpenSet[indexOfMinVal].GetF())
                    indexOfMinVal = i;
            return OpenSet[indexOfMinVal];
        }
    }

    internal class GridPoint
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int CostFromStart { get; set; } = 0; // AKA G
        public int Heuristic { get; set; } = 0; // AKA H
        public bool IsWall { get; set; } = false;
        public GridPoint Previous { get; set; } = null;
        public List<GridPoint> Neighbors { get; set; } = new List<GridPoint>(0);

        public GridPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public GridPoint(Point point) : this(point.X, point.Y) { }

        public int GetF()
        {
            return CostFromStart + Heuristic;
        }

        public void AddNeighbors(GridPoint[,] grid)
        {
            if (X > 0) // If not the first column
                Neighbors.Add(grid[Y, X - 1]);
            if (Y > 0) // If not the first row
                Neighbors.Add(grid[Y - 1, X]);
            if (X < grid.GetLength(1) - 1) // If not the last column
                Neighbors.Add(grid[Y, X + 1]);
            if (Y < grid.GetLength(0) - 1) // If not the last row
                Neighbors.Add(grid[Y + 1, X]);
        }
    }
}