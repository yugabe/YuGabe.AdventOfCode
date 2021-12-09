namespace YuGabe.AdventOfCode.Year2020
{
    public class Day15 : Day<LinkedList<ulong>>
    {
        public override LinkedList<ulong> ParseInput(string rawInput) =>
            new LinkedList<ulong>(rawInput.Split(',').Select(ulong.Parse));

        public override object ExecutePart1() => AtIndex(2020);
        public override object ExecutePart2() => AtIndex(30_000_000);

        public ulong AtIndex(ulong count)
        {
            var indexLookup = Input.WithIndexes().ToDictionary(e => e.Element, e => (ulong)e.Index);
            var last = Input.Last!.Value;
            indexLookup.Remove(last);
            var total = (ulong)Input.Count;
            while (total != count)
            {
                var val = indexLookup.TryGetValue(last, out var index) ? total - index - 1 : 0;
                indexLookup[last] = total - 1;
                last = val;
                total++;
            }
            return last;
        }
    }
}
