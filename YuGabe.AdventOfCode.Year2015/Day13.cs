using System;
using System.Collections.Generic;
using System.Linq;
using Tidy.AdventOfCode;

namespace YuGabe.AdventOfCode.Year2015
{
    public class Day13 : Day<List<(string person, string otherPerson, int delta)>>
    {
        public override List<(string person, string otherPerson, int delta)> ParseInput(string input) =>
            input.Split("\n").Where(s => !string.IsNullOrEmpty(s)).Select(s => s.Split())
                .Select(e => (e[0], e[10][0..^1], e[2] == "gain" ? int.Parse(e[3]) : -int.Parse(e[3]))).ToList();

        public override object ExecutePart1()
        {
            var param2 = Input.Select(p => (person: p.person[0], otherPerson: p.otherPerson[0], p.delta)).ToList();
            var allPeople = param2.Select(p => p.person).Distinct().ToList();
            var lookup = allPeople.ToList().ToDictionary(p => p, p => allPeople.Where(p2 => p2 != p).ToDictionary(p2 => p2, p2 => param2.Where(s => (s.person == p && s.otherPerson == p2) || (s.person == p2 && s.otherPerson == p)).Sum(e => e.delta)));
            var allSeatings = new HashSet<string>();
            var bestValue = 0;
            var i = allPeople.Count;
            var factorial = i;
            while (i != 1)
                factorial *= --i;
            while (allSeatings.Count != factorial)
            {
                var randomSeating = new string(allPeople.OrderBy(p => Guid.NewGuid()).ToArray());
                if (!allSeatings.Contains(randomSeating))
                {
                    var value = randomSeating.Aggregate((person: randomSeating.Last(), sum: 0), (previous, current) => (current, previous.sum + lookup[previous.person][current])).sum;
                    if (value > bestValue)
                        bestValue = value;
                    allSeatings.Add(randomSeating);
                }
            }
            return bestValue;
        }

        public override object ExecutePart2()
        {
            Input = Input.Concat(Input.Select(p => p.person).Distinct().SelectMany(p => new[] { ("X", p, 0), (p, "X", 0) })).ToList();
            return ExecutePart1();
        }
    }
}
