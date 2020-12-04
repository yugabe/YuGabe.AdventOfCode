using System.Linq;
using Tidy.AdventOfCode;

namespace YuGabe.AdventOfCode.Year2020
{
    class Day1 : Day.NewLineSplitParsed<int>
    {
        public override object ExecutePart1()
        {
            var set = Input.ToHashSet();
            var pair = set.Select(num => (num, diff: 2020 - num)).First(i => set.Contains(i.diff));
            return pair.num * pair.diff;
        }

        public override object ExecutePart2()
        {
            var set = Input.ToHashSet();
            var pair = set.SelectMany(num => set.Select(other => (num, other, diff: 2020 - num - other))).First(i => set.Contains(i.diff));
            return pair.num * pair.other * pair.diff;
        }
    }
}
