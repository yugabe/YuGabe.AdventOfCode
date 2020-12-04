using System;
using System.Linq;
using Tidy.AdventOfCode;

namespace YuGabe.AdventOfCode.Year2015
{
    public class Day8 : Day<string[]>
    {
        public override string[] ParseInput(string input) =>
            input.Trim().Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray();

        public override object ExecutePart1() =>
            Input.Select(p => (or: p, tx: p[1..^1].Replace("\\\\", "?").Replace("\\\"", "?"))).Sum(p => p.or.Length - (p.tx.Length - p.tx.Count(c => c == '\\') * 3));

        public override object ExecutePart2() =>
            Input.Sum(p => 2 + p.Count(c => c == '\\' || c == '\"'));
    }
}
