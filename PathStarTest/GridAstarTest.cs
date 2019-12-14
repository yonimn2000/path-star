using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using YonatanMankovich.PathStar;

namespace YonatanMankovich.PathStarTest
{
    static class GridAstarTest
    {
        public static void Test(GridAstar gridAstar, List<Point> wallPoints)
        {
            while (gridAstar.HasNextStep())
            {
                gridAstar.MakeStep();
                if (gridAstar.OpenSet.Count == 0)
                {
                    Console.SetCursorPosition(0, gridAstar.GridSize.Height);
                    Console.Write("No path...");
                    break;
                }
                Console.CursorVisible = false;

                foreach (Point wallPoint in wallPoints)
                    Helper.DrawPointInColor(wallPoint, ConsoleColor.Gray);

                foreach (GridPoint closedSetPoint in gridAstar.ClosedSet)
                    Helper.DrawPointInColor(closedSetPoint.GetAsPoint(), ConsoleColor.Cyan);

                foreach (GridPoint openSetPoint in gridAstar.OpenSet)
                    Helper.DrawPointInColor(openSetPoint.GetAsPoint(), ConsoleColor.Green);

                foreach (Point pathPoint in gridAstar.Path)
                    Helper.DrawPointInColor(pathPoint, ConsoleColor.Yellow);

                Thread.Sleep(100);
            }
            Console.ReadLine();
        }
    }
}