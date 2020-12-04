using System;
using System.Collections.Generic;
using System.Linq;
using Tidy.AdventOfCode;

namespace YuGabe.AdventOfCode.Year2017
{
    public class Day6 : Day
    {
        public override object ExecutePart1()
        {
            var input = Input.Split("\t").Select(int.Parse).ToArray();
            var visits = new List<int[]>();
            var hits = 0;
            while (visits.All(v => !v.SequenceEqual(input)))
            {
                visits.Add(input.ToArray());
                hits++;
                var max = input.Max();
                var ix = Array.IndexOf(input, max);
                input[ix] = 0;
                while (max-- > 0)
                    ++input[++ix % input.Length];
            }
            return hits;
        }

        public override object ExecutePart2()
        {
            var input = Input.Split("\t").Select(int.Parse).ToArray();
            var visits = new List<int[]>();
            var hits = 0;
            var inLoop = false;
            while (true)
            {
                if (visits.All(v => !v.SequenceEqual(input)))
                {
                    visits.Add(input.ToArray());
                    var max = input.Max();
                    var ix = Array.IndexOf(input, max);
                    input[ix] = 0;
                    while (max-- > 0)
                        ++input[++ix % input.Length];
                    if (inLoop)
                        hits++;
                }
                else
                {
                    if (inLoop)
                        return hits;
                    else
                    {
                        inLoop = true;
                        visits.Clear();
                    }
                }
            }
        }
    }
}
