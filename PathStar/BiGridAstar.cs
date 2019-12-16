using System.Collections.Generic;
using System.Drawing;

namespace YonatanMankovich.PathStar
{
    public class BiGridAstar : IGridAstar
    {
        public GridAstar GridAstar1 { get; }
        public GridAstar GridAstar2 { get; }

        public Size GridSize => GridAstar1.GridSize;
        public Point StartPoint => GridAstar1.StartPoint;
        public Point EndPoint => GridAstar1.EndPoint;
        public List<Point> Path => GetPath();
        public bool IsDone => (GridAstar1.IsDone && GridAstar2.IsDone) || DoPathsIntersect();

        public BiGridAstar(Size gridSize, Point startPoint, Point endPoint, List<Point> wallPoints)
        {
            GridAstar1 = new GridAstar(gridSize, startPoint, endPoint, wallPoints);
            GridAstar2 = new GridAstar(gridSize, endPoint, startPoint, wallPoints);
        }

        private bool DoPathsIntersect()
        {
            return GridAstar1.StartPoint == GridAstar2.StartPoint
                || (GridAstar1.Path.Count > 0 && GridAstar2.Path.Count > 0
                && (GridAstar1.Path.Contains(GridAstar2.Path[GridAstar2.Path.Count - 1]) // Path 2 intersects 1
                || GridAstar2.Path.Contains(GridAstar1.Path[GridAstar1.Path.Count - 1]))); // Path 1 intersects 2
        }

        private List<Point> GetPath()
        {
            List<Point> path = new List<Point>();
            int indexOfHead1on2 = GridAstar2.Path.IndexOf(GridAstar1.Path[GridAstar1.Path.Count - 1]);
            int indexOfHead2on1 = GridAstar1.Path.IndexOf(GridAstar2.Path[GridAstar2.Path.Count - 1]);

            if (indexOfHead1on2 >= 0) // Path 1 intersects 2
            {
                path.AddRange(GridAstar1.Path);
                for (int i = 0; i < indexOfHead1on2; i++)
                    path.Add(GridAstar2.Path[i]);
            }
            else if (indexOfHead2on1 >= 0) // Path 2 intersects 1
            {
                path.AddRange(GridAstar2.Path);
                for (int i = 0; i < indexOfHead2on1; i++)
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
            return GetOpenSet().Count > 0 && !IsDone;
        }
    }
}