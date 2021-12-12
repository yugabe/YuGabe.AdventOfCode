namespace YuGabe.AdventOfCode.Year2021;

public class Day12 : Day<HashSet<(string From, string To)>>
{
    public override HashSet<(string From, string To)> ParseInput(string rawInput) => rawInput.SplitAtNewLines().Select(l => l.SplitToTuple2("-")).ToHashSet()!;

    public static ILookup<string, string> GetAllConnections(IEnumerable<(string From, string To)> caves) 
        => caves.SelectMany(c => new[] { (To: c.From, From: c.To), (c.To, c.From) }).ToLookup(c => c.From, c => c.To);

    public override object ExecutePart1() => GetAllPaths(GetAllConnections(Input), "start", "end", new(), false).Count();

    public override object ExecutePart2() => GetAllPaths(GetAllConnections(Input), "start", "end", new(), true).Count();

    private IEnumerable<List<string>> GetAllPaths(ILookup<string, string> connections, string start, string end, List<string> visitedCaves, bool canVisitSmallCaveTwice)
    {
        visitedCaves = visitedCaves.Append(start).ToList();
        if (start == end)
        {
            yield return visitedCaves;
            yield break;
        }

        canVisitSmallCaveTwice &= !visitedCaves.Any(v => v.ToLower() == v && visitedCaves.Count(e => e == v) > 1);

        foreach (var path in connections[start]
            .Where(n => n != "start" && (n.ToLower() != n || !visitedCaves.Contains(n) || canVisitSmallCaveTwice))
            .SelectMany(n => GetAllPaths(connections, n, "end", visitedCaves.ToList(), canVisitSmallCaveTwice)))
            yield return path;
    }
}
