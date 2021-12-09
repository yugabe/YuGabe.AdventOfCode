namespace YuGabe.AdventOfCode.Year2017
{
    public class Day1 : Day
    {
        public override object ExecutePart1()
        {
            var input = Input.AsEnumerable().Select(c => int.Parse(c.ToString()));
            var last = input.Last();
            return input.Aggregate(0, (acc, next) => next == last ? acc + (last = next) : acc + ((last = next) - last));
        }

        public override object ExecutePart2()
        {
            var input = Input.AsEnumerable().Select(c => int.Parse(c.ToString())).ToArray();
            return input.Select((e, i) => e == input[(i + input.Length / 2) % input.Length] ? e : 0).Sum();
        }
    }
}
