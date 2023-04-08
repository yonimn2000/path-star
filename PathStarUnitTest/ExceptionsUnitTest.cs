namespace YonatanMankovich.PathStarUnitTest
{
    [TestClass]
    public class ExceptionsUnitTest
    {
        [TestMethod]
        [ExpectedException(typeof(PathNotFoundException))]
        public void NoPath()
        {
            IGridAstar gridAstar
                = new GridAstar(Common.GridSize, Common.StartPoint, Common.EndPoint, Common.GetRandomWalls(0));

            gridAstar.FindPath();

            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(PointOutsideOfGridException))]
        public void StartPointOutOfGrid()
        {
            IGridAstar gridAstar =
                new GridAstar(Common.GridSize, new Point(0, Common.GridSize.Height), Common.EndPoint, new List<Point>());

            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(PointOutsideOfGridException))]
        public void EndPointOutOfGrid()
        {
            _ = new GridAstar(Common.GridSize, Common.StartPoint, new Point(0, Common.GridSize.Width), new List<Point>());

            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(PointOutsideOfGridException))]
        public void WallPointOutOfGrid()
        {
            List<Point> wallPoints = new List<Point>
            {
                new Point(0, Common.GridSize.Height)
            };

            _ = new GridAstar(Common.GridSize, Common.StartPoint, new Point(0, Common.GridSize.Height), wallPoints);

            Assert.Fail();
        }
    }
}