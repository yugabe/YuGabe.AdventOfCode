using System.Collections.Generic;
using System.Linq;
using Tidy.AdventOfCode;

namespace YuGabe.AdventOfCode.Year2020
{
    public class Day9 : Day.NewLineSplitParsed<long>
    {
        public override object ExecutePart1() => GetInvalidNumberPart1();

        private long GetInvalidNumberPart1() => GetValidity(Input, 25).First(e => !e.valid).value;

        public override object ExecutePart2()
        {
            var invalid = GetInvalidNumberPart1();

            var match = Enumerable.Range(0, Input.Length)
                .SelectMany(x => Enumerable.Range(x + 1, Input.Length - x)
                    .Select(y => (range: Input[x..y], x, y)))
                .First(e => e.range.Sum() == invalid).range;
            return match.Min() + match.Max();
        }

        public IEnumerable<(long value, int index, bool valid)> GetValidity(long[] values, int preambleSize) =>
            values.Skip(preambleSize)
                .Select((e, i) => (value: e, index: i + preambleSize, range: Input[i..(i + preambleSize)].ToHashSet()))
                .Select(e => (e.value, e.index, e.range.Any(n => e.range.Contains(e.value - n) && e.value - n != n)));
    }
}
