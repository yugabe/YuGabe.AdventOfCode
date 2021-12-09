namespace YuGabe.AdventOfCode.Year2018
{
    public class Day1 : Day.NewLineSplitParsed<int>
    {
        public override object ExecutePart1() => Input.Sum();

        public override object ExecutePart2()
        {
            var found = new HashSet<int>();
            var freq = 0;
            while (true)
            {
                foreach (var item in Input)
                {
                    freq += item;
                    if (!found.Add(freq))
                        return freq;
                }
            }
        }
    }
}
