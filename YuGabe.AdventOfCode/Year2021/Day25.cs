namespace YuGabe.AdventOfCode.Year2021;

using Point = Point2D<int>;
using static Day25.Direction;
using static YuGabe.AdventOfCode.ConsoleUtilities.AdvancedConsole;

public class Day25 : Day<Dictionary<Point, Day25.Direction>>
{
    public enum Direction { None = 0, East = 1, South = 2 }
    public override Dictionary<Point, Direction> ParseInput(string rawInput) => rawInput.SplitAtNewLines().SelectMany((e, y) => e.Select((c, x) => (pos: new Point(x, y), val: c switch { '.' => None, '>' => East, 'v' => South, _ => throw null! }))).ToDictionary(e => e.pos, e => e.val);

    public override object ExecutePart1()
    {
        var (maxX, maxY, iterations, moved) = (Input.Max(kv => kv.Key.X), Input.Max(kv => kv.Key.Y), 0, false);
        var (dimX, dimY) = (maxX + 1, maxY + 1);

        Console.Clear();
        void Print()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(string.Join("\n", Range(0, maxY + 1).Select(y => string.Join("", Range(0, maxX + 1).Select(x => Input[new(x, y)] switch
            {
                None => ".",
                East => BackgroundGreen(BrightForegroundGreen(">")),
                South => BackgroundBlue(BrightForegroundBlue("v")),
                _ => throw null!
            })))));
            Console.WriteLine(iterations);
            Thread.Sleep(100);
        }

        Print();

        do
        {
            iterations++;
            var eastMovers = Input
                .Select(c => (cucumber: c, neighbor: new Point((c.Key.X + 1) % dimX, c.Key.Y)))
                .Where(c => c.cucumber.Value == East && Input[c.neighbor] == None)
                .ToList();

            foreach (var ((eastMover, _), neighbor) in eastMovers)
                (Input[eastMover], Input[neighbor]) = (Input[neighbor], Input[eastMover]);

            Print();

            var southMovers = Input
                .Select(c => (cucumber: c, neighbor: new Point(c.Key.X, (c.Key.Y + 1) % dimY)))
                .Where(c => c.cucumber.Value == South && Input[c.neighbor] == None)
                .ToList();

            foreach (var ((southMover, _), neighbor) in southMovers)
                (Input[southMover], Input[neighbor]) = (Input[neighbor], Input[southMover]);

            Print();
            moved = eastMovers.Any() || southMovers.Any();
        }
        while (moved);

        return iterations;
    }
    public override object ExecutePart2() => throw null!;
}
