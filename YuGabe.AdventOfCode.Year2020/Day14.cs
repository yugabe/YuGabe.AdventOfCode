using System;
using System.Collections.Generic;
using System.Linq;
using Tidy.AdventOfCode;

namespace YuGabe.AdventOfCode.Year2020
{
    public class Day14 : Day<(bool?[] mask, (ulong address, ulong value)[] values)[]>
    {
        public override (bool?[] mask, (ulong address, ulong value)[] values)[] ParseInput(string rawInput) =>
            rawInput.Split('\n').SequentialPartition(p => p.StartsWith("mask"), PartitioningMethod.KeepWithNext).Select(p =>
            {
                var items = p.ToArray();
                return (items[0].Trim().Split(" = ")[1].Select(e => e == '0' ? (bool?)false : e == '1' ? true : e == 'X' ? null : throw new InvalidOperationException()).ToArray(), items[1..].Select(l => (ulong.Parse(l[4..l.IndexOf(']')]), ulong.Parse(l.Split(" = ")[1]))).ToArray());
            }).ToArray();

        public override object ExecutePart1()
        {
            var memory = new Dictionary<ulong, ulong>();

            foreach (var (orMask, andMask, address, value) in Input.SelectMany(i => i.values.Select(v => (orMask: i.mask.Aggregate(0UL, (acc, b) => acc << 1 | (b == true ? 1 : 0)), andMask: i.mask.Aggregate(0UL, (acc, b) => acc << 1 | (b == false ? 1 : 0)), address: v.address, value: v.value))))
                memory[address] = value & ~andMask | orMask;

            return memory.Aggregate(0UL, (acc, e) => acc + e.Value);
        }

        public override object ExecutePart2()
        {
            var memory = new Dictionary<ulong, ulong>();

            foreach (var (mask, values) in Input)
            {
                var possibleValues = Math.Pow(2, mask.Count(m => m == null));

                var indexed = mask.Reverse().WithIndexes().Where(m => m.Element == null).Select((e, i) => (originalIndex: e.Index, newIndex: i)).ToDictionary(e => Math.Pow(2, e.newIndex), e => e.originalIndex);
                foreach (var (address, value) in values)
                {
                    for (var i = 0UL; i < possibleValues; i++)
                    {
                        var addressMask = 0UL;
                        for (var j = 1UL; j < possibleValues; j *= 2)
                        {
                            if ((i & j) != 0UL)
                                addressMask |= 1UL << indexed[j];
                        }

                        memory[address & ~mask.Aggregate(0UL, (acc, b) => acc << 1 | (b == null ? 1 : 0)) | mask.Aggregate(0UL, (acc, b) => acc << 1 | (b == true ? 1 : 0)) | addressMask] = value;
                    }
                }
            }

            return memory.Aggregate(0UL, (acc, e) => acc + e.Value);
        }
    }
}
