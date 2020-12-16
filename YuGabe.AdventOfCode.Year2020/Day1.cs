using System.Linq;
using Tidy.AdventOfCode;

namespace YuGabe.AdventOfCode.Year2020
{
    public class Day1 : Day.NewLineSplitParsed<int>
    {
        public override object ExecutePart1()
        {
            var set = Input.ToHashSet();
            return set.Select(num => (num, diff: 2020 - num)).First(i => set.Contains(i.diff)).FeedTo((num, diff) => num * diff);
        }

        public override object ExecutePart2()
        {
            var set = Input.ToHashSet();
            return set.SelectMany(num => set.Select(other => (num, other, diff: 2020 - num - other))).First(i => set.Contains(i.diff)).FeedTo((num, other, diff) => num * other * diff);
        }
    }
}
