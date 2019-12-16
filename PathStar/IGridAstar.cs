using System.Collections.Generic;
using System.Drawing;

namespace YonatanMankovich.PathStar
{
    public interface IGridAstar
    {
        Size GridSize { get; }
        Point StartPoint { get; }
        Point EndPoint { get; }
        List<Point> Path { get; }
        bool IsDone { get; }

        void FindPath();
        bool HasNextStep();
        void MakeStep();
        List<GridPoint> GetOpenSet();
        List<GridPoint> GetClosedSet();
    }
}