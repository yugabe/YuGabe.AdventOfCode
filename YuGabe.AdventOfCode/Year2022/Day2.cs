namespace YuGabe.AdventOfCode.Year2022;
public class Day2 : Day.NewLineSplitParsed<string>
{
    public override object ExecutePart1() =>
        Input.Sum(step => step[2] - 'W' +
            step switch
            {
                "A Z" or "B X" or "C Y" => 0,
                "A X" or "B Y" or "C Z" => 3,
                "A Y" or "B Z" or "C X" => 6,
                _ => throw null!
            });

    public override object ExecutePart2() =>
        Input.Sum(step => (3 * (step[2] - 'X')) +
            step switch
            {
                "A Y" or "B X" or "C Z" => 1,
                "A Z" or "B Y" or "C X" => 2,
                "A X" or "B Z" or "C Y" => 3,
                _ => throw null!
            });
}
