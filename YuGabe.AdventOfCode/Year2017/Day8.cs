namespace YuGabe.AdventOfCode.Year2017
{
    public class Day8 : Day
    {
        public override object ExecutePart1()
        {
            var input = Input.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Select(r => r.Split(" ", StringSplitOptions.RemoveEmptyEntries));
            var registers = input.Select(i => i[0]).Distinct().ToDictionary(i => i, i => 0);
            var operations = new Dictionary<string, Func<int, int, bool>>
            {
                { ">", (r, v) => r > v },
                { "<", (r, v) => r < v },
                { "==", (r, v) => r == v },
                { "!=", (r, v) => r != v },
                { "<=", (r, v) => r <= v },
                { ">=", (r, v) => r >= v },
            };
            foreach (var instruction in input.Where(i => operations[i[5]](registers[i[4]], int.Parse(i[6]))))
                registers[instruction[0]] += (instruction[1] == "inc" ? 1 : -1) * int.Parse(instruction[2]);
            return registers.Values.Max();
        }

        public override object ExecutePart2()
        {
            var input = Input.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Select(r => r.Split(" ", StringSplitOptions.RemoveEmptyEntries));
            var registers = input.Select(i => i[0]).Distinct().ToDictionary(i => i, i => 0);
            var operations = new Dictionary<string, Func<int, int, bool>>
            {
                { ">", (r, v) => r > v },
                { "<", (r, v) => r < v },
                { "==", (r, v) => r == v },
                { "!=", (r, v) => r != v },
                { "<=", (r, v) => r <= v },
                { ">=", (r, v) => r >= v },
            };
            return input.Where(i => operations[i[5]](registers[i[4]], int.Parse(i[6])))
                .Max(instruction => (registers[instruction[0]] += (instruction[1] == "inc" ? 1 : -1) * int.Parse(instruction[2])));
        }
    }
}
