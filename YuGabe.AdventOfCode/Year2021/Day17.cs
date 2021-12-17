namespace YuGabe.AdventOfCode.Year2021;

public class Day17 : Day<(int X1, int X2, int Y1, int Y2)>
{
    public override (int X1, int X2, int Y1, int Y2) ParseInput(string rawInput) => rawInput.Split(new[] { "..", "," }, SSO.None).Select(e => int.Parse(new string(e.Where(c => char.IsNumber(c) || c == '-').ToArray()))).ToArray().FeedTo(e => (e[0], e[1], e[2], e[3]));

    public override object ExecutePart1() => Simulate(Input.X1, Input.X2, Input.Y1, Input.Y2).Max()!;

    public override object ExecutePart2() => Simulate(Input.X1, Input.X2, Input.Y1, Input.Y2).Count(e => e != null);

    private static IEnumerable<int?> Simulate(int x1, int x2, int y1, int y2)
        => Range(0, x2 + 1).SelectMany(vx => Range(y1, (-y1 + 1) * 2).Select(vy => (vx, vy))).Select(e =>
        {
            var (posX, posY, vx, vy, topY) = (0, 0, e.vx, e.vy, 0);
            while (true)
            {
                (posX, posY, vx, vy, topY) = (posX + vx, posY + vy, Math.Max(vx - 1, 0), vy - 1, Math.Max(topY, posY));
                if (posX >= x1 && posX <= x2 && posY <= y2 && posY >= y1)
                    return (int?)topY;
                if ((vx == 0 && posX < x1) || posY < y1 || posX > x2)
                    return null;
            }
        });
}
