namespace YuGabe.AdventOfCode.Year2020
{
    public class Day9 : Day.NewLineSplitParsed<long>
    {
        public Day9() => InvalidNumberPart1 = new Lazy<long>(Input.Skip(25)
            .Select((e, i) => (value: e, index: i + 25, range: Input[i..(i + 25)].ToHashSet()))
            .First(e => e.range.Any(n => e.range.Contains(e.value - n) && e.value - n != n)).value);

        private Lazy<long> InvalidNumberPart1 { get; }

        public override object ExecutePart1() => InvalidNumberPart1.Value;

        public override object ExecutePart2() =>
            Enumerable.Range(0, Input.Length)
                .SelectMany(x => Enumerable.Range(x + 1, Input.Length - x)
                    .Select(y => (range: Input[x..y], x, y)))
                .First(e => e.range.Sum() == InvalidNumberPart1.Value).range.MinMax().FeedTo((x, y) => x + y);
    }
}
