namespace YuGabe.AdventOfCode.Year2022;
public class Day4 : Day<(Day4.Interval Left, Day4.Interval Right)[]>
{
    public record Interval(int Start, int End);

    public override (Interval Left, Interval Right)[] ParseInput(string rawInput) =>
        rawInput.SelectLinesFromTuple2(tokens => (left: tokens.token1.Split('-').Select(int.Parse).ToArray(), right: tokens.token2!.Split('-').Select(int.Parse).ToArray()), separator: ",")
        .Select(tokens => (new Interval(tokens.left[0], tokens.left[1]), new Interval(tokens.right[0], tokens.right[1])))
        .ToArray();

    public override object ExecutePart1() =>
        Input.Count(p => (p.Left.Start >= p.Right.Start && p.Left.End <= p.Right.End) || (p.Right.Start >= p.Left.Start && p.Right.End <= p.Left.End));

    public override object ExecutePart2() => 
        Input.Count(p => (p.Left.Start <= p.Right.Start && p.Left.End >= p.Right.Start) || (p.Right.Start <= p.Left.Start && p.Right.End >= p.Left.Start));
}
