namespace YuGabe.AdventOfCode.Year2017
{
    public class Day2 : Day
    {
        public override object ExecutePart1()
            => Input.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Sum(r =>
            {
                var cells = r.Split("\t", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                return cells.Max() - cells.Min();
            });

        public override object ExecutePart2()
            => Input.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Sum(r =>
            {
                int other = 0;
                var cells = r.Split("\t", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                var x = cells.First(c => (other = cells.FirstOrDefault(e => e != c && (e % c == 0))) != 0);
                return other / x;
            });
    }
}
