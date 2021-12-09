using System.Drawing;

namespace YuGabe.AdventOfCode.Year2018
{
    public class Day3 : Day<List<Day3.Claim>>
    {
        public struct Claim
        {
            public readonly int Num;
            public readonly RectangleF RectangleF;

            public readonly Rectangle Rectangle;

            public Claim(int num, int x, int y, int width, int height)
            {
                Num = num;
                RectangleF = new RectangleF(x, y, width, height);
                Rectangle = new Rectangle(x, y, width, height);
            }
        }
        public override List<Claim> ParseInput(string input) =>
            input.TrimEnd().Split('\n').Select(l => l.Split(new[] { ' ', ',', 'x', ':' }, StringSplitOptions.RemoveEmptyEntries)).Select(split =>
                new Claim(int.Parse(split[0][1..]), int.Parse(split[2]), int.Parse(split[3]), int.Parse(split[4]), int.Parse(split[5]))).ToList();

        public override object ExecutePart1()
        {
            var (minLeft, maxRight) = (Input.Min(e => e.RectangleF.Left), Input.Max(e => e.RectangleF.Right));
            var (minTop, maxBottom) = (Input.Min(e => e.RectangleF.Top), Input.Max(e => e.RectangleF.Bottom));
            var hits = 0;
            for (float x = minLeft + 0.5f; x < maxRight; x++)
                for (float y = minTop + 0.5f; y < maxBottom; y++)
                {
                    var point = new PointF(x, y);
                    var hitOne = false;
                    foreach (var r in Input)
                    {
                        if (r.RectangleF.Contains(point))
                        {
                            if (hitOne)
                            {
                                hits++;
                                break;
                            }
                            else
                                hitOne = true;
                        }
                    }
                }
            return hits;
        }

        public override object ExecutePart2() =>
            Input.First(p => Input.All(p2 => !p.Rectangle.IntersectsWith(p2.Rectangle) || p2.Rectangle == p.Rectangle)).Num;
    }

    public static class RectangleExtensions
    {
        public static int Area(this Rectangle rectangle)
        {
            return rectangle.X * rectangle.Y;
        }
    }
}
