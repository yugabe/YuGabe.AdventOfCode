namespace YuGabe.AdventOfCode.Year2022;
public partial class Day16 : Day<Dictionary<string, Day16.Valve>>
{
    public override Dictionary<string, Day16.Valve> ParseInput(string rawInput)
    {
        var dict = new Dictionary<string, Valve>();
        foreach (var valve in rawInput.Split("\n").Select(l => l.Split(new[] { ' ', '=', ';', ',' }, SSO.RemoveEmptyEntries)).Select(t => new Valve(dict, t[1], int.Parse(t[5]), t[10..])))
            dict[valve.Label] = valve;
        return dict;
    }

    public class Valve
    {
        public Valve(Dictionary<string, Valve> allValves, string label, int flowRate, string[] neighborValveLabels)
        {
            AllValves = allValves;
            Label = label;
            FlowRate = flowRate;
            NeighborValveLabels = neighborValveLabels;
            NeighborsLazy = new(() => new List<Valve>(neighborValveLabels.Length).Interleave(l => l.AddRange(NeighborValveLabels.Select(l => AllValves[l]).OrderByDescending(n => n.FlowRate))));
            AsString = $"[{Label}@{FlowRate}]";
        }
        public Dictionary<string, Valve> AllValves { get; }
        public string Label { get; }
        public int FlowRate { get; }
        public string[] NeighborValveLabels { get; }
        private Lazy<IEnumerable<Valve>> NeighborsLazy { get; }
        public IEnumerable<Valve> Neighbors => NeighborsLazy.Value;
        private string AsString { get; }
        public override string ToString() => AsString;
    }

    public override object ExecutePart1() => GetMaxPressure(SimplifyLayout(Input.Values), 0, new(), Input["AA"], 30);

    public override object ExecutePart2() => GetMaxPressure(SimplifyLayout(Input.Values), 0, new(), Input["AA"], Input["AA"], 26, 26, true);

    private static ILookup<Valve, (Valve To, int Cost)> SimplifyLayout(IEnumerable<Valve> valves)
        => valves.ToDictionary(v => v, from =>
        {
            var results = new Dictionary<Valve, int>() { [from] = 0 };
            var queue = new Queue<(Valve Valve, int Depth)>(from.Neighbors.Select(n => (n, 1)));

            while (queue.TryDequeue(out var next))
            {
                foreach (var (via, depth) in results.ToList().Where(r => r.Key.Neighbors.Contains(next.Valve)))
                    results[next.Valve] = results.TryGetValue(next.Valve, out var previousValue) ? Math.Min(previousValue, depth + 1) : depth + 1;

                foreach (var neighbor in next.Valve.Neighbors.Where(n => !results.ContainsKey(n)))
                    queue.Enqueue((neighbor, next.Depth + 1));
            }

            return results;
        })
        .Where(v => v.Key is { Label: "AA" } or not { FlowRate: 0 })
        .SelectMany(kv => kv.Value
            .Where(e => e.Key.FlowRate > 0)
            .Select(v => (From: kv.Key, To: v.Key, Cost: v.Value)))
        .ToLookup(e => e.From, e => (e.To, e.Cost));

    public static int GetMaxPressure(ILookup<Valve, (Valve To, int Cost)> costs, int totalPressure, HashSet<Valve> visited, Valve current, int minutesRemaining)
        => costs[current]
            .Select(e => (e.To, e.Cost, Minutes: minutesRemaining - (e.Cost + 1)))
            .Where(e => e.Minutes > 0 && !visited.Contains(e.To))
            .Select(e => GetMaxPressure(costs, totalPressure + (e.To.FlowRate * e.Minutes), new(visited) { e.To }, e.To, e.Minutes))
            .DefaultIfEmpty(totalPressure)
            .Max();

    public static int GetMaxPressure(ILookup<Valve, (Valve To, int Cost)> costs, int totalPressure, HashSet<Valve> visited, Valve yourPosition, Valve elephantsPosition, int yourMinutesRemaining, int elephantsMinutesRemaining, bool yourTurn)
        => (yourTurn
                ? costs[yourPosition]
                    .Select(e => (e.To, e.Cost, Minutes: yourMinutesRemaining - (e.Cost + 1)))
                    .Where(e => e.Minutes > 0 && !visited.Contains(e.To))
                    .Select(e => GetMaxPressure(costs, totalPressure + (e.To.FlowRate * e.Minutes), new(visited) { e.To }, e.To, elephantsPosition, e.Minutes, elephantsMinutesRemaining, false))
                : costs[elephantsPosition]
                    .Select(e => (e.To, e.Cost, Minutes: elephantsMinutesRemaining - (e.Cost + 1)))
                    .Where(e => e.Minutes > 0 && !visited.Contains(e.To))
                    .Select(e => GetMaxPressure(costs, totalPressure + (e.To.FlowRate * e.Minutes), new(visited) { e.To }, yourPosition, e.To, yourMinutesRemaining, e.Minutes, true)))
                .DefaultIfEmpty(totalPressure)
                .Max();
}
