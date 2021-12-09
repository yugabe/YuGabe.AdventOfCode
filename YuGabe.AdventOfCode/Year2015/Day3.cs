namespace YuGabe.AdventOfCode.Year2015
{
    public class Day3 : Day<List<Day3.Direction>>
    {
        public enum Direction
        {
            Up, Right, Down, Left
        }
        public override List<Direction> ParseInput(string input)
        {
            var lookup = new Dictionary<char, Direction>
            {
                ['^'] = Direction.Up,
                ['>'] = Direction.Right,
                ['v'] = Direction.Down,
                ['<'] = Direction.Left
            };
            return input.Select(c => lookup[c]).ToList();
        }

        public override object ExecutePart1()
        {
            var visits = new Dictionary<(int, int), int>() { [(0, 0)] = 1 };
            (int up, int right) = (0, 0);
            foreach (var d in Input)
            {
                if (d == Direction.Up)
                    up++;
                else if (d == Direction.Right)
                    right++;
                else if (d == Direction.Down)
                    up--;
                else if (d == Direction.Left)
                    right--;
                if (!visits.ContainsKey((up, right)))
                    visits[(up, right)] = 0;
                visits[(up, right)]++;
            }
            return visits.Count;
        }

        public override object ExecutePart2()
        {
            var visits = new Dictionary<(int, int), int>() { [(0, 0)] = 1 };
            var positions = new Dictionary<bool, (int, int)>
            {
                [true] = (0, 0),
                [false] = (0, 0)
            };
            (int rUp, int rRight) = (0, 0);
            foreach (var (d, s) in Input.Select((d, x) => (d, (x % 2) == 0)))
            {
                if (d == Direction.Up)
                    positions[s] = (positions[s].Item1 + 1, positions[s].Item2);
                else if (d == Direction.Right)
                    positions[s] = (positions[s].Item1, positions[s].Item2 + 1);
                else if (d == Direction.Down)
                    positions[s] = (positions[s].Item1 - 1, positions[s].Item2);
                else if (d == Direction.Left)
                    positions[s] = (positions[s].Item1, positions[s].Item2 - 1);
                if (!visits.ContainsKey(positions[s]))
                    visits[positions[s]] = 0;
                visits[positions[s]]++;
            }
            return visits.Count;
        }
    }
}
