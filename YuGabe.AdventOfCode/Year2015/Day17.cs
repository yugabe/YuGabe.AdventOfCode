namespace YuGabe.AdventOfCode.Year2015;

public class Day17 : Day.NewLineSplitParsed<int>
{
    public const int Target = 150;

    public static IEnumerable<IEnumerable<int>> Permutate(IEnumerable<int> source, int remainder)
    {
        if (!source.Any())
            yield break;
        var first = source.First();
        if (source.Skip(1).Any())
        {
            foreach (var permutation in source.SelectMany((element, index) => source.Where((_, i) => i != index).Permutate().Select(others => others.Prepend(element))))
                yield return permutation;
        }
        else
            yield return new[] { first };
    }

    public override object ExecutePart1()
    {
        var matchingIndexes = new HashSet<string>();

        foreach (var i in Enumerable.Range(0, Input.Length))
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            var count = Enumerable.Range(0, i).Permutate().Count();
            Console.WriteLine($"{i}: {sw.ElapsedMilliseconds} ms ({count} items)");
        }

        foreach (var permutation in Input.Permutate())
        {
            var sum = 0;
            foreach (var (element, index) in permutation.WithIndexes())
            {
                sum += element;
                if (sum == Target)
                    matchingIndexes.Add(string.Join(",", permutation.Take(index + 1)));
                if (sum >= Target)
                    break;
            }
        }

        return matchingIndexes.Count;
    }

    public override object ExecutePart2() => null;
}
