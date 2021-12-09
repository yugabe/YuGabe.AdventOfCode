namespace YuGabe.AdventOfCode.Year2017
{
    public class Day17 : Day<int>
    {
        public override int ParseInput(string input) => int.Parse(input);

        public override object ExecutePart1()
        {
            var buffer = new List<int>(2018) { 0 };
            var currentPos = 0;
            for (var i = 1; i <= 50000000; i++)
            {
                currentPos = (currentPos + Input) % buffer.Count;
                buffer.Insert(++currentPos, i);
            }
            return buffer[buffer.IndexOf(0) + 1];
        }

        public override object ExecutePart2()
        {
            var element = 0;
            var currentPos = 0;
            for (var i = 1; i <= 50000000; i++)
                if ((currentPos = (currentPos + Input) % i + 1) == 1)
                    element = i; // Who cares about the full state, amiright?
            return element;
        }
    }
}
