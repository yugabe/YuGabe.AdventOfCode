namespace YuGabe.AdventOfCode.Year2022;
public partial class Day16
{
    public object ExecutePart1OG()
    {
        var workingValves = Input.Values.Where(v => v.FlowRate > 0).OrderByDescending(v => v.FlowRate).ToList();
        var encounteredMaxPressure = 0;
        return GetMaxPressure(0, new(), Input["AA"], 30);

        int GetMaxPressure(int totalPressure, HashSet<Valve> openValves, Valve current, int minutesRemaining)
        {
            if (minutesRemaining == 0)
            {
                if (totalPressure > encounteredMaxPressure)
                    encounteredMaxPressure = totalPressure;
                return totalPressure;
            }
            if (totalPressure + GetPressureUpperBounds(workingValves, openValves, minutesRemaining) <= encounteredMaxPressure)
                return totalPressure;

            return current.Neighbors.Select(n => GetMaxPressure(totalPressure, openValves, n, minutesRemaining - 1)).Append(current.FlowRate > 0 && !openValves.Contains(current) ? GetMaxPressure(totalPressure + (current.FlowRate * (minutesRemaining - 1)), new(openValves) { current }, current, minutesRemaining - 1) : 0).Max();

            static int GetPressureUpperBounds(List<Valve> flowingValves, HashSet<Valve> openValves, int minutesRemaining)
                => flowingValves.Where(f => !openValves.Contains(f)).Take(1 + (minutesRemaining / 3)).Select((v, i) => v.FlowRate * (minutesRemaining - (i * 3))).Sum();
        }
    }
    public object ExecutePart2_WorksFastOnTest_ButSlooowOnRealInput2()
    {
        var flowingValves = Input.Values.Where(v => v.FlowRate > 0).OrderByDescending(v => v.FlowRate).ToList();
        var pressureMemo = new Dictionary<(string MemoKey, int MinutesRemaining), int>();
        var encounteredMaxPressure = 0;
        var result = GetMaxPressure(0, new(), Input["AA"], Input["AA"], 26, true);
        return result;

        int GetMaxPressure(int totalPressure, ValveSet openValves, Valve yourPosition, Valve elephantsPosition, int minutesRemaining, bool yourTurn)
        {
            if (openValves.Count == flowingValves.Count)
                return totalPressure;

            if (minutesRemaining == 0)
            {
                if (totalPressure > encounteredMaxPressure)
                {
                    encounteredMaxPressure = totalPressure;
                    Console.WriteLine(encounteredMaxPressure);
                }
                return totalPressure;
            }

            if ((totalPressure + GetPressureUpperBounds(flowingValves, openValves, minutesRemaining)) <= encounteredMaxPressure)
                return 0;

            var current = yourTurn ? yourPosition : elephantsPosition;
            var ifOpens = current.FlowRate > 0 && !openValves.Contains(current) ? GetMaxPressure(totalPressure + (current.FlowRate * (minutesRemaining - 1)), new(openValves) { current }, yourPosition, elephantsPosition, minutesRemaining - (yourTurn ? 0 : 1), !yourTurn) : 0;

            var maxFromNextStep = current.Neighbors.Select(n => GetMaxPressure(totalPressure, openValves, yourTurn ? n : yourPosition, yourTurn ? elephantsPosition : n, minutesRemaining - (yourTurn ? 0 : 1), !yourTurn)).Prepend(ifOpens).Where(n => n != 0).DefaultIfEmpty().Max();
            return maxFromNextStep;
        }

        int GetPressureUpperBounds(List<Valve> flowingValves, ValveSet openValves, int minutesRemaining)
        {
            var key = (openValves.MemoKey, minutesRemaining);
            if (pressureMemo.TryGetValue(key, out var value))
                return value;

            var total = 0;
            var currentFlow = 0;
            var closedValves = flowingValves.Where(f => !openValves.Contains(f)).ToList();
            for (var i = 0; i < minutesRemaining; i++)
            {
                if (i % 3 is 0)
                {
                    currentFlow += closedValves.ElementAtOrDefault(i / 3 * 2)?.FlowRate ?? 0;
                    currentFlow += closedValves.ElementAtOrDefault((i / 3 * 2) + 1)?.FlowRate ?? 0;
                }
                total += currentFlow;
            }
            return pressureMemo[key] = total;
        }
    }

    public object ExecutePart2_WorksFastOnTest_ButSlooowOnRealInput()
    {
        var flowingValves = Input.Values.Where(v => v.FlowRate > 0).OrderByDescending(v => v.FlowRate).ToList();
        var maxPressure = 0;
        var memoCache = new Dictionary<(string DictionaryMemoKey, int MinutesRemaining), int>();
        var _ = GetOpenValvePositions(new(), Input["AA"], Input["AA"], 26, true).Count();
        return maxPressure;
        IEnumerable<Dictionary<Valve, int>> GetOpenValvePositions(ValveDictionary openValves, Valve yourPosition, Valve elephantsPosition, int minutesRemaining, bool yourTurn)
        {
            var current = yourTurn ? yourPosition : elephantsPosition;

            if (minutesRemaining == 0)
            {
                if (openValves.Count > 0)
                {
                    if (openValves.Pressure > maxPressure)
                    {
                        maxPressure = openValves.Pressure;
                        Console.WriteLine(maxPressure);
                        yield return openValves;
                    }
                }
                yield break;
            }

            if (GetMaxPossiblePressure(flowingValves, openValves, minutesRemaining) is var possible && possible <= maxPressure)
                yield break;

            if (current.FlowRate > 0 && !openValves.ContainsKey(current))
            {
                foreach (var path in GetOpenValvePositions(new(openValves) { [current] = minutesRemaining }, yourPosition, elephantsPosition, minutesRemaining - (yourTurn ? 0 : 1), !yourTurn))
                    yield return path;
            }

            foreach (var path in current.Neighbors.SelectMany(neighbor => GetOpenValvePositions(openValves, yourTurn ? neighbor : yourPosition, yourTurn ? elephantsPosition : neighbor, minutesRemaining - (yourTurn ? 0 : 1), !yourTurn)))
                yield return path;
        };

        static int GetPressure(IEnumerable<KeyValuePair<Valve, int>> openValves) => openValves.Sum(kv => kv.Key.FlowRate * (kv.Value - 1));

        int GetMaxPossiblePressure(List<Valve> flowingValves, ValveDictionary openValves, int minutesRemaining)
        {
            var key = (openValves.MemoKey, minutesRemaining);
            if (memoCache.TryGetValue(key, out var memo))
                return memo;

            return memoCache[key] = openValves.Pressure + GetPressure(flowingValves.Where(f => !openValves.ContainsKey(f)).Select((v, i) => new KeyValuePair<Valve, int>(v, minutesRemaining - (i / 2 * 3))).Where(kv => kv.Value > 0).Take(minutesRemaining));
        }
    }

    public class ValveDictionary : Dictionary<Valve, int>
    {
        public ValveDictionary() { }
        public ValveDictionary(ValveDictionary other) : base(other) { }
        private static Dictionary<string, int> Pressures { get; } = new();

        private string? _memoKey;
        public string MemoKey => _memoKey ??= string.Join("|", this.OrderBy(kv => kv.Key.Label).Select(kv => $"{kv.Key.Label}-{kv.Value}"));
        public int Pressure => Pressures.TryGetValue(MemoKey, out var value) ? value : Pressures[MemoKey] = this.Sum(kv => kv.Key.FlowRate * (kv.Value - 1));
    }

    public class ValveSet : HashSet<Valve>
    {
        public ValveSet() { }
        public ValveSet(IEnumerable<Valve> collection) : base(collection) { }

        private string? _memoKey;
        public string MemoKey => _memoKey ??= string.Join("|", this.Select(v => v.Label).Order());
    }
}
