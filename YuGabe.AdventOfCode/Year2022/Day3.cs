namespace YuGabe.AdventOfCode.Year2022;
public class Day3 : Day<Day3.Rucksack[]>
{
    public record Rucksack(HashSet<char> Left, HashSet<char> Right)
    {
        public HashSet<char> All { get; } = new(Left.Concat(Right));
    };

    public override Rucksack[] ParseInput(string rawInput) => rawInput.SplitAtNewLines().Select(l => new Rucksack(l[0..(l.Length / 2)].ToHashSet(), l[(l.Length / 2)..].ToHashSet())).ToArray();

    public override object ExecutePart1() => Input.Sum(sack => GetPriority(sack.Left.Intersect(sack.Right).Single()));

    public override object ExecutePart2() => Input.Select(elf => elf.All).Chunk(3).Sum(group => GetPriority(group[0].Intersect(group[1]).Intersect(group[2]).Single()));

    private static int GetPriority(char c) => c switch
    {
        >= 'a' and <= 'z' => c - 'a' + 1,
        >= 'A' and <= 'Z' => c - 'A' + 27,
        _ => throw null!
    };
}
