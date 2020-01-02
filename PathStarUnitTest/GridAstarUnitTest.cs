using System.Collections.Generic;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YonatanMankovich.PathStar;

namespace YonatanMankovich.PathStarUnitTest
{
    [TestClass]
    public class GridAstarUnitTest
    {
        [TestMethod]
        public void PathExsits()
        {
            IGridAstar gridAstar = new GridAstar(Common.GridSize, Common.StartPoint, Common.EndPoint, Common.GetRandomWalls(1));
            gridAstar.FindPath();
            List<Point> expectedPath = new List<Point> { new Point(0, 0), new Point(1, 0), new Point(2, 0), new Point(3, 0), new Point(4, 0), new Point(4, 1), new Point(4, 2), new Point(4, 3), new Point(4, 4), new Point(4, 5), new Point(3, 5), new Point(2, 5), new Point(1, 5), new Point(0, 5), new Point(0, 6), new Point(0, 7), new Point(1, 7), new Point(2, 7), new Point(3, 7), new Point(3, 8), new Point(4, 8), new Point(5, 8), new Point(6, 8), new Point(7, 8), new Point(7, 9), new Point(8, 9), new Point(9, 9) };
            CollectionAssert.AreEqual(expectedPath, gridAstar.Path);
        }

        [TestMethod]
        public void BiPathExsits()
        {
            IGridAstar gridAstar = new BiGridAstar(Common.GridSize, Common.StartPoint, Common.EndPoint, Common.GetRandomWalls(1));
            gridAstar.FindPath();
            List<Point> expectedPath = new List<Point> { new Point(0, 0), new Point(0, 1), new Point(1, 1), new Point(1, 2), new Point(2, 2), new Point(3, 2), new Point(3, 3), new Point(3, 4), new Point(2, 4), new Point(1, 4), new Point(0, 4), new Point(0, 5), new Point(0, 6), new Point(0, 7), new Point(0, 8), new Point(1, 8), new Point(2, 8), new Point(3, 8), new Point(4, 8), new Point(4, 9), new Point(5, 9), new Point(6, 9), new Point(7, 9), new Point(8, 9), new Point(9, 9) };
            CollectionAssert.AreEqual(expectedPath, gridAstar.Path);
        }
    }
}