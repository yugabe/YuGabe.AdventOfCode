namespace YuGabe.AdventOfCode.Year2020
{
    public class Day18 : Day<Day18.HomeworkExpression[]>
    {
        public override HomeworkExpression[] ParseInput(string rawInput) =>
            rawInput.Split('\n').Select(l => HomeworkExpression.Parse(Invert(l))).ToArray();

        public static string Invert(object text) => new string(text.ToString()?.Replace('(', 'x').Replace(')', '(').Replace('x', ')').Reverse().ToArray());

        public abstract record HomeworkExpression
        {
            private static HomeworkExpression ParseOperation(string text, int index, HomeworkExpression left) =>
                text.ElementAtOrDefault(index + 2) switch
                {
                    '+' => new AdditionExpression(left, Parse(text[(index + 4)..])),
                    '*' => new MultiplicationExpression(left, Parse(text[(index + 4)..])),
                    default(char) => left,
                    _ => throw new InvalidOperationException()
                };

            public static HomeworkExpression Parse(string rawExpression) =>
                int.TryParse(rawExpression[0].ToString(), out var n) && new ConstantExpression(n) is var number
                    ? ParseOperation(rawExpression, 0, number)
                    : rawExpression[0] == '(' && GetGroupBoundaryIndex(rawExpression, 0, false) is var index
                        ? ParseOperation(rawExpression, index, new GroupExpression(Parse(rawExpression[1..index])))
                        : throw new InvalidOperationException();

            public abstract long Value { get; }
        }

        public static int GetGroupBoundaryIndex(string text, int index, bool reverse)
        {
            var parentheses = 0;
            while (true)
            {
                parentheses += text[index] switch
                {
                    '(' => 1,
                    ')' => -1,
                    _ => 0
                };
                if (parentheses == 0)
                    return index;
                index = reverse ? index - 1 : index + 1;
            }
        }

        public record ConstantExpression(int Number) : HomeworkExpression
        {
            public override long Value => Number;
            public override string ToString() => Value.ToString();
        }

        public record AdditionExpression(HomeworkExpression Left, HomeworkExpression Right) : HomeworkExpression
        {
            public override long Value => Left.Value + Right.Value;
            public override string ToString() => $"{Right} + {Left}";
        }

        public record MultiplicationExpression(HomeworkExpression Left, HomeworkExpression Right) : HomeworkExpression
        {
            public override long Value => Left.Value * Right.Value;
            public override string ToString() => $"{Right} * {Left}";
        }

        public record GroupExpression(HomeworkExpression Child) : HomeworkExpression
        {
            public override long Value => Child.Value;
            public override string ToString() => $"({Child})";
        }

        public override object ExecutePart1() => Input.Sum(i => i.Value);

        public override object ExecutePart2() => ParseInput(string.Join('\n', Input.Select(i => i.ToString())
            .Select(s =>
            {
                while (true)
                {
                    var plusIndex = s.IndexOf('+');
                    if (plusIndex == -1)
                        return s.Replace('x', '+');
                    s = int.TryParse(s[plusIndex + 2].ToString(), out _)
                        ? s.Insert(plusIndex + 3, ")")
                        : s[plusIndex + 2] == '('
                            ? s.Insert(GetGroupBoundaryIndex(s, plusIndex + 2, false), ")")
                            : throw new InvalidOperationException();
                    s = int.TryParse(s[plusIndex - 2].ToString(), out _)
                        ? s.Insert(plusIndex - 2, "(")
                        : s[plusIndex - 2] == ')'
                            ? s.Insert(GetGroupBoundaryIndex(s, plusIndex - 2, true), "(")
                            : throw new InvalidOperationException();

                    s = $"{s[..(plusIndex + 1)]}x{s[(plusIndex + 2)..]}";
                }
            }))).Sum(i => i.Value);
    }
}
