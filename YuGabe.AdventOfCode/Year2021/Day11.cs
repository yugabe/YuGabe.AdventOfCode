using static YuGabe.AdventOfCode.ConsoleUtilities.AdvancedConsole;

namespace YuGabe.AdventOfCode.Year2021;

public class Day11 : Day<Dictionary<Point2D<int>, int>>
{
    public override Dictionary<Point2D<int>, int> ParseInput(string rawInput) => rawInput.Split('\n', SSO.RemoveEmptyEntries | SSO.TrimEntries).SelectMany((r, ri) => r.Select((v, ci) => (v, ri, ci))).ToDictionary(e => new Point2D<int>(e.ci, e.ri), e => int.Parse(e.v.ToString()));

    public async Task<(int totalFlashes, int cycles)> Cycle(bool visualize, int delay, Func<int, bool> stopPredicate, CancellationToken cancellationToken = default)
    {
        var ((minX, maxX), (minY, maxY)) = (Input.MinMax(e => e.Key.X), Input.MinMax(e => e.Key.Y));

        var totalFlashes = 0;

        var cycles = 0;
        
        var handledFlashes = new HashSet<Point2D<int>>();

        while (!stopPredicate(cycles))
        {
            cycles++;
            var unhandledFlashes = new Queue<Point2D<int>>();
            handledFlashes.Clear();

            foreach (var key in Input.Keys)
                if (++Input[key] > 9)
                    unhandledFlashes.Enqueue(key);

            while (unhandledFlashes.TryDequeue(out var flash))
                if (handledFlashes.Add(flash))
                    foreach (var neighbor in GetValidNeighbors(flash))
                        if (++Input[neighbor] > 9)
                            unhandledFlashes.Enqueue(neighbor);

            if (visualize && delay > 0)
            {
                Console.Clear();
                Console.WindowWidth = Console.BufferWidth = Math.Max(Math.Max(Console.WindowWidth, Console.BufferWidth), maxX - minX + 2);
                Console.WriteLine($"After {cycles}:\n\n{string.Join("\n", Range(minY, maxY - minY + 1).Select(y => string.Join("", Range(minX, maxX - minY + 1).Select(x => new Point2D<int>(x, y)).Select(key => (key, value: Input[key])).Select(e => handledFlashes.Contains(e.key) ? Negative("0") : e.value.ToString()))))}\n");
                await Task.Delay(delay, cancellationToken);
            }

            foreach (var flash in handledFlashes)
                Input[flash] = 0;

            totalFlashes += handledFlashes.Count;

            IEnumerable<Point2D<int>> GetValidNeighbors(Point2D<int> flash) => new[] { flash.Y - 1, flash.Y, flash.Y + 1 }.SelectMany(y => new[] { flash.X - 1, flash.X, flash.X + 1 }.Select(x => new Point2D<int>(x, y))).Where(n => n != flash && n.X >= minX && n.X <= maxX && n.Y >= minY && n.Y <= maxY);
        }

        return (totalFlashes, cycles);
    }

    public override async Task<object> ExecutePart1Async(CancellationToken cancellationToken = default) 
        => (await Cycle(true, 50, i => i == 100, cancellationToken)).totalFlashes;

    public override async Task<object> ExecutePart2Async(CancellationToken cancellationToken = default) 
        => (await Cycle(true, 50, _ => Input.Values.All(v => v == 0), cancellationToken)).cycles;
}
