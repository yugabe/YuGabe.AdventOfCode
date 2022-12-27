using System.Linq.Expressions;
using static System.Linq.Expressions.ExpressionType;

namespace YuGabe.AdventOfCode.Year2022;
public class Day21 : Day<Dictionary<string, Day21.Operation>>
{
    public abstract record Operation(string Name, Dictionary<string, Operation> Operations)
    {
        public abstract long Value { get; }

        public abstract long? TryGetValue();

        public abstract long CalculateHumn(long? targetValue);

        public record Constant(string Name, long RawValue, Dictionary<string, Operation> Operations) : Operation(Name, Operations)
        {
            public override long Value => RawValue;

            public override long? TryGetValue() => RawValue;
            public override long CalculateHumn(long? targetValue) => throw new InvalidOperationException();
        }

        public record Humn(string Name, Dictionary<string, Operation> Operations) : Operation(Name, Operations)
        {
            public override long Value => throw new InvalidOperationException();

            public override long? TryGetValue() => null;
            public override long CalculateHumn(long? targetValue) => targetValue ?? throw new InvalidOperationException();
        }

        public record Binary(string Name, string Left, string Right, Dictionary<string, Operation> Operations) : Operation(Name, Operations)
        {
            public required ExpressionType ExpressionType { get; set; }
            public override long Value => ExpressionType switch
            {
                Add => Operations[Left].Value + Operations[Right].Value,
                Subtract => Operations[Left].Value - Operations[Right].Value,
                Multiply => Operations[Left].Value * Operations[Right].Value,
                Divide => Operations[Left].Value / Operations[Right].Value,
                _ => throw new NotImplementedException()
            };

            public override long CalculateHumn(long? targetValue)
            {
                var (left, right) = (Operations[Left], Operations[Right]);
                return (left.TryGetValue(), right.TryGetValue()) switch
                {
                    ({ } leftValue, null) => ExpressionType switch
                    {
                        Equal when targetValue is null => right.CalculateHumn(leftValue),
                        Add => right.CalculateHumn(targetValue - leftValue),
                        Subtract => right.CalculateHumn(leftValue - targetValue),
                        Multiply => right.CalculateHumn(targetValue / leftValue),
                        Divide => right.CalculateHumn(leftValue / targetValue),
                        _ => throw new NotImplementedException()
                    },
                    (null, { } rightValue) => ExpressionType switch
                    {
                        Equal when targetValue is null => left.CalculateHumn(rightValue),
                        Add => left.CalculateHumn(targetValue - rightValue),
                        Subtract => left.CalculateHumn(targetValue + rightValue),
                        Multiply => left.CalculateHumn(targetValue / rightValue),
                        Divide => left.CalculateHumn(targetValue * rightValue),
                        _ => throw new NotImplementedException()
                    },
                    (not null, not null) or (null, null) => throw new InvalidOperationException(),
                };
            }

            public override long? TryGetValue() => Operations[Left].TryGetValue() != null && Operations[Right].TryGetValue() != null ? Value : null;
        }
    }

    public override Dictionary<string, Operation> ParseInput(string rawInput)
    {
        var result = new Dictionary<string, Operation>();
        foreach (var block in rawInput.Split('\n').Select(b => b.Split(new char[] { ' ', ':' }, SSO.RemoveEmptyEntries)))
        {
            var name = block[0];
            if (block.Length == 2)
                result[name] = new Operation.Constant(name, long.Parse(block[1]), result);
            else
                result[name] = new Operation.Binary(name, block[1], block[3], result)
                {
                    ExpressionType = block[2].Single() switch
                    {
                        '+' => Add,
                        '-' => Subtract,
                        '*' => Multiply,
                        '/' => Divide,
                        _ => throw new NotImplementedException()
                    }
                };
        }
        return result;
    }

    public override object ExecutePart1() => Input["root"].Value;

    public override object ExecutePart2()
    {
        var root = (Operation.Binary)Input["root"];
        root.ExpressionType = Equal;
        Input.Remove("humn");
        Input["humn"] = new Operation.Humn("humn", Input);
        return root.CalculateHumn(null);
    }
}
