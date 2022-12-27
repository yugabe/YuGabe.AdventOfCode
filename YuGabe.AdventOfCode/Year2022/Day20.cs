namespace YuGabe.AdventOfCode.Year2022;
public class Day20 : Day<List<Day20.LinkedValue>>
{
    public const long DecryptionKey = 811589153;
    public override List<LinkedValue> ParseInput(string rawInput)
    {
        var list = new List<LinkedValue>(rawInput.Split("\n").Select(int.Parse).Select((n, i) => new LinkedValue { Value = n, OriginalIndex = i }));
        foreach (var (prev, cur, next) in list.WithNeighbors())
        {
            cur.Previous ??= prev!;
            cur.Next ??= next!;
        }
        list[0].Previous = list[^1];
        list[^1].Next = list[0];
        return list;
    }

    public class LinkedValue
    {
        private int rawValue;
        public required int Value { get => rawValue; init { rawValue = value; LongValue = DecryptionKey * rawValue; } }
        public long LongValue { get; private init; }
        public required int OriginalIndex { get; init; }
        public LinkedValue Previous { get; set; } = null!;
        public LinkedValue Next { get; set; } = null!;

        public override string ToString() => $"<{Previous?.Value}> {Value} <{Next?.Value}>";

        public void SwapWithNext()
        {
            var item = this;

            var (prev, next) = (item.Previous, item.Next);
            next.Next.Previous = item;
            prev.Next = next;
            (item.Previous, item.Next) = (next, next.Next);
            (next.Previous, next.Next) = (prev, item);
        }

        public void Swap(long times)
        {
            if (times > 0)
                for (var i = 0L; i < times; i++)
                    SwapWithNext();
            else
                for (var i = times; i < 0; i++)
                    SwapWithPrevious();
        }

        public void SwapWithPrevious() => Previous.SwapWithNext();
    }

    public override object ExecutePart1()
    {
        foreach (var item in Input)
            item.Swap(item.Value);
        return GetResult(c => c.Value);
    }

    public override object ExecutePart2()
    {
        for (var t = 0; t < 10; t++)
            foreach (var (item, index) in Input.WithIndexes())
                item.Swap(item.LongValue % (Input.Count - 1));
        return GetResult(c => c.LongValue);
    }

    public long GetResult(Func<LinkedValue, long> valueSelector)
    {
        var (current, sum) = (Input.First(e => e.Value == 0), 0L);
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 1000; j++)
                current = current.Next;
            sum += valueSelector(current);
        }
        return sum;
    }
}
