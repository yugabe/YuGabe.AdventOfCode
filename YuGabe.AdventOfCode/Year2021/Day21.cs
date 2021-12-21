namespace YuGabe.AdventOfCode.Year2021;

public class Day21 : Day<Day21.PlayerState[]>
{
    public override PlayerState[] ParseInput(string rawInput) => rawInput.SplitAtNewLines().Select((l, i) => new PlayerState(i + 1, 0, int.Parse(l[^1].ToString()))).ToArray();

    public record PlayerState(int Number, int Points, int Position) { }

    public override object ExecutePart1()
    {
        var (die, next, other) = ((totalTimesRolled: 0, lastRoll: 0), Input[0], Input.Skip(1).Single());
        int Roll() => (die = die with { totalTimesRolled = die.totalTimesRolled + 1, lastRoll = (die.lastRoll % 100) + 1 }).lastRoll;
        while (other.Points < 1000 && ((next.Position + Roll() + Roll() + Roll() - 1) % 10) + 1 is var newPosition)
            (next, other) = (other, next with { Points = next.Points + newPosition, Position = newPosition });
        return Math.Min(next.Points, other.Points) * die.totalTimesRolled;
    }

    public override object ExecutePart2()
    {
        var (universes, totalWins) = (new Stack<(PlayerState Next, PlayerState Other, long Parallels)>(new[] { (new PlayerState(1, 0, Input[0].Position), Input.Skip(1).Select((p, i) => new PlayerState(i + 2, 0, p.Position)).Single(), 1L) }), Input.Select(_ => 0L).ToArray());
        while (universes.TryPop(out var state) && state is var (next, other, parallels))
            foreach (var (newPoints, newPosition, universeCount) in new Dictionary<int, int>() { [3] = 1, [4] = 3, [5] = 6, [6] = 7, [7] = 6, [8] = 3, [9] = 1 }.Select(kv => (newPosition: ((next.Position + kv.Key - 1) % 10) + 1, universeCount: kv.Value)).Select(e => (newPoints: next.Points + e.newPosition, e.newPosition, e.universeCount)))
                if (newPoints >= 21)
                    totalWins[next.Number - 1] += parallels * universeCount;
                else
                    universes.Push(new(other, next with { Position = newPosition, Points = next.Points + newPosition }, parallels * universeCount));
        return totalWins.Max();
    }
}
