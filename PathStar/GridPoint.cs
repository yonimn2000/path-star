using System.Collections.Generic;
using System.Drawing;

namespace YonatanMankovich.PathStar
{
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