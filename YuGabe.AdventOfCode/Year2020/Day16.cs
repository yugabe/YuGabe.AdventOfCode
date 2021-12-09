using System.Linq;
using Tidy.AdventOfCode;
using static YuGabe.AdventOfCode.Year2020.Day16;

namespace YuGabe.AdventOfCode.Year2020
{
    using static TicketData;
    public class Day16 : Day<TicketData>
    {
        public record TicketData(Field[] Fields, int[] TicketValues, int[][] NearbyTicketValues)
        {
            public record Field(string Name, Interval[] Intervals) { }
            public record Interval(int Min, int Max)
            {
                public void Deconstruct(out int min, out int max) => (min, max) = (Min, Max);
                public bool ContainsInclusive(int value) => value >= Min && value <= Max;
            }
        }

        public override TicketData ParseInput(string rawInput)
        {
            static int[] TextToIntArray(string text) => text.Split(',').Select(int.Parse).ToArray();
            var parts = rawInput.Split('\n').SequentialPartition(string.IsNullOrWhiteSpace).ToArray();
            return new TicketData(parts[0].Select(l => l.Split(": ")).Select(i => new Field(i[0], i[1].Split(" or ").Select(s => s.Split("-").Select(int.Parse).ToArray()).Select(n => new Interval(n[0], n[1])).ToArray())).ToArray(), TextToIntArray(parts[1].Skip(1).First()), parts[2].Skip(1).Select(TextToIntArray).ToArray());
        }

        public override object ExecutePart1()
        {
            var intervals = Input.Fields.SelectMany(f => f.Intervals).ToArray();

            return Input.NearbyTicketValues.SelectMany(v => v).Where(v => intervals.All(i => !i.ContainsInclusive(v))).Sum();
        }

        public override object ExecutePart2()
        {
            var intervals = Input.Fields.SelectMany(f => f.Intervals).ToArray();
            var validTickets = Input.NearbyTicketValues.Where(n => n.All(v => intervals.Any(i => i.ContainsInclusive(v)))).Append(Input.TicketValues).ToArray();

            var keyToIndexes = Input.Fields.ToDictionary(f => f.Name, f => Enumerable.Range(0, Input.TicketValues.Length).Where(ix => validTickets.All(t => f.Intervals.Any(i => i.ContainsInclusive(t[ix])))).ToHashSet());

            while (keyToIndexes.Any(l => l.Value.Skip(1).Any()))
            {
                foreach (var (key, value) in keyToIndexes.Where(e => !e.Value.Skip(1).Any()).Select(e => (e.Key, e.Value.Single())))
                {
                    foreach (var other in keyToIndexes.Where(e => e.Key != key && e.Value.Contains(value)))
                        other.Value.Remove(value);
                }

                foreach (var v in keyToIndexes.SelectMany(kv => kv.Value.Select(v => (kv.Key, Value: v))).ToLookup(e => e.Value, e => e.Key).Where(vk => vk.Count() == 1))
                    keyToIndexes[v.Single()] = new[] { v.Key }.ToHashSet();
            }

            return keyToIndexes.Where(k => k.Key.StartsWith("departure")).Aggregate(1L, (acc, kv) => acc * Input.TicketValues[kv.Value.Single()]);
        }
    }
}
