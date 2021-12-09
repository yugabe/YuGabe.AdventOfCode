namespace YuGabe.AdventOfCode.Year2020
{
    public class Day6 : Day<IEnumerable<IEnumerable<string>>>
    {
        public override IEnumerable<IEnumerable<string>> ParseInput(string rawInput) => rawInput.Split('\n').SequentialPartition(string.IsNullOrEmpty);

        public override object ExecutePart1() => Input.Sum(group => group.SelectMany(line => line).Distinct().Count());

        public override object ExecutePart2() => Input.Sum(group => group.SelectMany(line => line).Distinct().Count(c => group.All(line => line.Contains(c))));
    }
}
