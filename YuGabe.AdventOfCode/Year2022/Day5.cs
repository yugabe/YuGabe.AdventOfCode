namespace YuGabe.AdventOfCode.Year2022;
public class Day5 : Day<Day5.CratesInput>
{
    public record CratesInput(IReadOnlyDictionary<int, Stack<char>> Layout, IReadOnlyList<Instruction> Instructions);
    public override CratesInput ParseInput(string rawInput)
    {
        var parts = rawInput.Split("\n\n");
        var layoutRaw = parts[0].Split('\n').Reverse().ToArray();
        return new CratesInput(
            Layout: Range(0, (int)Math.Ceiling((double)layoutRaw[0].Length / 4)).Select(i => 1 + (i * 4)).ToDictionary(i => layoutRaw[0][i] - '0', i => new Stack<char>(layoutRaw.Skip(1).Select(l => l[i]).Where(char.IsLetter))),
            Instructions: parts[1].Split('\n').Select(l => l.Split(' ')).Select(l => new Instruction(int.Parse(l[1]), int.Parse(l[3]), int.Parse(l[5]))).ToList());
    }

    public record Instruction(int Amount, int From, int To);

    public override object ExecutePart1()
    {
        foreach (var (from, to) in Input.Instructions.SelectMany(i => Range(0, i.Amount).Select(_ => (i.From, i.To))))
            Input.Layout[to].Push(Input.Layout[from].Pop());

        return new string(Input.Layout.OrderBy(s => s.Key).Select(stack => stack.Value.Peek()).ToArray());
    }

    public override object ExecutePart2()
    {
        foreach (var (amount, from, to) in Input.Instructions)
            foreach(var crate in Range(0, amount).Select(_ => Input.Layout[from].Pop()).Reverse())
                Input.Layout[to].Push(crate);

        return new string(Input.Layout.OrderBy(s => s.Key).Select(stack => stack.Value.Peek()).ToArray());
    }
}
