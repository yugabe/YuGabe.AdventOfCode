using System.Text;

namespace YuGabe.AdventOfCode.Year2017
{
    public class Day14 : Day
    {
        public override object ExecutePart1()
        {
            Input = Input.Trim();
            byte[] Hash(string input)
            {
                var list = Enumerable.Range(0, 256).Select(i => (byte)i).ToArray();
                int skip = 0, pos = 0;
                foreach (var _ in Enumerable.Range(0, 64))
                {
                    foreach (var length in Encoding.ASCII.GetBytes(input).Concat(new byte[] { 17, 31, 73, 47, 23 }))
                    {
                        foreach (var e in list.Concat(list).Skip(pos).Take(length).Reverse().ToArray())
                        {
                            list[pos] = e;
                            pos = (pos + 1) % list.Length;
                        }
                        pos = (pos + skip++) % list.Length;
                    }
                }
                return Enumerable.Range(0, 16).Select(i => list.Skip(i * 16).Take(16).Aggregate((a, b) => (byte)(a ^ b))).ToArray();
            }
            return Enumerable.Range(0, 128).SelectMany(n => Hash($"{Input}-{n}")).Sum(b => Convert.ToString(b, 2).Count(c => c == '1'));
        }

        public override object ExecutePart2()
        {
            Input = Input.Trim();
            var regions = Enumerable.Range(0, 128).Select(_ => new int?[128]).ToArray();
            bool[] Hash(string input)
            {
                var list = Enumerable.Range(0, 256).Select(i => (byte)i).ToArray();
                int skip = 0, pos = 0;
                foreach (var length in Enumerable.Range(0, 64).SelectMany(_ => Encoding.ASCII.GetBytes(input).Concat(new byte[] { 17, 31, 73, 47, 23 })))
                {
                    foreach (var e in list.Concat(list).Skip(pos).Take(length).Reverse().ToArray())
                    {
                        list[pos] = e;
                        pos = (pos + 1) % list.Length;
                    }
                    pos = (pos + skip++) % list.Length;
                }
                return Enumerable.Range(0, 16).Select(i => list.Skip(i * 16).Take(16).Aggregate((a, b) => (byte)(a ^ b))).SelectMany(b => Convert.ToString(b, 2).PadLeft(8).Select(i => i == '1')).ToArray();
            }
            var currentRegionCounter = 0;
            var matrix = Enumerable.Range(0, 128).Select(i => Hash($"{Input}-{i}")).ToArray();

            void Visit(int x, int y, bool increase)
            {
                if (x >= 0 && y >= 0 && x < 128 && y < 128 && matrix[x][y] && (regions[x][y] == null || regions[x][y]!.Value < currentRegionCounter))
                {
                    regions[x][y] = currentRegionCounter;
                    Visit(x - 1, y, false);
                    Visit(x + 1, y, false);
                    Visit(x, y - 1, false);
                    Visit(x, y + 1, false);
                    if (increase)
                        currentRegionCounter++;
                }
            }
            for (var x = 0; x < 128; x++)
                for (var y = 0; y < 128; y++)
                    Visit(x, y, true);

            return regions.SelectMany(r => r).Distinct().Count(r => r != null);
        }
    }
}
