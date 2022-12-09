namespace YuGabe.AdventOfCode.Year2022;
public class Day9 : Day<Day9.Instruction[]>
{
    public record Instruction(Dir Dir, int Value);
    public enum Dir { Unknown = 0, Up = 'U', Down = 'D', Left = 'L', Right = 'R' }
    public override Instruction[] ParseInput(string rawInput) => rawInput.SplitAtNewLines().Select(l => new Instruction((Dir)l[0], int.Parse(l[2..]))).ToArray();

    public override object ExecutePart1()
    {
        var (head, tail) = ((X: 0, Y: 0), (X: 0, Y: 0));
        return Input.SelectMany(instruction => Range(0, instruction.Value).Select(step => tail = Follow(head = StepOne(instruction.Dir, head), tail))).Distinct().Count();
    }

    private static (int X, int Y) StepOne(Dir dir, (int X, int Y) head) =>
        dir switch
        {
            Dir.Up => (head.X, head.Y - 1),
            Dir.Down => (head.X, head.Y + 1),
            Dir.Left => (head.X - 1, head.Y),
            Dir.Right => (head.X + 1, head.Y),
            _ => throw null!
        };

    private static (int X, int Y) Follow((int X, int Y) head, (int X, int Y) tail) =>
        (head.X - tail.X, head.Y - tail.Y) switch
        {
            ( >= -1 and <= 1, >= -1 and <= 1) => tail,
            var (dx, dy) => (tail.X + int.Sign(dx), tail.Y + int.Sign(dy))
        };

    public override object ExecutePart2()
    {
        var snake = new (int X, int Y)[10];

        return Input.SelectMany(instruction => Range(0, instruction.Value).Select(step =>
        {
            snake[0] = StepOne(instruction.Dir, snake[0]);
            for (var i = 1; i <= 9; i++)
                snake[i] = Follow(snake[i - 1], snake[i]);
            return snake[9];
        })).Distinct().Count();
    }
}
