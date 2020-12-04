using System;
using System.Linq;
using Tidy.AdventOfCode;

namespace YuGabe.AdventOfCode.Year2017
{
    public class Day13 : Day
    {
        public class Scanner
        {
            public int Value { get; set; }
            public bool Down { get; set; }
            public int Depth { get; set; }
            public int Range { get; set; }
            public void Step()
            {
                Value += Down ? 1 : -1;
                if (Value == 1)
                    Down = true;
                if (Value == Range)
                    Down = false;
            }
            public void Reset()
            {
                Value = 1;
                Down = true;
            }
        }

        public override object ExecutePart1()
        {
            var layers = Input.Replace("\r\n", "\n").Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(r => r.Split(":", StringSplitOptions.RemoveEmptyEntries).Select(t => int.Parse(t.Trim())).ToArray()).Select(t => (depth: t[0], range: t[1], scanner: new Scanner { Value = 1, Down = true })).ToList();
            var severity = 0;
            for (var step = 0; step < layers.Max(l => l.depth); step++)
            {
                var (depth, range, scanner) = layers.FirstOrDefault(l => l.depth == step);

                if (scanner?.Value == 1)
                    severity += depth * range;
                foreach (var layer in layers)
                {
                    layer.scanner.Value += layer.scanner.Down ? 1 : -1;
                    if (layer.scanner.Value == 1)
                        layer.scanner.Down = true;
                    if (layer.scanner.Value == layer.range)
                        layer.scanner.Down = false;
                }
            }
            return severity;
        }

        public override object ExecutePart2()
        {
            var layers = Input.Replace("\r\n", "\n").Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(r => r.Split(":", StringSplitOptions.RemoveEmptyEntries).Select(t => int.Parse(t.Trim())).Select((t, x) => x == 1 ? (t - 1) * 2 : t).ToArray()).ToArray();
            var delay = 0;
            while (true)
            {
                if (layers.All(l => (l[0] + delay) % l[1] != 0))
                    return delay;
                delay++;
            }
        }
    }
}
