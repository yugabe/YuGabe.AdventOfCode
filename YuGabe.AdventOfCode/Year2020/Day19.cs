namespace YuGabe.AdventOfCode.Year2020
{
    public class Day19 : Day<(Day19.RuleEngine Engine, string[] Messages)>
    {
        public override (RuleEngine Engine, string[] Messages) ParseInput(string rawInput) =>
            rawInput.Split('\n').SequentialPartition(string.IsNullOrWhiteSpace).Split2().FeedTo((rules, messages) => (new RuleEngine(rules), messages.ToArray()));

        public sealed class RuleEngine
        {
            public RuleEngine(IEnumerable<string> rules) => Rules = rules.Select(ParseRule).ToDictionary(r => r.Number);

            public Rule ParseRule(string line) =>
                line.Split(": ").Split2().FeedTo((id, body) => new Rule(int.Parse(id), body.Contains('\"') ? body.Replace("\"", "").Single() : null, !body.Contains('\"') ? body.Split(" | ").Select(t => t.Split(' ').Select(int.Parse).ToArray()).ToArray() : null, this));

            private IDictionary<int, Rule> Rules { get; }

            public Rule this[int number] { get => Rules[number]; set => Rules[number] = value; }
        }

        public sealed record Rule(int Number, char? Value, int[][]? AndOrs, RuleEngine Engine)
        {
            public IEnumerable<ReadOnlyMemory<char>> GetMatchRemainders(ReadOnlyMemory<char> message) =>
                Value != null
                    ? message.Length >= 1 && message.Span[0] == Value ? (new[] { message[1..] }) : Enumerable.Empty<ReadOnlyMemory<char>>()
                    : AndOrs?.SelectMany(ands => ands.Aggregate(new[] { message }.AsEnumerable(), (acc, a) => acc.SelectMany(r => Engine[a].GetMatchRemainders(r)))) ?? throw new InvalidOperationException();
        }

        public override object ExecutePart1() => Input.Messages.Count(m => Input.Engine[0].GetMatchRemainders(m.AsMemory()).Any(r => r.Length == 0));

        public override object ExecutePart2()
        {
            foreach (var rule in new[] { "8: 42 | 42 8", "11: 42 31 | 42 11 31" }.Select(Input.Engine.ParseRule))
                Input.Engine[rule.Number] = rule;
            return ExecutePart1();
        }
    }
}
