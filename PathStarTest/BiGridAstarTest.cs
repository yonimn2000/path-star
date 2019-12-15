using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using YonatanMankovich.PathStar;

namespace YonatanMankovich.PathStarTest
{
    static class BiGridAstarTest
    {
        public static void Test(BiGridAstar biGridAstar, List<Point> wallPoints)
        {
            while (biGridAstar.HasNextStep())
            {
                try
                {
                    biGridAstar.MakeStep();
                }
                catch (Exception e)
                {
                    Console.SetCursorPosition(0, biGridAstar.GetGridSize().Height);
                    Console.Write(e.Message);
                    break;
                }
                Console.CursorVisible = false;

                foreach (Point wallPoint in wallPoints)
                    Helper.DrawPointInColor(wallPoint, ConsoleColor.Gray);

                foreach (GridPoint closedSetPoint in biGridAstar.GetClosedSet())
                    Helper.DrawPointInColor(closedSetPoint.GetAsPoint(), ConsoleColor.Cyan);

                foreach (GridPoint openSetPoint in biGridAstar.GetOpenSet())
                    Helper.DrawPointInColor(openSetPoint.GetAsPoint(), ConsoleColor.Green);

                foreach (Point pathPoint in biGridAstar.GetPath())
                    Helper.DrawPointInColor(pathPoint, ConsoleColor.Yellow);

                Thread.Sleep(100);
            }
            Console.ReadLine();
        }
    }
}