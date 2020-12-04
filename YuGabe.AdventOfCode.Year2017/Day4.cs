using System;
using System.Linq;
using Tidy.AdventOfCode;

namespace YuGabe.AdventOfCode.Year2017
{
    public class Day4 : Day
    {
        public override object ExecutePart1()
        {
            return Input.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Count(p =>
            {
                var words = p.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                return words.Distinct().Count() == words.Length;
            });
        }

        public override object ExecutePart2()
        {
            return Input.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Count(p =>
            {
                var words = p.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(e => new string(e.OrderBy(c => c).ToArray())).ToArray();
                return words.Distinct().Count() == words.Length;
            });
        }
    }
}
