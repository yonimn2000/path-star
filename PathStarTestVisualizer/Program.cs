using System.Drawing;
using YonatanMankovich.ConsoleDiffWriter.Color;
using YonatanMankovich.PathStar;
using YonatanMankovich.SimpleColorConsole;

Console.Title = "Grid A* Path Tester";
Size gridSize = new Size(45, 25);
Point startPoint = new Point(0, 0);
Point endPoint = new Point(gridSize.Width - 1, gridSize.Height - 1);
List<Point> wallPoints = new List<Point>();
Random random = new Random();

for (int i = 0; i < gridSize.Width * gridSize.Height * 0.4; i++)
    wallPoints.Add(new Point(random.Next(gridSize.Width), random.Next(gridSize.Height)));

Console.Title = "Testing Grid A*";
IGridAstar gridAstar = new GridAstar(gridSize, startPoint, endPoint, wallPoints);
ColorLinesDiff diff = new ColorLinesDiff();

while (gridAstar.HasNextStep())
{
    gridAstar.MakeStep();

    ColorLines colorLines = new ColorLines();

    // Create grid.
    for (int i = 0; i < gridSize.Height; i++)
        colorLines.AddLine(new ColorString(new string(' ', gridSize.Width * 2)));

    // Draw walls.
    foreach (Point wallPoint in wallPoints)
    {
        colorLines[wallPoint.Y][wallPoint.X * 2] = new ColorChar(' ').BackWhite();
        colorLines[wallPoint.Y][wallPoint.X * 2 + 1] = new ColorChar(' ').BackWhite();
    }

    // Draw open set (to explore).
    foreach (Point openSetPoint in gridAstar.GetOpenSet().Select(p => p.GetAsPoint()))
    {
        colorLines[openSetPoint.Y][openSetPoint.X * 2] = new ColorChar(' ').BackDarkGreen();
        colorLines[openSetPoint.Y][openSetPoint.X * 2 + 1] = new ColorChar(' ').BackDarkGreen();
    }

    // Draw closed set (already explored).
    foreach (Point closedSetPoint in gridAstar.GetClosedSet().Select(p => p.GetAsPoint()))
    {
        colorLines[closedSetPoint.Y][closedSetPoint.X * 2] = new ColorChar(' ').BackDarkRed();
        colorLines[closedSetPoint.Y][closedSetPoint.X * 2 + 1] = new ColorChar(' ').BackDarkRed();
    }

    // Draw the path.
    foreach (Point pathPoint in gridAstar.Path)
    {
        colorLines[pathPoint.Y][pathPoint.X * 2] = new ColorChar(' ').BackYellow();
        colorLines[pathPoint.Y][pathPoint.X * 2 + 1] = new ColorChar(' ').BackYellow();
    }

    diff.WriteDiff(colorLines);
}

Console.WriteLine(Environment.NewLine + (gridAstar.IsDone ? "Path found!" : "Path not found..."));