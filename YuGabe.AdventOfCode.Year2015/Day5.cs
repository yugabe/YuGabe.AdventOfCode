using System.Collections.Generic;
using System.Linq;
using Tidy.AdventOfCode;

namespace YuGabe.AdventOfCode.Year2015
{
    public class Day5 : Day<List<string>>
    {
        public override List<string> ParseInput(string input) =>
            input.Split("\n").ToList();

        public override object ExecutePart1()
        {
            var blackList = new List<string> { "ab", "cd", "pq", "xy" };
            return Input.Count(s => s.Count(c => "aeiou".Contains(c)) >= 3 &&
                s.Select((c, i) => (c, i)).Any(e => e.i != s.Length - 1 && e.c == s[e.i + 1]) &&
                blackList.All(e => !s.Contains(e)));
        }

        public override object ExecutePart2()
        {
            return Input.Count(s =>
                s.Select((c, i) => (c, i)).Any(e => e.i < s.Length - 2 && e.c == s[e.i + 2]) &&
                s.Select((c, i) => (c, i)).Any(e => e.i < s.Length - 2 && s[(e.i + 2)..].Contains(s.Substring(e.i, 2)))
            );
        }
    }
}
