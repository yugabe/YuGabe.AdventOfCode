namespace YuGabe.AdventOfCode.Year2015
{
    public class Day2 : Day<List<Day2.Box>>
    {
        public struct Box
        {
            public int Length;
            public int Width;
            public int Height;
            public int TotalSurface => 2 * Length * Width + 2 * Length * Height + 2 * Width * Height;
            public int TotalVolume => Length * Width * Height;
            public IEnumerable<int> Sides => new[] { Length, Width, Height };
            public int SmallestArea => Sides.OrderBy(i => i).Take(2).Aggregate(1, (a, c) => a * c);
            public int SmallestPerimeter => Sides.OrderBy(i => i).Take(2).Aggregate(0, (a, c) => a + 2 * c);
        }
        public override List<Box> ParseInput(string input) =>
            input.Split("\n").Select(r => r.Split("x").Select(int.Parse).ToArray()).Select(r => new Box { Length = r[0], Width = r[1], Height = r[2] }).ToList();
        public override object ExecutePart1() =>
            Input.Sum(p => p.TotalSurface + p.SmallestArea);

        public override object ExecutePart2() =>
            Input.Sum(p => p.TotalVolume + p.SmallestPerimeter);
    }
}
