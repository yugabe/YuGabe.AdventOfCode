namespace YuGabe.AdventOfCode.Year2022;
public class Day12 : Day<Dictionary<Point2D<int>, char>>
{
    public override Dictionary<Point2D<int>, char> ParseInput(string rawInput) => rawInput.SplitAtNewLines().SelectMany((line, y) => line.Select((c, x) => (c, x, y))).ToDictionary(e => new Point2D<int>(e.x, e.y), e => e.c);

    public override object ExecutePart1() => Climb(Input)!.Value;

    public override object ExecutePart2() => Input.Where(p => p.Value is 'S' or 'a').Min(start => Climb(Input.ToDictionary(p => p.Key, p => p.Key == start.Key ? 'S' : p.Value == 'S' ? 'a' : p.Value)))!.Value;

    private static int? Climb(Dictionary<Point2D<int>, char> map)
    {
        var (start, end) = (map.Single(p => p.Value == 'S').Key, map.Single(p => p.Value == 'E').Key);
        var (visited, queue) = (new Dictionary<Point2D<int>, int>() { [start] = 0 }, new Queue<Point2D<int>>() { start });
        while (queue.TryDequeue(out var point))
        {
            var possibleNeighbors = new[] { point with { X = point.X - 1 }, point with { X = point.X + 1 }, point with { Y = point.Y - 1 }, point with { Y = point.Y + 1 } };
            foreach (var neighbor in possibleNeighbors.Where(CanClimb))
            {
                visited[neighbor] = visited[point] + 1;
                queue.Enqueue(neighbor);
            }

            bool CanClimb(Point2D<int> neighbor) => !visited.ContainsKey(neighbor) && map.TryGetValue(neighbor, out var n) && ((map[point], n) is ('S', 'a') or ('S', 'b') or ('z', 'E') || (n <= map[point] + 1 && n is >= 'a' and <= 'z'));
        }
        return visited.TryGetValue(end, out var value) ? value : null;
    }
}
