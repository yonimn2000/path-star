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
        public List<Point> Path { get; private set; } = new List<Point>();
        public List<GridPoint> OpenSet { get; private set; } = new List<GridPoint>();
        public List<GridPoint> ClosedSet { get; private set; } = new List<GridPoint>();

        private GridPoint[,] Grid { get; set; }

        public GridAstar(Size gridSize, Point startPoint, Point endPoint, List<Point> wallPoints)
        {
            GridSize = gridSize;
            StartPoint = startPoint;
            EndPoint = endPoint;
            Grid = new GridPoint[GridSize.Height, GridSize.Width];

            for (int y = 0; y < GridSize.Height; y++)
                for (int x = 0; x < GridSize.Width; x++)
                    Grid[y, x] = new GridPoint(x, y);

            for (int y = 0; y < GridSize.Height; y++)
                for (int x = 0; x < GridSize.Width; x++)
                    Grid[y, x].AddNeighbors(Grid);

            foreach (Point wallPoint in wallPoints)
                Grid[wallPoint.Y, wallPoint.X].IsWall = true;

            Grid[StartPoint.Y, StartPoint.X].IsWall = false;
            Grid[EndPoint.Y, EndPoint.X].IsWall = false;
            OpenSet.Add(Grid[StartPoint.Y, StartPoint.X]);
        }

        public void FindPath()
        {
            while (HasNextStep())
                MakeStep();
            IsDone = true;
            if (OpenSet.Count == 0)
                throw new Exception("No solution");
        }

        public bool HasNextStep()
        {
            return OpenSet.Count > 0 && !IsDone;
        }

        public void MakeStep()
        {
            GridPoint currentPoint = GetCurrentPoint();
            if (currentPoint.X == EndPoint.X && currentPoint.Y == EndPoint.Y)
            {
                IsDone = true;
                BuildPathToStartFromCurrentPoint(currentPoint);
                return;
            }
            OpenSet.Remove(currentPoint);
            ClosedSet.Add(currentPoint);
            foreach (GridPoint neighbor in currentPoint.Neighbors)
            {
                if (!ClosedSet.Contains(neighbor) && !neighbor.IsWall)
                {
                    int tempCost = currentPoint.CostFromStart + Manhattan(neighbor, currentPoint);
                    if (OpenSet.Contains(neighbor) && tempCost < neighbor.CostFromStart)
                        OpenSet.Remove(neighbor);
                    if (ClosedSet.Contains(neighbor) && tempCost < neighbor.CostFromStart)
                        ClosedSet.Remove(neighbor);
                    if (!OpenSet.Contains(neighbor) && !ClosedSet.Contains(neighbor))
                    {
                        neighbor.CostFromStart = tempCost;
                        OpenSet.Add(neighbor);
                        neighbor.Heuristic = Manhattan(neighbor, new GridPoint(EndPoint));
                        neighbor.Previous = currentPoint;
                    }
                }
            }
            BuildPathToStartFromCurrentPoint(currentPoint);
        }

        private void BuildPathToStartFromCurrentPoint(GridPoint currentPoint)
        {
            GridPoint walker = currentPoint;
            Path.Clear();
            Path.Add(walker.GetAsPoint());
            while (walker.Previous != null)
            {
                Path.Add(walker.Previous.GetAsPoint());
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

    public class GridPoint
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int CostFromStart { get; set; } = 0; // AKA G
        public int Heuristic { get; set; } = 0; // AKA H
        public bool IsWall { get; set; } = false;
        public GridPoint Previous { get; set; } = null;
        public List<GridPoint> Neighbors { get; set; } = new List<GridPoint>(0);

        const byte WEIGHT = 2;

        public GridPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public GridPoint(Point point) : this(point.X, point.Y) { }

        public int GetF()
        {
            return CostFromStart + WEIGHT * Heuristic;
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

        public Point GetAsPoint()
        {
            return new Point(X, Y);
        }
    }
}