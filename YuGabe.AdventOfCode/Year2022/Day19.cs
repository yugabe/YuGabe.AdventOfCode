using System.Collections.Concurrent;

namespace YuGabe.AdventOfCode.Year2022;
public partial class Day19 : Day<Day19.Blueprint[]>
{
    public record Blueprint(short Number, short[][] RobotCosts);

    public override Blueprint[] ParseInput(string rawInput) => rawInput.Split("\n").Select(l => l.Split(':', ' ').Where(t => short.TryParse(t, out _)).Select(short.Parse).ToArray()).Select(t => new Blueprint(t[0], new short[][]
    {
        /*[Geode] = */new short[] { /*[Geode] =*/ 0, /*[Obsidian] =*/ t[6], /*[Clay] = */ 0, /*[Ore] =*/ t[5] },
        /*[Obsidian] = */new short[] { /*[Geode] =*/ 0, /*[Obsidian] =*/ 0, /*[Clay] =*/ t[4], /*[Ore] =*/ t[3] },
        /*[Clay] = */new short[] { /*[Geode] =*/ 0, /*[Obsidian] =*/ 0, /*[Clay] =*/ 0, /*[Ore] =*/ t[2] },
        /*[Ore] = */new short[] { /*[Geode] =*/ 0, /*[Obsidian] =*/ 0, /*[Clay] =*/ 0, /*[Ore] =*/ t[1] },
    })).ToArray();

    public static IReadOnlyDictionary<Blueprint, short> Execute2(Blueprint[] blueprints, short minutes)
    {
        var maxGeodesByBlueprint = new ConcurrentDictionary<Blueprint, short>();
        Parallel.ForEach(blueprints, blueprint =>
        {
            // 0: MinutesRemaining
            // 1: Robots[Geode]
            // 2: Robots[Obsidian]
            // 3: Robots[Clay]
            // 4: Robots[Ore]
            // 5: Resources[Geode]
            // 6: Resources[Obsidian]
            // 7: Resources[Clay]
            // 8: Resources[Ore]

            // could try partitioning into queues by modulo of priority?
            var queue = new PriorityQueue<short[], int>(new[] { (new short[] { minutes, 0, 0, 0, 1, 0, 0, 0, 0 }, int.MinValue) });
            short max = 0;
            Span<short> nextState = stackalloc short[9];
            while (queue.TryDequeue(out var state, out var priority))
            {
                var minutesRemaining = state[0];
                var geodeRobots = state[1];
                var geodes = state[5];
                if (geodeRobots > 0)
                {
                    var newMax = (short)((geodeRobots * minutesRemaining) + geodes);
                    if (newMax > max)
                    {
                        max = newMax;
                        Console.WriteLine($"Blueprint #{blueprint.Number}: {newMax}");
                    }
                }

                for (var robotType = 0; robotType < 4; robotType++) // robotType is the robot we're trying to build next
                {
                    var costs = blueprint.RobotCosts[robotType];
                    short maxWait = 0;
                    for (var costType = 0; costType < 4; costType++)
                    {
                        var neededResources = costs[costType] - state[5 + costType];
                        if (neededResources > 0)
                        {
                            var robotsCount = state[1 + costType];
                            if (robotsCount > 0)
                            {
                                var waitForCurrentResource = (short)Math.Ceiling((double)neededResources / robotsCount);
                                if (waitForCurrentResource > maxWait)
                                {
                                    if (waitForCurrentResource >= minutesRemaining)
                                        goto nextRobot; // not enough time to make, try next robotType
                                    else
                                        maxWait = waitForCurrentResource;
                                }
                            }
                            else
                                goto nextRobot; // not enough materials, and no robots to make them
                        }
                    }

                    maxWait++;
                    nextState[0] = (short)(minutesRemaining - maxWait);
                    nextState[1] = (short)(state[1] + (robotType == 0 ? 1 : 0));
                    nextState[2] = (short)(state[2] + (robotType == 1 ? 1 : 0));
                    nextState[3] = (short)(state[3] + (robotType == 2 ? 1 : 0));
                    nextState[4] = (short)(state[4] + (robotType == 3 ? 1 : 0));
                    nextState[5] = (short)(state[5] + (state[1] * maxWait) - costs[0]);
                    nextState[6] = (short)(state[6] + (state[2] * maxWait) - costs[1]);
                    nextState[7] = (short)(state[7] + (state[3] * maxWait) - costs[2]);
                    nextState[8] = (short)(state[8] + (state[4] * maxWait) - costs[3]);
                    var m = nextState[0];
                    var robots = nextState[1];
                    var bounds = nextState[5] + (robots * m); // definite number of geodes
                    for (var r = robots; r < robots + m; r++)
                    {
                        bounds += r * (m + r - robots - 1); // if every minute after, we'd build a geode harvester (regardless of whether we can afford to)...
                        if (bounds > max)
                        {
                            queue.Enqueue(nextState.ToArray(), -((nextState[1] * nextState[0]) + nextState[5]));
                            break;
                        }
                    }
                nextRobot:;
                }
            }
            maxGeodesByBlueprint[blueprint] = max;
            Console.WriteLine($" > Blueprint #{blueprint.Number} ({maxGeodesByBlueprint.Count} complete): {max}");
        });
        return maxGeodesByBlueprint;
    }

    public override object ExecutePart1() => Execute2(Input, 24).Sum(b => b.Key.Number * b.Value);
    public override object ExecutePart2() => Execute2(Input[..3], 32).Aggregate(1, (acc, kv) => acc * kv.Value);
}
