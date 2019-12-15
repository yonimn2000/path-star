using System;
using System.Collections.Generic;
using System.Drawing;
using YonatanMankovich.PathStar;

namespace YonatanMankovich.PathStarTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Grid A* Path Tester";
            Size gridSize = new Size(45, 25);
            Point startPoint = new Point(0, 0);
            Point endPoint = new Point(gridSize.Width - 1, gridSize.Height - 1);
            List<Point> wallPoints = new List<Point>();
            Random random = new Random();

            for (int i = 0; i < gridSize.Width * gridSize.Height * 0.4; i++)
                wallPoints.Add(new Point(random.Next(gridSize.Width), random.Next(gridSize.Height)));
            Console.Title = "Testing Grid A*";
            GridAstarTest.Test(new GridAstar(gridSize, startPoint, endPoint, wallPoints), wallPoints);
            Console.Clear();
            Console.Title = "Testing Grid A* - Bidirectional";
            BiGridAstarTest.Test(new BiGridAstar(gridSize, startPoint, endPoint, wallPoints), wallPoints);
        }
    }
}