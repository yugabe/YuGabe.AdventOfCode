namespace YuGabe.AdventOfCode.Year2022;
public class Day6 : Day
{
    public override object ExecutePart1() => Execute(4);

    public override object ExecutePart2() => Execute(14);

    public int Execute(int markerLength) => Input.WithIndexes().Skip(markerLength - 1).First(e => Input.Skip(e.Index - (markerLength - 1)).Take(markerLength).ToHashSet().Count == markerLength).Index + 1;
}
