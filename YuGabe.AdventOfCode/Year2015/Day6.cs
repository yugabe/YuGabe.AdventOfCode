namespace YuGabe.AdventOfCode.Year2015
{
    public class Day6 : Day<List<Day6.Instruction>>
    {
        public override List<Instruction> ParseInput(string input) =>
            input.Split("\n").Select(r => new Instruction().Parse(r)).ToList();

        public override object ExecutePart1()
        {
            var state = new bool[1000, 1000];
            foreach (var (i, x, y) in Input.SelectMany(i => Enumerable.Range(i.From.x, i.To.x - i.From.x + 1).SelectMany(x => Enumerable.Range(i.From.y, i.To.y - i.From.y + 1).Select(y => (i, x, y)))))
                state[x, y] = i.Verb == InstructionVerb.Toggle ? !state[x, y]
                    : i.Verb == InstructionVerb.Toggle;
            return Enumerable.Range(0, 1000000).Count(i => state[i / 1000, i % 1000]);
        }

        public override object ExecutePart2()
        {
            var state = new int[1000, 1000];
            foreach (var (i, x, y) in Input.SelectMany(i => Enumerable.Range(i.From.x, i.To.x - i.From.x + 1).SelectMany(x => Enumerable.Range(i.From.y, i.To.y - i.From.y + 1).Select(y => (i, x, y)))))
                state[x, y] = Math.Max(0, state[x, y] += i.Verb == InstructionVerb.Toggle ? 2 : i.Verb == InstructionVerb.TurnOff ? -1 : 1);
            return Enumerable.Range(0, 1000000).Sum(i => state[i / 1000, i % 1000]);
        }

        public enum InstructionVerb
        {
            TurnOn,
            TurnOff,
            Toggle
        }

        public struct Instruction
        {
            public InstructionVerb Verb;
            public (int x, int y) From;
            public (int x, int y) To;
            public string Raw;
            public override string ToString() => Raw;
            public Instruction Parse(string instruction)
            {
                Raw = instruction;
                if (instruction.StartsWith("toggle"))
                {
                    Verb = InstructionVerb.Toggle;
                    instruction = instruction["toggle".Length..];
                }
                else if (instruction.StartsWith("turn on"))
                {
                    Verb = InstructionVerb.TurnOn;
                    instruction = instruction["turn on".Length..];
                }
                else if (instruction.StartsWith("turn off"))
                {
                    Verb = InstructionVerb.TurnOff;
                    instruction = instruction["turn off".Length..];
                }
                (int, int) ParseNum() => (int.Parse(instruction.Substring(0, instruction.IndexOf(','))), int.Parse(new string(instruction.Skip(instruction.IndexOf(',') + 1).TakeWhile(c => c != ' ').ToArray())));
                From = ParseNum();
                instruction = instruction[(instruction.LastIndexOf(' ') + 1)..];
                To = ParseNum();
                return this;
            }
        }
    }
}
