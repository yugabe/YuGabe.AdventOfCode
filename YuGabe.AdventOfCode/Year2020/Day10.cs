namespace YuGabe.AdventOfCode.Year2020
{
    public class Day10 : Day.NewLineSplitParsed<int>
    {
        public override object ExecutePart1() =>
            Input.OrderBy(e => e).WithNeighbors().Select(e => e.current - e.previous).Aggregate((ones: 0, threes: 1), (acc, e) => (acc.ones += e == 1 ? 1 : 0, acc.threes += e == 3 ? 1 : 0)).FeedTo((ones, threes) => ones * threes);

        public override object ExecutePart2()
        {
            Input = Input.Prepend(0).OrderBy(e => e).ToArray();

            var lookup = Input.SelectMany(n => new[] { n + 1, n + 2, n + 3 }.Where(Input.Contains).Select(m => (n, m))).ToLookup(e => e.n, e => e.m);
            
            var total = 1L;

            for (var i = 0; i < Input.Length;)
            {
                (total, i) = (GetCount(i), GetCount(i + 1), GetCount(i + 2), GetCount(i + 3)) switch
                {
                    (3, 3, 2, 1) => (total * 7, i + 3),
                    (3, 2, 1, _) => (total * 4, i + 2),
                    (2, 1, _, _) => (total * 2, i + 1),
                    (1, _, _, _) or (0, 0, 0, 0) => (total, i + 1),
                    _ => throw new InvalidOperationException()
                };
            }

            return total;

            int GetCount(int index) => index > Input.Length - 1 ? 0 : lookup[Input.ElementAtOrDefault(index)].Count();
        }
    }
}
