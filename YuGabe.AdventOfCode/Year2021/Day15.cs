namespace YuGabe.AdventOfCode.Year2021;

using Point = Point2D<int>;

public class Day15 : Day<Dictionary<Point, int>>
{
    public override Dictionary<Point, int> ParseInput(string rawInput) => rawInput.SplitAtNewLines().SelectMany((y, yi) => y.WithIndexes().Select(x => (x.Element, X: x.Index, Y: yi))).ToDictionary(e => new Point(e.X, e.Y), e => int.Parse(e.Element.ToString()));

    public static int CalculatePathRisk(Dictionary<Point, int> map)
    {
        var (maxX, maxY) = (map.Keys.Max(k => k.X), map.Keys.Max(k => k.Y));
        var rollingRisks = new Dictionary<Point, int>() { [new(0, 0)] = 0 };

        foreach (var (x, y) in map.Keys.OrderBy(k => k.Y).ThenBy(k => k.X))
            foreach (var (neighborCoordinate, risk) in new Point[] { new(x - 1, y), new(x + 1, y), new(x, y - 1), new(x, y + 1) }.Where(map.ContainsKey).Select(n => (Coordinate: n, Risk: map[n] + rollingRisks[new(x, y)])))
                rollingRisks.AddOrUpdate(neighborCoordinate, risk, oldValue => Math.Min(oldValue, risk));

        return rollingRisks[new(maxX, maxY)];
    }

    public override object ExecutePart1() => CalculatePathRisk(Input);

    public override object ExecutePart2()
    {
        var (biggerMap, maxX, maxY) = (new Dictionary<Point, int>(Input), Input.Keys.Max(k => k.X), Input.Keys.Max(k => k.Y));
        for (var dy = 0; dy <= 4; dy++)
            for (var dx = 0; dx <= 4; dx++)
                foreach (var (x, y) in Input.Keys.OrderBy(e => e.Y).ThenBy(e => e.X))
                    biggerMap[new((dx * (maxX + 1)) + x, (dy * (maxY + 1)) + y)] = ((Input[new(x, y)] + dx + dy - 1) % 9) + 1;

        return CalculatePathRisk(biggerMap);
    }

    public void VisualizePart2()
    {
        Console.Clear();
        var i = 0;
        var (biggerMap, maxX, maxY) = (new Dictionary<Point, int>(Input), Input.Keys.Max(k => k.X), Input.Keys.Max(k => k.Y));
        for (var dy = 0; dy <= 4; dy++)
            for (var dx = 0; dx <= 4; dx++)
                foreach (var (x, y) in Input.Keys.OrderBy(_ => Guid.NewGuid()))
                {
                    var coord = new Point((dx * (maxX + 1)) + x, (dy * (maxY + 1)) + y);
                    var newValue = ((Input[new(x, y)] + dx + dy - 1) % 9) + 1;
                    var delta = 255 - (int)((double)(dx + dy) / 8 * 224);
                    var text = ConsoleUtilities.AdvancedConsole.Foreground(newValue.ToString(), System.Drawing.Color.FromArgb(delta, delta, delta));
                    if (x == 0 && y == 0)
                        text = ConsoleUtilities.AdvancedConsole.Background(text, ConsoleUtilities.BackgroundCode.BackgroundMagenta);
                    Console.SetCursorPosition(coord.X, coord.Y);
                    Console.Write(text);
                    if (++i % 10 == 0)
                        Thread.Sleep(1);
                    biggerMap[coord] = newValue;
                }
    }
}
