using System.Collections;

namespace YuGabe.AdventOfCode.Year2018
{
    public class Day6 : Day<(int x, int y, int id)[]>
    {
        public override (int x, int y, int id)[] ParseInput(string input)
            => input.Trim().Split('\n').Select((l, i) => { var e = l.Split(", "); return (int.Parse(e[0]), int.Parse(e[1]), i + 1); }).ToArray();

        public class OffsetMap<T> : IEnumerable<(int x, int y, T value)>
        {
            private readonly T[,] _map;
            public (int MinX, int MaxX, int MinY, int MaxY) Dimensions { get; }
            public OffsetMap(int minX, int maxX, int minY, int maxY) // 44, 358, 40, 355
            {
                _map = new T[maxX - minX + 1, maxY - minY + 1];
                Dimensions = (minX, maxX, minY, maxY);
            }
            public T this[int x, int y]
            {
                get => _map[x - Dimensions.MinX, y - Dimensions.MinY];
                set => _map[x - Dimensions.MinX, y - Dimensions.MinY] = value;
            }

            public IEnumerator<(int x, int y, T value)> GetEnumerator()
                => Enumerable.Range(0, _map.GetLength(0)).SelectMany(x => Enumerable.Range(0, _map.GetLength(1)).Select(y => (x + Dimensions.MinX, y + Dimensions.MinY, _map[x, y]))).GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public override object ExecutePart1()
        {
            var map = new OffsetMap<int>(Input.Min(p => p.x), Input.Max(p => p.x), Input.Min(p => p.y), Input.Max(p => p.y));
            foreach (var (x, y, value) in map)
            {
                var closestTwo = Input.GroupBy(p => Math.Abs(p.x - x) + Math.Abs(p.y - y)).OrderBy(e => e.Key).First().Take(2).ToList();
                if (closestTwo.Count == 1)
                    map[x, y] = closestTwo[0].id;
            }
            var excludes = new HashSet<int>(map.Where(p => p.x == map.Dimensions.MinX || p.x == map.Dimensions.MaxX || p.y == map.Dimensions.MinY || p.y == map.Dimensions.MaxY).Select(p => p.value));
            return map.GroupBy(e => e.value).Where(e => e.Key != 0 && !excludes.Contains(e.Key)).Max(e => e.Count());
        }

        public override object ExecutePart2()
        {
            var map = new OffsetMap<int>(Input.Min(p => p.x), Input.Max(p => p.x), Input.Min(p => p.y), Input.Max(p => p.y));
            foreach (var (x, y, value) in map)
                map[x, y] = Input.Sum(p => Math.Abs(p.x - x) + Math.Abs(p.y - y));
            return map.Count(m => m.value < 10000);
        }
    }
}
