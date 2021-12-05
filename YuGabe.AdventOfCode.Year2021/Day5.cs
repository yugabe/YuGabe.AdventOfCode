namespace YuGabe.AdventOfCode.Year2021;

public class Day5 : Day<Day5.Line[]>
{
    public record Point([Split(0, ",")] int X, [Split(1, ",")] int Y) : Parsed { }
    public record Line([Split(0, " -> ")] Point Start, [Split(1, " -> ")] Point End) : Parsed { }
    public override Line[] ParseInput(string rawInput) => rawInput.ToMany<Line>();

    public override object ExecutePart1()
    {
        var map = new Dictionary<Point, int>();
        foreach (var line in Input.Where(l => l.Start.X == l.End.X))
            for (var point = new Point(line.Start.X, Math.Min(line.Start.Y, line.End.Y)); point.Y <= Math.Max(line.Start.Y, line.End.Y); point = point with { Y = point.Y + 1 })
                map[point] = map.TryGetValue(point, out var value) ? value + 1 : 1;
        foreach (var line in Input.Where(l => l.Start.Y == l.End.Y))
            for (var point = new Point(Math.Min(line.Start.X, line.End.X), line.Start.Y); point.X <= Math.Max(line.Start.X, line.End.X); point = point with { X = point.X + 1 })
                map[point] = map.TryGetValue(point, out var value) ? value + 1 : 1;
        return map.Count(e => e.Value > 1);
    }

    public override object ExecutePart2()
    {
        var map = new Dictionary<Point, int>();

        var (maxX, maxY) = (Math.Max(Input.Max(k => k.End.X), Input.Max(k => k.Start.X)), Math.Max(Input.Max(k => k.End.Y), Input.Max(k => k.Start.Y)));
        foreach (var (start, end) in Input)
        {
            var current = start;
            int value;
            while (current != end)
            {
                map[current] = map.TryGetValue(current, out value) ? value + 1 : 1;
                current = new Point(current.X < end.X ? current.X + 1 : current.X > end.X ? current.X - 1 : current.X, current.Y < end.Y ? current.Y + 1 : current.Y > end.Y ? current.Y - 1 : current.Y);
            }
            map[current] = map.TryGetValue(current, out value) ? value + 1 : 1;
        }

        return map.Count(e => e.Value > 1);
    }
}
