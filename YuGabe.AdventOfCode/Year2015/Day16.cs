namespace YuGabe.AdventOfCode.Year2015;

using static Day16.SampleKind;

public class Day16 : Day<Day16.Sue[]>
{
    public enum SampleKind { Children, Cats, Samoyeds, Pomeranians, Akitas, Vizslas, Goldfish, Trees, Cars, Perfumes }

    public record Sue(int Id, Dictionary<SampleKind, int> Samples) { }

    public override Sue[] ParseInput(string rawInput) => rawInput.SplitAtNewLines().Select(line => line.Split(new[] { ' ', ':', ',' }, StringSplitOptions.RemoveEmptyEntries)).Select(tokens => new Sue(int.Parse(tokens[1]), tokens[2..].Chunk(2).ToDictionary(ch => Enum.Parse<SampleKind>(ch[0], true), ch => int.Parse(ch[1])))).ToArray();

    public static Dictionary<SampleKind, int> SueFacts { get; } = new()
    {
        { Children, 3 },
        { Cats, 7 },
        { Samoyeds, 2 },
        { Pomeranians, 3 },
        { Akitas, 0 },
        { Vizslas, 0 },
        { Goldfish, 5 },
        { Trees, 3 },
        { Cars, 2 },
        { Perfumes, 1 }
    };

    public override object ExecutePart1() => Input.Single(i => i.Samples.All(s => SueFacts.TryGetValue(s.Key, out var value) && s.Value == value)).Id;

    public override object ExecutePart2() => Input.Single(i => i.Samples.All(s => SueFacts.TryGetValue(s.Key, out var value) && s.Key switch
    {
        Cats or Trees => s.Value > value,
        Pomeranians or Goldfish => s.Value < value,
        _ => s.Value == value
    })).Id;
}
