namespace YuGabe.AdventOfCode.Year2021;

public class Day10 : Day<string[]>
{
    public override string[] ParseInput(string rawInput) => rawInput.SplitAtNewLines();

    private static Dictionary<char, (char pair, int part1Points, int part2Points)> CharactersMap { get; } = new()
    {
        ['('] = (')', 3, 1),
        ['['] = (']', 57, 2),
        ['{'] = ('}', 1197, 3),
        ['<'] = ('>', 25137, 4),
        [')'] = ('(', 3, 1),
        [']'] = ('[', 57, 2),
        ['}'] = ('{', 1197, 3),
        ['>'] = ('<', 25137, 4),
    };
    public static (long? corruptedPoints, long? incompletePoints) GetLinePoints(string line)
    {
        var stack = new Stack<char>();
        foreach (var (c, i) in line.WithIndexes())
        {
            var (pair, pointsPart1, pointsPart2) = CharactersMap[c];
            if (c is '(' or '[' or '{' or '<')
                stack.Push(c);
            else
            {
                if (!stack.TryPop(out var opening))
                    return (null, null);
                if (pair != opening)
                    return (pointsPart1, null);
            }
        }
        return (null, stack.Aggregate(0L, (acc, c) => (5 * acc) + CharactersMap[c].part2Points));
    }

    public override object ExecutePart1() => Input.Select(l => GetLinePoints(l).corruptedPoints).Sum()!;

    public override object ExecutePart2() => Input.Select(l => GetLinePoints(l).incompletePoints).Where(s => s != null).OrderBy(s => s).ToList() is var points ? points[(int)Math.Floor((double)points.Count / 2)]! : null!;
}
