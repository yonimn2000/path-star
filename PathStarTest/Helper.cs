using System;
using System.Drawing;

namespace YonatanMankovich.PathStarTest
{
    static class Helper
    {
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