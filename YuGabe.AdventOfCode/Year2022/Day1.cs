namespace YuGabe.AdventOfCode.Year2022;
public class Day1 : Day<Day1.Elf[]>
{
    public record Elf(int[] FoodCalories)
    {
        public int TotalCalories { get; } = FoodCalories.Sum();
    }

    public override Elf[] ParseInput(string rawInput) => rawInput
        .SplitAtNewLines(splitOptions: StringSplitOptions.None)
        .SequentialPartition(string.IsNullOrWhiteSpace)
        .Select(e => new Elf(e.Select(int.Parse).ToArray()))
        .ToArray();

    public override object ExecutePart1() => Input.Select(elf => elf.TotalCalories).Max();

    public override object ExecutePart2() => Input.Select(elf => elf.TotalCalories).OrderDescending().Take(3).Sum();
}
