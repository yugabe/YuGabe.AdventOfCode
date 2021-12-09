namespace YuGabe.AdventOfCode.Year2017
{
    public class Day3 : Day
    {
        enum Steps
        {
            Up, Left, Down, Right
        }
        public override object ExecutePart1()
        {
            var p = int.Parse(Input);
            int right = 0, up = 0, steps = 1, currentSteps = 0;
            var dir = Steps.Right;
            for (var i = 1; i < p; i++)
            {
                switch (dir)
                {
                    case Steps.Up:
                        up++;
                        break;
                    case Steps.Left:
                        right--;
                        break;
                    case Steps.Down:
                        up--;
                        break;
                    case Steps.Right:
                        right++;
                        break;
                }
                if (++currentSteps == steps)
                {
                    currentSteps = 0;
                    dir = (Steps)((int)(dir + 1) % 4);

                    if (dir == Steps.Left || dir == Steps.Right)
                        steps++;
                }
            }
            return Math.Abs(right) + Math.Abs(up);
        }

        public override object ExecutePart2()
        {
            var p = int.Parse(Input);
            var grid = new int[100, 100];
            int right = 0, up = 0, steps = 1, currentSteps = 0;
            var dir = Steps.Right;
            grid[50, 50] = 1;
            for (var i = 1; i < p; i++)
            {
                if (i > 1)
                {
                    var val = grid[right + 49, up + 49]
                        + grid[right + 49, up + 50]
                        + grid[right + 49, up + 51]
                        + grid[right + 50, up + 49]
                        + grid[right + 50, up + 51]
                        + grid[right + 51, up + 49]
                        + grid[right + 51, up + 50]
                        + grid[right + 51, up + 51];

                    if (val > p)
                        return val;
                    grid[right + 50, up + 50] = val;
                }
                switch (dir)
                {
                    case Steps.Up:
                        up++;
                        break;
                    case Steps.Left:
                        right--;
                        break;
                    case Steps.Down:
                        up--;
                        break;
                    case Steps.Right:
                        right++;
                        break;
                }

                if (++currentSteps == steps)
                {
                    currentSteps = 0;
                    dir = (Steps)((int)(dir + 1) % 4);

                    if (dir == Steps.Left || dir == Steps.Right)
                        steps++;
                }
            }
            return 0;
        }
    }
}
