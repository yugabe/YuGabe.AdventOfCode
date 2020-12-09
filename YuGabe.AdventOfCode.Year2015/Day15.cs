using System;
using System.Collections.Generic;
using System.Linq;
using Tidy.AdventOfCode;

namespace YuGabe.AdventOfCode.Year2015
{
    public class Day15 : Day<Day15.Ingredient[]>
    {
        public override Ingredient[] ParseInput(string rawInput) => rawInput.Split('\n').Select(l => l.Split(' ')).Select(l => new Ingredient(l[0].TrimEnd(':', ' '), int.Parse(l[2].TrimEnd(',')), int.Parse(l[4].TrimEnd(',')), int.Parse(l[6].TrimEnd(',')), int.Parse(l[8].TrimEnd(',')), int.Parse(l[10].TrimEnd(',')))).ToArray();

        public record Ingredient(string Name, long Capacity, long Durability, long Flavor, long Texture, long Calories)
        {
            public long CalculateScore(int multiplier) => multiplier * (Capacity + Durability + Flavor + Texture);
        }

        private static Func<Ingredient, long>[] PropertySelectors { get; } = { i => i.Capacity, i => i.Durability, i => i.Flavor, i => i.Texture };

        public override object ExecutePart1() =>
            GetQuantities(100, 4).Select(q => q.ToArray())
                .Max(q => PropertySelectors.Aggregate(1L, (acc, s) => acc * Math.Max(Input.WithIndexes().Sum(i => q[i.Index] * s(i.Element)), 0)));

        public override object ExecutePart2() =>
            GetQuantities(100, 4).Select(q => q.ToArray())
                .Where(q => Input.WithIndexes().Sum(i => i.Element.Calories * q[i.Index]) == 500)
                .Max(q => PropertySelectors.Aggregate(1L, (acc, s) => acc * Math.Max(Input.WithIndexes().Sum(i => q[i.Index] * s(i.Element)), 0)));
        public IEnumerable<int[]> GetQuantities(int sum, int length) =>
            (sum, length) switch
            {
                ( < 0, _) or (_, < 0) or ( > 0, 0) => throw new ArgumentOutOfRangeException(nameof(length)),
                (0, 0) => Enumerable.Empty<int[]>(),
                (_, 1) => new[] { new[] { sum } },
                _ => Enumerable.Range(0, sum + 1).SelectMany(i => GetQuantities(sum - i, length - 1).Select(r => r.Prepend(i).ToArray()))
            };
    }
}
