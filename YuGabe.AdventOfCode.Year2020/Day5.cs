using System;
using System.Collections.Generic;
using System.Linq;
using Tidy.AdventOfCode;

namespace YuGabe.AdventOfCode.Year2020
{
    class Day5 : Day<HashSet<int>>
    {
        public override HashSet<int> ParseInput(string rawInput) => 
            rawInput.Split('\n').Select(i => Convert.ToInt32(i.Replace('F', '0').Replace('B', '1').Replace('R', '1').Replace('L', '0'), 2)).ToHashSet();

        public override object ExecutePart1() => Input.Max();

        public override object ExecutePart2() => Input.First(i => !Input.Contains(i - 1) && Input.Contains(i - 2) && (i >> 3) is not 0b1 and not 0b1111111) - 1;
    }
}
