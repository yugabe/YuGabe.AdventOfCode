namespace YuGabe.AdventOfCode.Year2022;
public class Day10 : Day<(string Instruction, int? Value)[]>
{
    public override (string Instruction, int? Value)[] ParseInput(string rawInput) => rawInput.SplitAtNewLines().Select(l => l.Split(" ")).Select(l => (l[0], int.TryParse(l.ElementAtOrDefault(1), out var value) ? (int?)value : null)).ToArray();

    private IReadOnlyDictionary<string, int> InstructionCosts { get; } = new Dictionary<string, int> { ["addx"] = 2, ["noop"] = 1 };

    public override object ExecutePart1()
    {
        var sum = 0;
        for (var (cycle, pc, x) = (0, 0, 1); pc < Input.Length; pc++)
        {
            var (instruction, value) = Input[pc];
            var cost = InstructionCosts[instruction];
            for (var tick = 0; tick < cost; tick++)
                if (++cycle % 40 == 20)
                    sum += cycle * x;
            if (instruction == "addx")
                x += value!.Value;
        }
        return sum;
    }

    public override object ExecutePart2()
    {
        for (var (cycle, pc, x) = (1, 0, 1); pc < Input.Length; pc++)
        {
            var end = cycle + InstructionCosts[Input[pc].Instruction];
            for (; cycle < end; cycle++)
            {
                if (cycle % 40 == 1)
                    Console.WriteLine();
                Console.Write((cycle % 40) - x is >= 0 and <= 2 ? '#' : '.');
            }
            if (Input[pc] is ("addx", int value))
                x += value;
        }
        throw null!;
    }
}
