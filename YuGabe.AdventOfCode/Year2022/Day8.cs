using YuGabe.AdventOfCode.Common;

namespace YuGabe.AdventOfCode.Year2022;
public class Day8 : Day<Map2D<int>>
{
    public override Map2D<int> ParseInput(string rawInput) => new(rawInput.SplitAtNewLines().SelectMany((row, y) => row.Select((c, x) => (value: c - 48, x, y))).ToDictionary(e => (e.x, e.y), e => e.value));

    public override object ExecutePart1() => Input.Values.Count(tree => tree.AllCardinalNeighbors.Any(dir => dir.All(o => o.Value < tree.Value)));

    public override object ExecutePart2() => base.Input.Values.Max(tree => tree.AllCardinalNeighbors.Aggregate(1, (acc, dir) => dir.WithNeighbors().TakeWhile(o => (o.previous?.Value ?? 0) < tree.Value).Count()));
}
