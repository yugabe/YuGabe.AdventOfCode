namespace YuGabe.AdventOfCode.Year2021;

public class Day1 : Day.NewLineSplitParsed<int>
{
    public override object ExecutePart1() => Input.Aggregate((deepens: 0, previousDepth: int.MaxValue), (acc, depth) => (acc.deepens + (depth > acc.previousDepth ? 1 : 0), depth)).deepens;

    public override object ExecutePart2() => Input[..^2].WithIndexes().Select((e, i) => Input[i..(i + 3)].Sum()).Aggregate((deepens: 0, previousDepth: int.MaxValue), (acc, depth) => (acc.deepens + (depth > acc.previousDepth ? 1 : 0), depth)).deepens;
}
