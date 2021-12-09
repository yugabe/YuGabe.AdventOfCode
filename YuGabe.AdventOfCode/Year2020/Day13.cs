using System.Linq;
using Tidy.AdventOfCode;

namespace YuGabe.AdventOfCode.Year2020
{
    public class Day13 : Day<(int Arrival, int?[] Buses)>
    {
        public override (int Arrival, int?[] Buses) ParseInput(string rawInput)
        {
            var lines = rawInput.Split('\n');
            return (int.Parse(lines[0]), lines[1].Split(',').Select(b => int.TryParse(b, out var value) ? (int?)value : null).ToArray());
        }

        public override object ExecutePart1() =>
            Input.Buses.Where(b => b != null).Cast<int>().Select(id => (id, wait: ((Input.Arrival / id) + 1) * id - Input.Arrival)).OrderBy(i => i.wait).First().FeedTo((id, wait) => id * wait);

        public override object ExecutePart2()
        {
            var buses = Input.Buses.WithIndexes().Where(b => b.Element != null).Select(e => (n: e.Element!.Value, a: e.Index)).OrderByDescending(b => b.n).ToArray();

            long x = buses[0].n - buses[0].a;
            long m = 1;
            for (var i = 0; i < buses.Length - 1; i++)
            {
                m *= buses[i].n;

                var target = (buses[i + 1].n - buses[i + 1].a) % buses[i + 1].n;
                if (target < 0)
                    target += buses[i + 1].n;
                while (x % buses[i + 1].n != target)
                    x += m;
            }

            return x;
        }
    }
}
