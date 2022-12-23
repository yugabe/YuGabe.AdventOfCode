using System.Collections.Concurrent;
using static YuGabe.AdventOfCode.Year2022.Day19.ResourceType;

namespace YuGabe.AdventOfCode.Year2022;
public partial class Day19
{
    public enum ResourceType { Geode, Obsidian, Clay, Ore }
    public record StateOG(int MinutesRemaining, Dictionary<ResourceType, int> Robots, Dictionary<ResourceType, int> Resources);
    public record BlueprintOG(int Number, Dictionary<ResourceType, Dictionary<ResourceType, int>> RobotCosts);
    public static BlueprintOG[] ParseInputOG(string rawInput) => rawInput.Split("\n").Select(l => l.Split(':', ' ').Where(t => int.TryParse(t, out _)).Select(int.Parse).ToArray()).Select(t => new BlueprintOG(t[0], new()
    {
        [Ore] = new() { [Ore] = t[1], [Clay] = 0, [Obsidian] = 0, [Geode] = 0 },
        [Clay] = new() { [Ore] = t[2], [Clay] = 0, [Obsidian] = 0, [Geode] = 0 },
        [Obsidian] = new() { [Ore] = t[3], [Clay] = t[4], [Obsidian] = 0, [Geode] = 0 },
        [Geode] = new() { [Ore] = t[5], [Clay] = 0, [Obsidian] = t[6], [Geode] = 0 },
    })).ToArray();

    public static IReadOnlyDictionary<BlueprintOG, int> ExecuteOG(BlueprintOG[] blueprints, int minutes)
    {
        var startingState = new StateOG(minutes, new() { [Ore] = 1, [Clay] = 0, [Obsidian] = 0, [Geode] = 0 }, new() { [Ore] = 0, [Clay] = 0, [Obsidian] = 0, [Geode] = 0 });
        var queues = blueprints.Select(b => new PriorityQueue<(StateOG State, BlueprintOG Blueprint), int>(new[] { ((startingState, b), int.MinValue) })).ToList();
        var maxGeodesByBlueprint = new ConcurrentDictionary<BlueprintOG, int>();
        var results = Parallel.ForEach(blueprints, new() { MaxDegreeOfParallelism = Environment.ProcessorCount }, blueprint =>
        {
            var queue = new PriorityQueue<StateOG, int>(new[] { (startingState, int.MinValue) });
            var max = 0;
            while (queue.TryDequeue(out var state, out var priority))
            {
                var geodeRobots = state.Robots[Geode];
                if (geodeRobots > 0)
                {
                    var newMax = (geodeRobots * state.MinutesRemaining) + state.Resources[Geode];
                    if (newMax > max)
                    {
                        max = newMax;
                        Console.WriteLine($"Blueprint #{blueprint.Number}: {newMax}");
                    }
                }

                if (GetGeodeUpperBoundsOG(state) is var bounds && bounds <= max)
                    continue;

                foreach (var (robotType, costs) in blueprint.RobotCosts)
                {
                    var minutesForCosts = costs.Select(rc =>
                    {
                        var neededResources = rc.Value - state.Resources[rc.Key];
                        if (neededResources <= 0)
                            return (int?)0; // already available
                        if (state.Robots[rc.Key] is var robotsCount && robotsCount > 0)
                            return (int)Math.Ceiling((double)neededResources / robotsCount); // time for current robots to make deficit
                        return null; // unobtainable
                    }).ToList();
                    if (minutesForCosts.All(m => m != null))
                    {
                        var minutesToWait = 1 + minutesForCosts.Max() ?? throw new Exception("All costs should be obtainable at this point.");
                        if (minutesToWait < state.MinutesRemaining)
                        {
                            var nextState = new StateOG(
                                                            state.MinutesRemaining - minutesToWait,
                                                            new(state.Robots) { [robotType] = state.Robots[robotType] + 1 },
                                                            new(state.Resources.ToDictionary(
                                                                r => r.Key,
                                                                r => r.Value + (state.Robots[r.Key] * minutesToWait) - costs[r.Key])));
                            if (GetGeodeUpperBoundsOG(nextState) is var nextBounds && nextBounds > max)
                                queue.Enqueue(nextState, GetPriorityOG(nextState));
                        }
                    }
                }
            }
            maxGeodesByBlueprint[blueprint] = max;
            Console.WriteLine($" > Blueprint #{blueprint.Number} ({maxGeodesByBlueprint.Count} complete): {max}");
        });
        return maxGeodesByBlueprint;
    }

    public static int GetPriorityOG(StateOG state) =>
        -((10 * ((state.Robots[Geode] * state.MinutesRemaining) + state.Resources[Geode])) +
        (5 * ((state.Robots[Obsidian] * state.MinutesRemaining) + state.Resources[Obsidian])) +
        (3 * ((state.Robots[Clay] * state.MinutesRemaining) + state.Resources[Clay])) +
        (1 * ((state.Robots[Ore] * state.MinutesRemaining) + state.Resources[Ore])));

    public static int GetGeodeUpperBoundsOG(StateOG state)
    {
        var robots = state.Robots[Geode];
        return state.Resources[Geode] + (robots * state.MinutesRemaining) + Range(robots, state.MinutesRemaining).Sum(r => r * (state.MinutesRemaining + r - robots - 1));
    }

    //public object ExecutePart1OG() => Execute(Input, 24).Sum(b => b.Key.Number * b.Value);

    //public object ExecutePart2OG() => Execute(Input[..3], 32).Aggregate(1, (acc, kv) => acc * kv.Value);
}
