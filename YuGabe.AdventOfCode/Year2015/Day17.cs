namespace YuGabe.AdventOfCode.Year2015;

public class Day17 : Day.NewLineSplitParsed<int>
{
    public static IEnumerable<IEnumerable<(int number, int id)>> Permutate(IEnumerable<(int number, int id)> source, int remainder)
    {
        foreach (var (number, id) in source)
        {
            if (remainder == number)
                yield return new[] { (number, id) };
            foreach (var permutation in Permutate(source.Where(e => e.id > id && e.number <= remainder - number), remainder - number))
                yield return permutation.Prepend((number, id));
        }
    }

    public override object ExecutePart1() => Permutate(Input.WithIndexes(), 150).Count();

    public override object ExecutePart2()
    {
        var permutations = Permutate(Input.WithIndexes(), 150).ToList();
        var leastItems = permutations.Min(p => p.Count());
        return permutations.Count(p => p.Count() == leastItems);
    }
}
