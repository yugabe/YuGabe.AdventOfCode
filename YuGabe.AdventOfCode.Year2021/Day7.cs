namespace YuGabe.AdventOfCode.Year2021;

public class Day7 : Day<int[]>
{
    public override int[] ParseInput(string rawInput) => rawInput.ToMany<int>(",");
    private object Execute(Func<int, int, double> sumSelector)
        => Input.MinMax() is var (min, max) ? Enumerable.Range(min, max - min + 1).Min(pos => Input.Sum(c => sumSelector(pos, c))) : 0;

    public override object ExecutePart1() => Execute(static (pos, c) => Math.Abs(c - pos));
    public override object ExecutePart2() => Execute(static (pos, c) => Math.Abs(c - pos) is var d ? ((((double)d - 1) / 2) + 1) * d : 0);
}