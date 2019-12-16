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

            for (int i = 0; i < gridSize.Width * gridSize.Height * 0.3; i++)
                wallPoints.Add(new Point(random.Next(gridSize.Width), random.Next(gridSize.Height)));
            Console.Title = "Testing Grid A*";
            Test(new GridAstar(gridSize, startPoint, endPoint, wallPoints), wallPoints);
            Console.Clear();
            Console.Title = "Testing Grid A* - Bidirectional";
            Test(new BiGridAstar(gridSize, startPoint, endPoint, wallPoints), wallPoints);
        }

        static void Test(IGridAstar gridAstar, List<Point> wallPoints)
        {
            while (gridAstar.HasNextStep())
            {
                try
                {
                    gridAstar.MakeStep();
                }
                catch (Exception e)
                {
                    Console.SetCursorPosition(0, gridAstar.GridSize.Height);
                    Console.Write(e.Message);
                    break;
                }

                Console.CursorVisible = false;

                foreach (Point wallPoint in wallPoints)
                    DrawPointInColor(wallPoint, ConsoleColor.Gray);

                foreach (GridPoint closedSetPoint in gridAstar.GetClosedSet())
                    DrawPointInColor(closedSetPoint.GetAsPoint(), ConsoleColor.Cyan);

                foreach (GridPoint openSetPoint in gridAstar.GetOpenSet())
                    DrawPointInColor(openSetPoint.GetAsPoint(), ConsoleColor.Green);

                foreach (Point pathPoint in gridAstar.Path)
                    DrawPointInColor(pathPoint, ConsoleColor.Yellow);
            }
            Console.ReadLine();
        }

        public static void DrawPointInColor(Point point, ConsoleColor color)
        {
            ConsoleColor prevColor = Console.BackgroundColor;
            Console.SetCursorPosition(point.X * 2, point.Y);
            Console.BackgroundColor = color;
            Console.Write("  ");
            Console.BackgroundColor = prevColor;
        }
    }
}