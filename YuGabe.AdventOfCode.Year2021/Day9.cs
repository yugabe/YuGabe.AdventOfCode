namespace YuGabe.AdventOfCode.Year2021;

using KV = KeyValuePair<Point2D<int>, int>;

public class Day9 : Day<Dictionary<Point2D<int>, int>>
{
    public override Dictionary<Point2D<int>, int> ParseInput(string rawInput) => rawInput.SplitAtNewLines().WithIndexes().SelectMany(line => line.Element.WithIndexes()
            .Select(c => (x: c.Index, y: line.Index, value: int.Parse(c.Element.ToString()))))
            .ToDictionary(e => new Point2D<int>(e.x, e.y), e => e.value);

    public IEnumerable<KV> GetNeighbors(Point2D<int> p) => new Point2D<int>[] { new(p.X - 1, p.Y), new(p.X + 1, p.Y), new(p.X, p.Y - 1), new(p.X, p.Y + 1) }
        .Where(Input.ContainsKey).Select(k => new KV(k, Input[k]));

    public IEnumerable<KV> GetAllLowPoints() => Input.Where(p => GetNeighbors(p.Key).All(n => n.Value > p.Value));

    public override object ExecutePart1() => GetAllLowPoints().Sum(p => p.Value + 1);

    public IEnumerable<KV> GetBasinElements(KV point) => GetNeighbors(point.Key).Where(p => p.Value > point.Value && p.Value != 9).SelectMany(GetBasinElements).Prepend(point).Distinct();

    public override object ExecutePart2() => GetAllLowPoints().Select(k => GetBasinElements(k).Count()).OrderByDescending(s => s).Take(3).Aggregate((p, c) => p * c);
}
