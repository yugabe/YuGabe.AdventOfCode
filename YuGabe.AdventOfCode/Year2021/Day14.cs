namespace YuGabe.AdventOfCode.Year2021;

using InsertionLookup = Dictionary<(char Left, char Right), char>;

public class Day14 : Day<(string PolymerTemplate, InsertionLookup InsertionPoints)>
{
    public override (string PolymerTemplate, InsertionLookup InsertionPoints) ParseInput(string rawInput) => rawInput.SplitAtNewLines("\n").FeedTo(lines => (lines[0], lines[1..].ToDictionary(l => (l[0], l[1]), l => l[^1])));

    public override object ExecutePart1() => Execute(10);
    public override object ExecutePart2() => Execute(40);

    public long Execute(int iterations) 
        => Range(0, iterations)
        .Aggregate(Input.PolymerTemplate[..^1].WithIndexes().ToLookup(e => (Left: e.Element, Right: Input.PolymerTemplate[e.Index + 1])).ToDictionary(e => e.Key, e => e.LongCount()), (acc, _) =>
        {
            var newComponents = new Dictionary<(char Left, char Right), long>();
            foreach (var (key, value) in acc)
            {
                newComponents.AddOrUpdate((key.Left, Input.InsertionPoints[key]), value, old => old + value);
                newComponents.AddOrUpdate((Input.InsertionPoints[key], key.Right), value, old => old + value);
            }
            return newComponents;
        }).Append(new((Input.PolymerTemplate[^1], '_'), 1L)).ToLookup(kv => kv.Key.Left, kv => kv.Value).ToDictionary(e => e.Key, e => e.Sum()).MinMax(e => e.Value).FeedTo(e => e.max - e.min);
}
