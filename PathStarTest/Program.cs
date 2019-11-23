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
            Size gridSize = new Size(10, 10);
            Point startPoint = new Point(0, 0);
            Point endPoint = new Point(9, 9);
            List<Point> wallPoints = new List<Point>();

            for (int i = 0; i < 5; i++)
            {
                wallPoints.Add(new Point(i + 5, 4));
                wallPoints.Add(new Point(i, 2));
                wallPoints.Add(new Point(i + 3, 6));
            }

            gridAstar = new GridAstar(gridSize, startPoint, endPoint, wallPoints);

            while (gridAstar.HasNextStep())
            {
                gridAstar.MakeStep();
                Console.CursorVisible = false;

                foreach (Point wallPoint in wallPoints)
                    DrawPointInColor(wallPoint, ConsoleColor.Gray);

                foreach (GridPoint closedSetPoint in gridAstar.ClosedSet)
                    DrawPointInColor(closedSetPoint.GetAsPoint(), ConsoleColor.Cyan);

                foreach (GridPoint openSetPoint in gridAstar.OpenSet)
                    DrawPointInColor(openSetPoint.GetAsPoint(), ConsoleColor.Green);

                foreach (Point pathPoint in gridAstar.Path)
                    DrawPointInColor(pathPoint, ConsoleColor.Yellow);

                Thread.Sleep(250);
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