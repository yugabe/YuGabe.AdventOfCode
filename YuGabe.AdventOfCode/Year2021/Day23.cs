namespace YuGabe.AdventOfCode.Year2021;

public class Day23 : Day
{
    public override string ParseInput(string rawInput) => new(rawInput.Where(".ABCD".Contains).ToArray());

    public static void Draw(string state, int? usedEnergy = null, bool clear = false, int? left = null, int? top = null)
    {
        Console.SetCursorPosition(left ?? Console.CursorLeft, top ?? Console.CursorTop);
        Console.WriteLine(StateToString(state, usedEnergy, left));
    }
    public static string StateToString(string state, int? usedEnergy = null, int? left = null) => new string(' ', left ?? 0).FeedTo(padding => $"#############\t{usedEnergy}\n{padding}#{state[0..11]}#\n{padding}###{state[11]}#{state[12]}#{state[13]}#{state[14]}###\n{padding}  #{state[15]}#{state[16]}#{state[17]}#{state[18]}#\n{padding}  #########\n");

    public static IEnumerable<(string Positions, int EnergyUsed)> GetNextPossibleStates(string positions, int energyUsed, string winState, Dictionary<char, ((int index, int depth)[] targetPositions, int unitCost, (int corridorPosition, int cost)[][] corridorPositionsMatrix)> goalDescriptors)
    {
        if (positions == winState)
            yield break;

        string Swap(int index1, int index2)
        {
            var positionArray = positions.ToCharArray();
            (positionArray[index1], positionArray[index2]) = (positionArray[index2], positionArray[index1]);
            return new string(positionArray);
        }

        foreach (var (target, (targetPositions, unitCost, corridorPositionsMatrix)) in goalDescriptors)
        {
            if (targetPositions.All(t => positions[t.index] == target))
                continue;

            if (targetPositions.FirstOrDefault(t => positions[t.index] != '.') is var (topItem, topDepth) && topItem != default)
                foreach (var (corridorPosition, cost) in corridorPositionsMatrix.SelectMany(e => e.TakeWhile(e => positions[e.corridorPosition] == '.')))
                    yield return (Swap(topItem, corridorPosition), energyUsed + ((cost + topDepth) * goalDescriptors[positions[topItem]].unitCost));

            if (targetPositions.Any(t => positions[t.index] != '.' && positions[t.index] != target))
                continue;

            var (topTarget, topTargetIndex) = targetPositions.First(t => positions[t.index] == '.');

            foreach (var corridorPositions in corridorPositionsMatrix)
            {
                foreach (var ((corridorPosition, cost), index) in corridorPositions.WithIndexes())
                {
                    if (index != 0 && positions[corridorPositions[index - 1].corridorPosition] != '.')
                        break;
                    if (positions[corridorPosition] == target)
                        yield return (Swap(corridorPosition, topTarget), energyUsed + ((cost + topTargetIndex) * unitCost));
                }
            }
        }
    }

    public static int Execute(string input, string winState, Dictionary<char, ((int index, int depth)[] targetPositions, int unitCost, (int corridorPosition, int cost)[][] corridorPositionsMatrix)> goalDescriptors)
    {
        var states = new Stack<(string Positions, int EnergyUsed, string? PreviousPositions)>(new[] { (input, 0, (string?)null) });
        var statesMinEnergy = new Dictionary<string, (int EnergyUsed, string? PreviousPositions)>();

        while (states.TryPop(out var state))
        {
            if (!statesMinEnergy.TryGetValue(state.Positions, out var value) || value.EnergyUsed > state.EnergyUsed)
            {
                statesMinEnergy[state.Positions] = (state.EnergyUsed, state.PreviousPositions);
                if (!statesMinEnergy.TryGetValue(winState, out var previousWinState) || state.EnergyUsed < previousWinState.EnergyUsed)
                    foreach (var (positions, energyUsed) in GetNextPossibleStates(state.Positions, state.EnergyUsed, winState, goalDescriptors))
                        states.Push((positions, energyUsed, state.Positions));
            }
        }

        return statesMinEnergy[winState].EnergyUsed;
    }

    public override object ExecutePart1() => Execute(Input, "...........ABCDABCD", new()
    {
        ['A'] = (new[] { 11, 15 }.WithIndexes().ToArray(), 1, new (int, int)[][] { new[] { (1, 2), (0, 3) }, new[] { (3, 2), (5, 4), (7, 6), (9, 8), (10, 9) } }),
        ['B'] = (new[] { 12, 16 }.WithIndexes().ToArray(), 10, new (int, int)[][] { new[] { (3, 2), (1, 4), (0, 5) }, new[] { (5, 2), (7, 4), (9, 6), (10, 7) } }),
        ['C'] = (new[] { 13, 17 }.WithIndexes().ToArray(), 100, new (int, int)[][] { new[] { (5, 2), (3, 4), (1, 6), (0, 7) }, new[] { (7, 2), (9, 4), (10, 5) } }),
        ['D'] = (new[] { 14, 18 }.WithIndexes().ToArray(), 1000, new (int, int)[][] { new[] { (7, 2), (5, 4), (3, 6), (1, 8), (0, 9) }, new[] { (9, 2), (10, 3) } })
    });
    public override object ExecutePart2() => Execute(Input = ParseInput($"{Input[0..15]}\n  #D#C#B#A#\n  #D#B#A#C#\n{Input[15..]}"), "...........ABCDABCDABCDABCD", new()
    {
        ['A'] = (new[] { 11, 15, 19, 23 }.WithIndexes().ToArray(), 1, new (int, int)[][] { new[] { (1, 2), (0, 3) }, new[] { (3, 2), (5, 4), (7, 6), (9, 8), (10, 9) } }),
        ['B'] = (new[] { 12, 16, 20, 24 }.WithIndexes().ToArray(), 10, new (int, int)[][] { new[] { (3, 2), (1, 4), (0, 5) }, new[] { (5, 2), (7, 4), (9, 6), (10, 7) } }),
        ['C'] = (new[] { 13, 17, 21, 25 }.WithIndexes().ToArray(), 100, new (int, int)[][] { new[] { (5, 2), (3, 4), (1, 6), (0, 7) }, new[] { (7, 2), (9, 4), (10, 5) } }),
        ['D'] = (new[] { 14, 18, 22, 26 }.WithIndexes().ToArray(), 1000, new (int, int)[][] { new[] { (7, 2), (5, 4), (3, 6), (1, 8), (0, 9) }, new[] { (9, 2), (10, 3) } })
    });
}
