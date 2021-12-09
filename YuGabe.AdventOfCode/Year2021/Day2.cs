namespace YuGabe.AdventOfCode.Year2021;

public class Day2 : Day<(Day2.Command command, int value)[]>
{
    public enum Command { Up, Down, Forward }

    public override object ExecutePart1()
    {
        var (position, depth) = Input.Aggregate((position: 0, depth: 0), (acc, e) =>
            e.command switch
            {
                Command.Up => (acc.position, acc.depth -= e.value),
                Command.Down => (acc.position, acc.depth += e.value),
                Command.Forward => (acc.position += e.value, acc.depth),
                _ => throw null!
            });
        return position * depth;
    }

    public override object ExecutePart2()
    {
        var (position, depth, aim) = Input.Aggregate((position: 0, depth: 0, aim: 0), (acc, e) =>
            e.command switch
            {
                Command.Up => (acc.position, acc.depth, acc.aim -= e.value),
                Command.Down => (acc.position, acc.depth, acc.aim += e.value),
                Command.Forward => (acc.position += e.value, acc.depth += acc.aim * e.value, acc.aim),
                _ => throw null!
            });
        return position * depth;
    }

    public override (Command, int)[] ParseInput(string rawInput)
        => rawInput.SelectLinesFromTuple2(e => (Enum.Parse<Command>(e.token1, true), int.Parse(e.token2 ?? throw null!)));
}
