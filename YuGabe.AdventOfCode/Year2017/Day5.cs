namespace YuGabe.AdventOfCode.Year2017
{
    public class Day5 : Day
    {
        public override object ExecutePart1()
        {
            var input = Input.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            var counter = 0;
            var steps = 0;
            while (counter >= 0 && counter < input.Length)
            {
                steps++;
                var jumpBy = input[counter];
                ++input[counter];
                counter += jumpBy;
            }
            return steps;
        }

        public override object ExecutePart2()
        {
            var input = Input.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            var counter = 0;
            var steps = 0;
            while (counter >= 0 && counter < input.Length)
            {
                steps++;
                var jumpBy = input[counter];
                if (jumpBy >= 3)
                    --input[counter];
                else
                    ++input[counter];
                counter += jumpBy;
            }
            return steps;
        }
    }
}
