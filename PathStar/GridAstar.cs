using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace YonatanMankovich.PathStar
{
    public class GridAstar : IGridAstar
    {
        public Size GridSize { get; }
        public Point StartPoint { get; }
        public Point EndPoint { get; }
        public bool IsDone { get; private set; }
        public List<Point> Path { get; private set; } = new List<Point>();

        internal List<GridPoint> OpenSet { get; set; } = new List<GridPoint>();
        internal List<GridPoint> ClosedSet { get; set; } = new List<GridPoint>();

        private GridPoint[,] Grid { get; set; }

        public GridAstar(Size gridSize, Point startPoint, Point endPoint, List<Point> wallPoints)
        {
            GridSize = gridSize;

            if (!IsPointOnGrid(startPoint))
                throw new PointOutsideOfGridException(startPoint, gridSize, "Start point");

            StartPoint = startPoint;

            if (!IsPointOnGrid(endPoint))
                throw new PointOutsideOfGridException(endPoint, gridSize, "End point");

            EndPoint = endPoint;
            Grid = new GridPoint[GridSize.Height, GridSize.Width];

            for (int y = 0; y < GridSize.Height; y++)
                for (int x = 0; x < GridSize.Width; x++)
                    Grid[y, x] = new GridPoint(x, y);

            for (int y = 0; y < GridSize.Height; y++)
                for (int x = 0; x < GridSize.Width; x++)
                    Grid[y, x].AddNeighbors(Grid);

            foreach (Point wallPoint in wallPoints.ToArray())
            {
                if (!IsPointOnGrid(wallPoint))
                    throw new PointOutsideOfGridException(wallPoint, gridSize, "Wall point");

                Grid[wallPoint.Y, wallPoint.X].IsWall = true;
            }

            Grid[StartPoint.Y, StartPoint.X].IsWall = false;
            Grid[EndPoint.Y, EndPoint.X].IsWall = false;
            OpenSet.Add(Grid[StartPoint.Y, StartPoint.X]);
        }

        private bool IsPointOnGrid(Point point)
            => point.X >= 0 && point.Y >= 0 && point.X < GridSize.Width && point.Y < GridSize.Height;

        public void FindPath()
        {
            while (HasNextStep())
                MakeStep();

            IsDone = true;

            if (OpenSet.Count == 0)
                throw new PathNotFoundException();
        }

        public bool HasNextStep() => OpenSet.Count > 0 && !IsDone;

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
                if (!neighbor.IsWall && !ClosedSet.Contains(neighbor))
                {
                    int tempCost = currentPoint.CostFromStart + Manhattan(neighbor, currentPoint);

                    if (tempCost < neighbor.CostFromStart && OpenSet.Contains(neighbor))
                        OpenSet.Remove(neighbor);

                    if (tempCost < neighbor.CostFromStart && ClosedSet.Contains(neighbor))
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

            Path.Reverse();
        }

        private int Manhattan(GridPoint pointA, GridPoint pointB)
            => Math.Abs(pointA.X - pointB.X) + Math.Abs(pointA.Y - pointB.Y);

        private GridPoint GetCurrentPoint()
        {
            int indexOfMinVal = 0;

            for (int i = 0; i < OpenSet.Count; i++)
                if (OpenSet[i].GetF() < OpenSet[indexOfMinVal].GetF())
                    indexOfMinVal = i;

            if (OpenSet.Count == 0)
                throw new PathNotFoundException();

            return OpenSet[indexOfMinVal];
        }

        public List<GridPoint> GetOpenSet() => new List<GridPoint>(OpenSet);

        public List<GridPoint> GetClosedSet() => new List<GridPoint>(ClosedSet);

        public List<GridPoint> GetWallPoints() => Grid.Cast<GridPoint>().ToList();
    }
}