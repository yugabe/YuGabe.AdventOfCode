using System.Linq;
using Tidy.AdventOfCode;

namespace YuGabe.AdventOfCode.Year2018
{
    public class Day2 : Day.NewLineSplitParsed<string>
    {
        public override object ExecutePart1() =>
            Input.Count(p => p.GroupBy(c => c).Any(g => g.Count() == 2)) * Input.Count(p => p.GroupBy(c => c).Any(g => g.Count() == 3));

        public override object ExecutePart2() =>
            new string(Input.SelectMany(p => Input.Select(p2 => (p, p2)))
                .Select(p => p.p.Select((c, i) => (c, i)).Where(e => p.p2[e.i] == e.c))
                .First(p => p.Count() == Input[0].Length - 1).Select(c => c.c).ToArray());
    }
}
