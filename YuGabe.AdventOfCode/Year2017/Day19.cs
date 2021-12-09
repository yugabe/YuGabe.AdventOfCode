namespace YuGabe.AdventOfCode.Year2017
{
    public class Day19 : Day<string[]>
    {
        public override string[] ParseInput(string input)
        {
            return input.Split("\n").ToArray();
        }

        private enum Direction
        {
            Up, Right, Down, Left
        }
        private static (int x, int y) GetPositionByDirection(int x, int y, Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return (x, y - 1);
                case Direction.Right:
                    return (x + 1, y);
                case Direction.Down:
                    return (x, y + 1);
                case Direction.Left:
                    return (x - 1, y);
            }
            throw new NotImplementedException();
        }
        public override object ExecutePart1()
        {
            var (x, y) = (Input[0].IndexOf('|'), 0);
            var direction = Direction.Down;
            string checkpoints = "";
            while (true)
            {
                (x, y) = GetPositionByDirection(x, y, direction);

                var nextChar = Input[y][x];
                if (char.IsLetter(nextChar))
                    checkpoints += nextChar;
                else if (nextChar == '+')
                    direction = Enum.GetValues(typeof(Direction)).Cast<Direction>()
                        .Where(d => d != direction && d != (Direction)(((int)direction + 2) % 4))
                        .Select(d => (dir: d, pos: GetPositionByDirection(x, y, d)))
                        .Single(np => np.pos.y >= 0 && np.pos.y < Input.Length && np.pos.x >= 0 && np.pos.x < Input[np.pos.y].Length && Input[np.pos.y][np.pos.x] != ' ')
                            .dir;
                else if (nextChar == ' ')
                    return checkpoints;
            }
        }

        public override object ExecutePart2()
        {
            var steps = 0;
            var (x, y) = (Input[0].IndexOf('|'), 0);
            var direction = Direction.Down;
            while (true)
            {
                steps++;
                (x, y) = GetPositionByDirection(x, y, direction);

                var nextChar = Input[y][x];
                if (nextChar == '+')
                    direction = Enum.GetValues(typeof(Direction)).Cast<Direction>()
                        .Where(d => d != direction && d != (Direction)(((int)direction + 2) % 4))
                        .Select(d => (dir: d, pos: GetPositionByDirection(x, y, d)))
                        .Single(np => np.pos.y >= 0 && np.pos.y < Input.Length && np.pos.x >= 0 && np.pos.x < Input[np.pos.y].Length && Input[np.pos.y][np.pos.x] != ' ')
                            .dir;
                else if (nextChar == ' ')
                    return steps;
            }
        }
    }
}
