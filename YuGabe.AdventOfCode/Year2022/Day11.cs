using System.Linq.Expressions;

namespace YuGabe.AdventOfCode.Year2022;
public class Day11 : Day<Day11.Monkey[]>
{
    public override Monkey[] ParseInput(string rawInput) => rawInput.Split("\n\n").Select(block => block.Split("\n")).Select(lines =>
        new Monkey(
            lines[0].TrimEnd(':')[^1] - '0',
            lines[1].Split(':')[1].Split(',', SSO.TrimEntries).Select(long.Parse),
            lines[2].Trim(),
            int.Parse(lines[3].Split(' ')[^1]),
            int.Parse(lines[4].Split(' ')[^1]),
            int.Parse(lines[5].Split(' ')[^1]))).ToArray();

    public class Monkey
    {
        public Monkey(int id, IEnumerable<long> items, string rawOperation, int testDivisibleBy, int ifTrue, int ifFalse)
        {
            Items = new Queue<Item>(60000);
            Items.Enqueue(items.Select(i => new Item { WorryLevel = i }));
            (Id, RawOperation, TestDivisibleBy, IfTrue, IfFalse) = (id, rawOperation, testDivisibleBy, ifTrue, ifFalse);
            var oldParam = Expression.Parameter(typeof(long), "old");
            var expressionTokens = rawOperation.Split(' ');
            var (left, op, right) = (expressionTokens[3], expressionTokens[4], expressionTokens[5]);
            OperationExpression = Expression.Lambda<Func<long, long>>(Expression.MakeBinary(op switch
            {
                "*" => ExpressionType.Multiply,
                "+" => ExpressionType.Add,
                _ => throw null!
            }, left == "old" ? oldParam : Expression.Constant(long.Parse(left)), right == "old" ? oldParam : Expression.Constant(long.Parse(right))), oldParam);
            Operation = OperationExpression.Compile();
        }
        public int Id { get; }
        public Queue<Item> Items { get; }
        public string RawOperation { get; }
        public Expression<Func<long, long>> OperationExpression { get; }
        public Func<long, long> Operation { get; }
        public int TestDivisibleBy { get; }
        public int IfTrue { get; }
        public int IfFalse { get; }

        public int Inspections { get; set; }

        public override string ToString() => $"""#{Id}: {Inspections,4} inspections, "{OperationExpression} % {TestDivisibleBy} == 0 ? {IfTrue} : {IfFalse}", {Items.Count,3} items = [{string.Join(", ", Items.Select(i => i.WorryLevel))}]""";
    }

    public class Item
    {
        public long WorryLevel { get; set; }
        public override string ToString() => $"({WorryLevel})";
    }

    public override object ExecutePart1() => Execute(w => w / 3);

    public override object ExecutePart2()
    {
        var allMods = Input.Aggregate(1, (acc, m) => acc * m.TestDivisibleBy);
        return Execute(w => w % allMods);
    }

    private long Execute(Func<long, long> newWorryLevelModifier)
    {
        for (var round = 1; round <= 10_000; round++)
        {
            foreach (var monkey in Input)
            {
                while (monkey.Items.TryDequeue(out var item))
                {
                    var originalWorryLevel = item.WorryLevel;
                    monkey.Inspections++;
                    item.WorryLevel = newWorryLevelModifier(monkey.Operation(item.WorryLevel));
                    var targetMonkey = item.WorryLevel % monkey.TestDivisibleBy == 0 ? monkey.IfTrue : monkey.IfFalse;
                    Input[targetMonkey].Items.Enqueue(item);
                }
            }
        }
        return Input.Select(m => m.Inspections).OrderDescending().Take(2).Aggregate((long)1, (acc, seed) => acc * seed);
    }
}
