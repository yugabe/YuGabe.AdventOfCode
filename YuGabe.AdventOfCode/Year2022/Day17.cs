namespace YuGabe.AdventOfCode.Year2022;
public class Day17 : Day
{
    public class Stage
    {
        public byte[] Bytes { get; } = new byte[1024 * 1024 * 1024];
        public long[] Heights { get; } = new long[1024 * 1024 * 1024];
        public long LastRound { get; private set; }

        public const int LineWidth = 7;
        public int Top { get; private set; }

        private static byte GetXOrThrow(int x) => x >= 0 && x < LineWidth ? (byte)x : throw new ArgumentOutOfRangeException(nameof(x));

        public bool this[int x, int y]
        {
            get => (Bytes[y] & (1 << GetXOrThrow(x))) != 0;
            set
            {
                if (value)
                    Bytes[y] |= (byte)(1 << GetXOrThrow(x));
                else
                    Bytes[y] &= (byte)~(1 << GetXOrThrow(x));

                if (value && y >= Top)
                    Top = y + 1;
            }
        }

        public bool CanMoveRight(RockType rock, Point from) => rock.RightMostX + from.X + 1 < LineWidth && rock.LeftBottomRelativePoints.All(p => !this[p.X + from.X + 1, p.Y + from.Y]);
        public bool CanMoveLeft(RockType rock, Point from) => rock.LeftMostX + from.X > 0 && rock.LeftBottomRelativePoints.All(p => !this[p.X + from.X - 1, p.Y + from.Y]);
        public bool CanMoveDown(RockType rock, Point from) => from.Y > 0 && rock.LeftBottomRelativePoints.All(p => !this[p.X + from.X, p.Y + from.Y - 1]);
        public void Settle(RockType rock, Point from, int? currentRound = null)
        {
            foreach (var relativePoint in rock.LeftBottomRelativePoints)
                this[relativePoint.X + from.X, relativePoint.Y + from.Y] = true;
            if (currentRound is { } round)
                Heights[LastRound = round] = Top;
        }

        public string Print((RockType Rock, Point From)? currentRock)
        {
            var pending = currentRock is { Rock: var rock, From: var from } ? rock.LeftBottomRelativePoints.Select(p => (X: p.X + from.X, Y: p.Y + from.Y)).ToHashSet() : new();
            var buffer = string.Join("\n", Range(0, (int)Top + 7).Select(y => (int)Top + 6 - y).Select(y => string.Join("", Range(0, LineWidth).Select(x =>
            {
                if (this[x, y])
                    return '#';
                if (pending.Contains((x, y)))
                    return '@';
                return '.';
            })))).ToArray();
            return new(buffer);
        }

        public void SimulateFall(RockType rock, Func<char> nextInstructionAccessor, int? currentRound = null)
        {
            var leftBottom = new Point(2, Top + 3);
            while (true)
            {
                var nextInstruction = nextInstructionAccessor();
                if (nextInstruction == '<')
                {
                    if (CanMoveLeft(rock, leftBottom))
                        leftBottom.X--;
                }
                else if (nextInstruction == '>')
                {
                    if (CanMoveRight(rock, leftBottom))
                        leftBottom.X++;
                }
                else
                    throw null!;

                if (CanMoveDown(rock, leftBottom))
                    leftBottom.Y--;
                else
                    break;
            }

            Settle(rock, leftBottom, currentRound);
        }
    }

    public sealed class RockType
    {
        private RockType(Point[] leftBottomRelativePoints, string simpleVisual)
        {
            LeftBottomRelativePoints = leftBottomRelativePoints;
            (LeftMostX, RightMostX) = leftBottomRelativePoints.MinMax(p => p.X);
            AsString = $"{simpleVisual} ({string.Join("\n", Range(0, leftBottomRelativePoints.Max(p => p.Y) + 1).Select(y => new string(Range(0, RightMostX + 1).Select(x => leftBottomRelativePoints.Contains(new Point(x, y)) ? '#' : '.').ToArray())))})";
        }

        public IReadOnlyList<Point> LeftBottomRelativePoints { get; }
        public int LeftMostX { get; }
        public int RightMostX { get; }

        public static IEnumerable<RockType> InfiniteRocks => Rocks.LoopInfinitely();

        private static RockType[] Rocks { get; } = new RockType[]
        {
            new(new Point[] { (0, 0), (1, 0), (2, 0), (3, 0) }, "-"),
            new(new Point[] { (1, 0), (0, 1), (1, 1), (2, 1), (1, 2) }, "+"),
            new(new Point[] { (0, 0), (1, 0), (2, 0), (2, 1), (2, 2) }, "_|"),
            new(new Point[] { (0, 0), (0, 1), (0, 2), (0, 3) }, "|"),
            new(new Point[] { (0, 0), (0, 1), (1, 0), (1, 1) }, "[]"),
        };

        private string AsString { get; }
        public override string ToString() => AsString;
    }

    public override object ExecutePart1()
    {
        var stage = new Stage();
        var instructions = Input.LoopInfinitely().GetEnumerator();
        foreach (var rock in RockType.InfiniteRocks.Take(2022))
            stage.SimulateFall(rock, instructions.GetNext);
        return stage.Top;
    }

    public override object ExecutePart2()
    {
        var stage = new Stage();
        var instructions = Input.LoopInfinitely().GetEnumerator();
        var big = Input.Length;
        foreach (var (rock, round) in RockType.InfiniteRocks.Select((r, i) => (r, i + 1)))
        {
            stage.SimulateFall(rock, instructions.GetNext, round);

            if (round > big * 3 && round % big == 0)
            {
                for (var i = big + 1; i < stage.LastRound / 3; i++)
                {
                    if (Range(0, i).All(e =>
                         stage.Heights[big + e + 1] - stage.Heights[big + e] is var diff
                            && diff == stage.Heights[big + (2 * i) + e + 1] - stage.Heights[big + (2 * i) + e]
                            && diff == stage.Heights[big + (3 * i) + e + 1] - stage.Heights[big + (3 * i) + e]
                    ))
                    {
                        var (n, m) = ((1000000000000 - big) / i, (1000000000000 - big) % i);
                        var d = stage.Heights[big + i] - stage.Heights[big];
                        return (n * d) + stage.Heights[big + m];
                    }
                }
            }
        }
        throw new Exception("Should've found the repeating pattern already.");
    }
}
