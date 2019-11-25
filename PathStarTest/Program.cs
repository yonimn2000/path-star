using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using YonatanMankovich.PathStar;

namespace YonatanMankovich.PathStarTest
{
    class Program
    {
        static GridAstar gridAstar;
        static void Main(string[] args)
        {
            Size gridSize = new Size(45, 25);
            Point startPoint = new Point(0, 0);
            Point endPoint = new Point(gridSize.Width - 1, gridSize.Height - 1);
            List<Point> wallPoints = new List<Point>();
            Random random = new Random();

            for (int i = 0; i < gridSize.Width * gridSize.Height * 0.25; i++)
                wallPoints.Add(new Point(random.Next(gridSize.Width), random.Next(gridSize.Height)));

            gridAstar = new GridAstar(gridSize, startPoint, endPoint, wallPoints);

            while (gridAstar.HasNextStep())
            {
                gridAstar.MakeStep();
                if (gridAstar.OpenSet.Count == 0)
                {
                    Console.SetCursorPosition(0, gridSize.Height);
                    Console.Write("No path...");
                    break;
                }
                Console.CursorVisible = false;

                foreach (Point wallPoint in wallPoints)
                    DrawPointInColor(wallPoint, ConsoleColor.Gray);

                foreach (GridPoint closedSetPoint in gridAstar.ClosedSet)
                    DrawPointInColor(closedSetPoint.GetAsPoint(), ConsoleColor.Cyan);

                foreach (GridPoint openSetPoint in gridAstar.OpenSet)
                    DrawPointInColor(openSetPoint.GetAsPoint(), ConsoleColor.Green);

                foreach (Point pathPoint in gridAstar.Path)
                    DrawPointInColor(pathPoint, ConsoleColor.Yellow);

                Thread.Sleep(100);
            }
            Console.ReadLine();
        }

        private static void DrawPointInColor(Point point, ConsoleColor color)
        {
            ConsoleColor prevColor = Console.BackgroundColor;
            Console.SetCursorPosition(point.X * 2, point.Y);
            Console.BackgroundColor = color;
            Console.Write("  ");
            Console.BackgroundColor = prevColor;
        }
    }
}