using System;
using System.Drawing;

namespace YonatanMankovich.PathStar
{
    public class PathNotFoundException : Exception
    {
        public PathNotFoundException() : base("A path from the start point to the end point could not be found.") { }
    }

    public class PointOutsideOfGridException : Exception
    {
        public PointOutsideOfGridException() : base("The point was outside of the grid.") { }

        public PointOutsideOfGridException(Point point, Size gridSize, string pointIdentifier = "") :
            base($"The point {point} {(pointIdentifier.Length == 0 ? "" : $"('{pointIdentifier}')")} was outside of the grid of size {gridSize}.")
        { }
    }
}