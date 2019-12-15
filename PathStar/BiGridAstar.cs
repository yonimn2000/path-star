using System;
using System.Collections.Generic;
using System.Drawing;

namespace YonatanMankovich.PathStar
{
    public class BiGridAstar
    {
        public GridAstar GridAstar1 { get; }
        public GridAstar GridAstar2 { get; }

        public BiGridAstar(Size gridSize, Point startPoint, Point endPoint, List<Point> wallPoints)
        {
            GridAstar1 = new GridAstar(gridSize, startPoint, endPoint, wallPoints);
            GridAstar2 = new GridAstar(gridSize, endPoint, startPoint, wallPoints);
        }

        public Size GetGridSize()
        {
            return GridAstar1.GridSize;
        }

        public Point GetStartPoint()
        {
            return GridAstar1.StartPoint;
        }

        public Point GetEndPoint()
        {
            return GridAstar1.EndPoint;
        }

        public bool IsDone()
        {
            return (GridAstar1.IsDone && GridAstar2.IsDone) || DoPathsIntersect();
        }

        private bool DoPathsIntersect()
        {
            return GridAstar1.StartPoint == GridAstar2.StartPoint
                || (GridAstar1.Path.Count > 0 && GridAstar2.Path.Count > 0
                && (GridAstar1.Path.Contains(GridAstar2.Path[0]) // Path 2 intersects 1
                || GridAstar2.Path.Contains(GridAstar1.Path[0]))); // Path 1 intersects 2
        }

        public List<Point> GetPath()
        {
            List<Point> path = new List<Point>();
            int indexOfHead1on2 = GridAstar2.Path.IndexOf(GridAstar1.Path[0]);
            int indexOfHead2on1 = GridAstar1.Path.IndexOf(GridAstar2.Path[0]);

            if (indexOfHead1on2 >= 0) // Path 1 intersects 2
            {
                List<Point> path1 = new List<Point>(GridAstar1.Path);
                path1.Reverse();
                path.AddRange(path1);
                for (int i = GridAstar2.Path.Count - 1; i > indexOfHead1on2; i--)
                    path.Add(GridAstar2.Path[i]);
            }
            else if (indexOfHead2on1 >= 0) // Path 2 intersects 1
            {
                List<Point> path2 = new List<Point>(GridAstar2.Path);
                path2.Reverse();
                path.AddRange(path2);
                for (int i = GridAstar1.Path.Count - 1; i > indexOfHead2on1; i--)
                    path.Add(GridAstar1.Path[i]);
            }
            else
            {
                path.AddRange(GridAstar1.Path);
                path.AddRange(GridAstar2.Path);
            }
            return path;
        }

        public List<GridPoint> GetOpenSet()
        {
            List<GridPoint> openSet = new List<GridPoint>(GridAstar1.OpenSet);
            openSet.AddRange(GridAstar2.OpenSet);
            return openSet;
        }

        public List<GridPoint> GetClosedSet()
        {
            List<GridPoint> closedSet = new List<GridPoint>(GridAstar1.ClosedSet);
            closedSet.AddRange(GridAstar2.ClosedSet);
            return closedSet;
        }

        public void FindPath()
        {
            while (HasNextStep())
                MakeStep();
            if (GetOpenSet().Count == 0)
                throw new PathNotFoundException();
        }

        public void MakeStep()
        {
            GridAstar1.MakeStep();
            GridAstar2.MakeStep();
        }

        public bool HasNextStep()
        {
            return GetOpenSet().Count > 0 && !IsDone();
        }
    }
}