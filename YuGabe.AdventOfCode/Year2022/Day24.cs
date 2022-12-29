namespace YuGabe.AdventOfCode.Year2022;
public class Day24 : Day<Day24.Map>
{
    public record Map(Gust[] Blizzard, int StartX, int EndX, int TopWallY, int RightWallX, int BottomWallY, int LeftWallX);
    public record Gust(Point StartPosition, Direction Direction)
    {
        public Point Position { get; set; } = StartPosition;
    }
    public enum Direction { Up, Right, Down, Left }

    public override Map ParseInput(string rawInput) => rawInput.Split("\n").FeedTo(lines => new Map(lines.SelectMany((l, y) => l.Select((c, x) => (c, x, y)).Where(e => "^>v<".Contains(e.c)).Select(g => new Gust((g.x, g.y), (Direction)"^>v<".IndexOf(g.c)))).ToArray(), lines[0].IndexOf('.'), lines[^1].IndexOf('.'), 0, lines[0].Length - 1, lines.Length - 1, 0));

    public override object ExecutePart1() => Execute(Input, new Point(Input.StartX, Input.TopWallY), new Point(Input.EndX, Input.BottomWallY));

    private static int Execute(Map map, Point start, Point end)
    {
        var steps = 0;
        var possiblePositions = new HashSet<Point>() { start };
        while (!possiblePositions.Contains(end))
        {
            steps++;
            var gustPositions = map.Blizzard.Select(gust => gust.Position = gust.Direction switch
            {
                Direction.Up => (gust.Position.X, gust.Position.Y - 1 > map.TopWallY ? gust.Position.Y - 1 : map.BottomWallY - 1),
                Direction.Right => (gust.Position.X + 1 < map.RightWallX ? gust.Position.X + 1 : map.LeftWallX + 1, gust.Position.Y),
                Direction.Down => (gust.Position.X, gust.Position.Y + 1 < map.BottomWallY ? gust.Position.Y + 1 : map.TopWallY + 1),
                Direction.Left => (gust.Position.X - 1 > map.LeftWallX ? gust.Position.X - 1 : map.RightWallX - 1, gust.Position.Y),
                _ => throw new InvalidOperationException()
            }).ToHashSet();

            possiblePositions = possiblePositions.SelectMany(p => p.CardinalNeighbors.Prepend(p)).Where(p => (p == start || p == end || (p.X > map.LeftWallX && p.X < map.RightWallX && p.Y > map.TopWallY && p.Y < map.BottomWallY)) && !gustPositions.Contains(p)).ToHashSet();
        }

        return steps;
    }

    public override object ExecutePart2()
    {
        var (start, end) = (new Point(Input.StartX, Input.TopWallY), new Point(Input.EndX, Input.BottomWallY));
        return Execute(Input, start, end) + Execute(Input, end, start) + Execute(Input, start, end);
    }
}
