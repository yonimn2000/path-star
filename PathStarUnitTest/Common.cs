namespace YonatanMankovich.PathStarUnitTest
{
    public static class Common
    {
        public static readonly Size GridSize = new Size(10, 10);
        public static readonly Point StartPoint = new Point(0, 0);
        public static readonly Point EndPoint = new Point(GridSize.Width - 1, GridSize.Height - 1);

        public static List<Point> GetRandomWalls(int seed)
        {
            Random random = new Random(seed);
            List<Point> wallPoints = new List<Point>();

            for (int i = 0; i < GridSize.Width * GridSize.Height * 0.3; i++)
                wallPoints.Add(new Point(random.Next(GridSize.Width), random.Next(GridSize.Height)));

            return wallPoints;
        }
    }
}