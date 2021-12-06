namespace YuGabe.AdventOfCode.Year2021;

public class Day6 : Day<int[]>
{
    public override int[] ParseInput(string rawInput) => rawInput.ToMany<int>(",");

    public override object ExecutePart1() => PredictFishPopulation(80);
    public override object ExecutePart2() => PredictFishPopulation(256);

    private long PredictFishPopulation(int days) 
        => Enumerable.Range(0, days)
                     .Aggregate(Input.ToLookup(l => l).ToDictionary(e => e.Key, e => e.LongCount()), (fishes, _) =>
                     {
                         fishes = fishes.ToDictionary(kv => kv.Key - 1, kv => kv.Value);
                         fishes[8] = fishes.TryGetValue(-1, out var negativeOne) ? negativeOne : 0;
                         fishes[6] = fishes.TryGetValue(6, out var six) ? six + negativeOne : negativeOne;
                         fishes.Remove(-1);
                         return fishes;
                     }).Sum(e => e.Value);
}
