namespace YuGabe.AdventOfCode.Year2021;
using Point = Point2D<int>;
using Image = Dictionary<Point2D<int>, bool>;

public class Day20 : Day<(string Algorithm, Image Image)>
{
    public override (string Algorithm, Image Image) ParseInput(string rawInput) => rawInput.SplitAtNewLines().FeedTo(l => (l[0], Range(0, l.Length - 1).SelectMany(y => Range(0, l.Length - 1).Select(x => (x, y))).ToDictionary(c => new Point(c.x, c.y), c => l[c.y + 1][c.x] == '#')));

    public static int CountLitPixelsAfterIterations(Image image, string algorithm, int iterations) => Range(0, iterations).Aggregate((image, infiniteLit: false), (acc, _) => Execute(acc.image, algorithm, acc.infiniteLit)).image.Values.Count(p => p);
    public static (Image Image, bool InfiniteLit) Execute(Image image, string algorithm, bool infiniteLit)
        => image.Keys.Select(k => k.X).MinMax().FeedTo(mm => (Range(mm.min - 1, mm.max - mm.min + 3).SelectMany(y => Range(mm.min - 1, mm.max - mm.min + 3).Select(x => new Point(x, y))).ToDictionary(p => p, p => algorithm[new Point[] {
                        new(p.X - 1, p.Y - 1), new(p.X, p.Y - 1), new(p.X + 1, p.Y - 1),
                        new(p.X - 1, p.Y    ), new(p.X, p.Y    ), new(p.X + 1, p.Y    ),
                        new(p.X - 1, p.Y + 1), new(p.X, p.Y + 1), new(p.X + 1, p.Y + 1) }.Select(p => image.TryGetValue(p, out var oldValue) ? oldValue : infiniteLit).ToInt()] == '#'), algorithm[infiniteLit ? 511 : 0] == '#'));

    public override object ExecutePart1() => CountLitPixelsAfterIterations(Input.Image, Input.Algorithm, 2);
    public override object ExecutePart2() => CountLitPixelsAfterIterations(Input.Image, Input.Algorithm, 50);
}
