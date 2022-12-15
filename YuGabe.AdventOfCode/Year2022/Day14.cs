namespace YuGabe.AdventOfCode.Year2022;
public class Day14 : Day<Day14.Line[]>
{
    public record Line(Point[] Points)
    {
        public override string ToString() => $"{string.Join(" -> ", Points.Select(p => $"{p.X},{p.Y}"))}";
    }

    public override Line[] ParseInput(string rawInput) => rawInput.Split('\n').Select(line => new Line(line.Split(" -> ").Select(p => p.Split(',').FeedTo(t => new Point(int.Parse(t[0]), int.Parse(t[1])))).ToArray())).ToArray();

    public enum Unit { Rock, Air, SandSource, Sand }
    public override object ExecutePart1() => SimulateSandFall(Input);
    public override object ExecutePart2()
    {
        var maxY = Input.SelectMany(l => l.Points).Max(p => p.Y);
        return SimulateSandFall(Input.Append(new(new Point[] { (500 - (maxY + 2), maxY + 2), (500 + (maxY + 2), maxY + 2) })).ToArray());
    }

    private static int SimulateSandFall(Line[] lines)
    {
        var sandOrigin = new Point(500, 0);
        var map = lines.SelectMany(line => line.Points.WithNeighbors().Skip(1).SelectMany(n => n.previous.LineTo(n.current)).Select(p => (Key: p, Value: Unit.Rock))).Distinct().Prepend((Key: sandOrigin, Value: Unit.SandSource)).ToDictionary(e => e.Key, e => e.Value);
        var (yMin, yMax) = map.Keys.MinMax(k => k.Y);

        while (TryDropSandGrain(out var settlePosition))
            map[settlePosition] = Unit.Sand;

        return map.Values.Count(e => e is Unit.Sand);

        bool TryDropSandGrain(out Point settlePosition)
        {
            for (var (x, y) = (sandOrigin.X, yMin); y < yMax; y++)
                if (map.ContainsKey((x, y + 1)))
                    if (!map.ContainsKey((x - 1, y + 1)))
                        x--;
                    else if (!map.ContainsKey((x + 1, y + 1)))
                        x++;
                    else
                        return !map.TryGetValue(settlePosition = (x, y), out var existing) || existing == Unit.SandSource;
            settlePosition = default;
            return false;
        }
    }
}
