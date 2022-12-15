namespace YuGabe.AdventOfCode.Year2022;
public class Day15 : Day<Day15.Sensor[]>
{
    public record Sensor(Point Position, Point Beacon)
    {
        public int Range { get; } = Graphs.GetManhattanDistance(Position, Beacon);
        public bool InRange(Point point) => Graphs.GetManhattanDistance(Position, point) <= Range;
        public (int Min, int Max)? GetMinMaxXInRange(int y) => Math.Abs(Position.Y - y) is var distance && distance <= Range ? (Position.X - (Range - distance), Position.X + (Range - distance)) : ((int, int)?)null;
    }

    public override Sensor[] ParseInput(string rawInput) => rawInput.Split('\n').Select(line => line.Split(new[] { '=', ',', ':' }, SSO.TrimEntries)).Select(s => new Sensor((int.Parse(s[1]), int.Parse(s[3])), (int.Parse(s[5]), int.Parse(s[7])))).ToArray();

    public override object ExecutePart1()
    {
        var ((xMin, xMax), maxRange) = (Input.SelectMany(s => new[] { s.Position.X, s.Beacon.X }).MinMax(), Input.Max(s => s.Range));
        var beacons = Input.Select(s => s.Beacon).ToHashSet();
        return Range(xMin - maxRange, xMax - xMin + (2 * maxRange)).Select(x => new Point(x, 2000000)).Where(p => Input.Any(s => s.InRange(p)) && !beacons.Contains(p)).Count();
    }

    public override object ExecutePart2() =>
        Range(0, 4000000).SelectMany(y => Input.Select(s => s.GetMinMaxXInRange(y)).Where(r => r != null).Select(r => r!.Value).OrderBy(x => x.Min).ToList().FeedTo(ranges => ranges.SelectMany(r => new[] { r.Min - 1, r.Max + 1 }.Where(x => x is > 0 and < 4000000 && ranges.All(r => r.Min > x || r.Max < x))).Select(x => (x, y)))).First().FeedTo(e => ((long)e.x * 4000000) + e.y);
}
