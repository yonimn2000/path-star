using System;
using System.Drawing;

namespace YonatanMankovich.PathStar
{
    public class PathNotFoundException : Exception
    {
        public PathNotFoundException() : base("Path to destination point was not found.") { }
    }

    public class PointOutsideOfGridException : Exception
    {
        public PointOutsideOfGridException() : base("The point was outside of the grid.") { }

        public PointOutsideOfGridException(Point point, Size gridSize) : base($"The point {point} was outside of the grid of size {gridSize}.") { }
    }
}