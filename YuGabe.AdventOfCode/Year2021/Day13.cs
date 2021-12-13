namespace YuGabe.AdventOfCode.Year2021;

public class Day13 : Day<Day13.TransparentPaper>
{
    public override TransparentPaper ParseInput(string rawInput) => rawInput.To<TransparentPaper>();

    public enum Axis { X, Y };
    public record Point([Split(0, ",")] int X, [Split(1, ",")] int Y) { }
    public record Instruction(Axis Axis, int Value) { }
    public record TransparentPaper
    {
        public TransparentPaper(
            [Split(0, "\n\n"), InnerSplit("\n")] Point[] points,
            [Split(1, "\n\n"), InnerSplit("\n")] string[] rawInstructions)
        {
            Points = points.ToHashSet();
            Instructions = rawInstructions.Select(i => i.Split("=")).Select(tokens => new Instruction(Enum.Parse<Axis>(tokens[0][^1].ToString(), true), int.Parse(tokens[1]))).ToArray();
        }

        public HashSet<Point> Points { get; }
        public Instruction[] Instructions { get; }
    }

    public override object ExecutePart1() => Fold(Input.Points, Input.Instructions[0]).Count;

    public override object ExecutePart2()
    {
        Console.Clear();
        foreach (var point in Input.Instructions.Aggregate(Input.Points, Fold))
        {
            Console.SetCursorPosition(point.X, point.Y);
            Console.Write('█');
        }
        return "JRZBLGKH";
    }

    private static HashSet<Point> Fold(HashSet<Point> points, Instruction instruction) =>
        points.Select(point => (instruction.Axis is Axis.X is var x && x ? point.X : point.Y) < instruction.Value ? point : new(x ? Math.Abs(instruction.Value - (point.X - instruction.Value)) : point.X, x ? point.Y : Math.Abs(instruction.Value - (point.Y - instruction.Value)))).ToHashSet();
}
