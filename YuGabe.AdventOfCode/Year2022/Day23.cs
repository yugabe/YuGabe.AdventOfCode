using System.Drawing;

namespace YuGabe.AdventOfCode.Year2022;
public class Day23 : Day<Dictionary<Point, Point?>>
{
    public Point[] Neighbors { get; } = new Point[] { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) };
    public (Point Destination, Point[] Neighbors)[] DecisionLookup { get; } = new List<(Point Destination, Point[] Neighbors)>()
        {
            ((0, -1), new Point[] { (-1, -1), (0, -1), (1, -1) }),
            ((0, 1), new Point[] { (-1, 1), (0, 1), (1, 1) }),
            ((-1, 0), new Point[] { (-1, -1), (-1, 0), (-1, 1) }),
            ((1, 0), new Point[] { (1, -1), (1, 0), (1, 1) })
        }.LoopInfinitely().Take(7).ToArray();

    public override Dictionary<Point, Point?> ParseInput(string rawInput) => rawInput.Split("\n").SelectMany((l, y) => l.Select((c, x) => (c, x, y)).Where(e => e.c == '#')).ToDictionary(e => new Point(e.x, e.y), e => (Point?)null);

    public override object ExecutePart1()
    {
        for (var round = 0; round < 10; round++)
            TryMove(round);

        var ((minX, maxX), (minY, maxY)) = (Input.MinMax(kv => kv.Key.X), Input.MinMax(kv => kv.Key.Y));
        return ((maxX - minX + 1) * (maxY - minY + 1)) - Input.Count;
    }

    public override object ExecutePart2()
    {
        var round = 0;
        while (TryMove(round)) 
            round++;
        return round + 1;
    }

    public bool TryMove(int round)
    {
        var moved = false;
        Input = Input.ToDictionary(kv => kv.Key, kv => Neighbors.Any(n => Input.ContainsKey(kv.Key + n)) && DecisionLookup[(round % 4)..((round % 4) + 4)].Where(kkv => kkv.Neighbors.All(v => !Input.ContainsKey(kv.Key + v))).TryGetFirst(out var proposedMove) ? kv.Key + proposedMove.Destination : (Point?)null);

        foreach (var elves in Input.Where(kv => kv.Value != null).GroupBy(kv => kv.Value).Where(v => !v.Skip(1).Any()).ToList())
        {
            moved = true;
            var (coord, moveTo) = elves.Single();
            Input.Remove(coord);
            Input[moveTo!.Value] = null;
        }

        return moved;
    }
}
