namespace YuGabe.AdventOfCode.Year2021;
using Point = Point3D<int>;

public class Day22 : Day<(Day22.ToggleState State, Point A, Point B)[]>
{
    public enum ToggleState { Off = 0, On = 1 }
    public override (ToggleState State, Point A, Point B)[] ParseInput(string rawInput) => rawInput.SplitAtNewLines().Select(line => new string(line.Where(c => char.IsDigit(c) || c is '.' or ',' or '-').ToArray()).FeedTo(e => e).Split(new[] { "..", "," }, SSO.None).Select(e => int.Parse(e)).ToArray().FeedTo(tokens => (line.StartsWith("on") ? ToggleState.On : line.StartsWith("off") ? ToggleState.Off : throw new InvalidOperationException(), new Point(tokens[0], tokens[2], tokens[4]), new Point(tokens[1], tokens[3], tokens[5])))).ToArray();

    public static long Execute((ToggleState State, Point A, Point B)[] toggles)
    {
        var (xes, ys, zs) = (toggles.SelectMany(t => new[] { t.A.X, t.B.X, t.A.X + 1, t.B.X + 1 }).Distinct().OrderBy(v => v).ToArray(), toggles.SelectMany(t => new[] { t.A.Y, t.B.Y, t.A.Y + 1, t.B.Y + 1 }).Distinct().OrderBy(v => v).ToArray(), toggles.SelectMany(t => new[] { t.A.Z, t.B.Z, t.A.Z + 1, t.B.Z + 1 }).Distinct().OrderBy(v => v).ToArray());
        (toggles, var axOrderedToggles, var sum) = (toggles = toggles.Reverse().ToArray(), toggles.WithIndexes().OrderBy(t => t.Element.A.X).ToList(), 0L);

        Parallel.For(0, xes.Length, xi =>
        {
            for (var yi = 0; yi < ys.Length; yi++)
                for (var zi = 0; zi < zs.Length; zi++)
                {
                    var (x, y, z) = (xes[xi], ys[yi], zs[zi]);
                    int? lastGoodIndex = null;

                    foreach (var ((State, A, B), index) in axOrderedToggles)
                    {
                        if (A.X > x)
                            break;
                        if (/* A.X > x && */B.X >= x && A.Y <= y && B.Y >= y && A.Z <= z && B.Z >= z)
                            lastGoodIndex = lastGoodIndex == null ? index : Math.Min(lastGoodIndex.Value, index);
                    }
                    if (lastGoodIndex != null && toggles[lastGoodIndex.Value].State == ToggleState.On)
                    {
                        var xd = (long)Math.Abs((xi == xes.Length - 1 ? x + 1 : xes[xi + 1]) - x);
                        var yd = (long)Math.Abs((yi == ys.Length - 1 ? y + 1 : ys[yi + 1]) - y);
                        var zd = (long)Math.Abs((zi == zs.Length - 1 ? z + 1 : zs[zi + 1]) - z);
                        Interlocked.Add(ref sum, xd * yd * zd);
                    }
                }
        });

        return sum;
    }

    public override object ExecutePart1() => Execute(Input.Where(ps => new[] { ps.A.X, ps.B.X, ps.A.Y, ps.B.Y, ps.A.Z, ps.B.Z }.All(v => v is >= -50 and <= 50)).ToArray());

    public override object ExecutePart2() => Execute(Input);
}
