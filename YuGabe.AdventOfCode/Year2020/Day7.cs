using System.Collections.Generic;
using System.Linq;
using Tidy.AdventOfCode;

namespace YuGabe.AdventOfCode.Year2020
{
    public class Day7 : Day<ILookup<string, (int value, string name)>>
    {
        public override ILookup<string, (int value, string name)> ParseInput(string rawInput) =>
            rawInput.Split('\n').Where(l => !l.EndsWith("no other bags.")).Select(line => line.Split(' ')).SelectMany(
                e => Enumerable.Range(1, (e.Length - 3) / 4).Select(
                    i => (key: string.Join(' ', e[0], e[1]), value: (number: int.Parse(e[i * 4]), name: string.Join(' ', e[i * 4 + 1], e[i * 4 + 2]))))).ToLookup(e => e.key, e => e.value);

        public override object ExecutePart1() => Input.Count(e => GetChildrenNonRecursive(e.Key).Any(c => c.name == "shiny gold"));

        public override object ExecutePart2() => GetChildrenNonRecursive("shiny gold").Sum(c => c.totalValue);

        public IEnumerable<(int value, string name, int totalValue)> GetChildrenNonRecursive(string name)
        {
            var queue = new Queue<(int value, string name, int totalValue)>(Input[name].Select(c => (c.value, c.name, c.value)));

            while (queue.TryDequeue(out var value))
            {
                yield return value;
                foreach (var child in Input[value.name])
                    queue.Enqueue((child.value, child.name, child.value * value.totalValue));
            }
        }
    }
}
