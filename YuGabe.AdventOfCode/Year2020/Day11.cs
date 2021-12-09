namespace YuGabe.AdventOfCode.Year2020
{
    public class Day11 : Day<Dictionary<(int X, int Y), bool?>>
    {
        public override Dictionary<(int X, int Y), bool?> ParseInput(string rawInput) =>
            rawInput.Split('\n').SelectMany((line, y) => line.Trim().Select((c, x) => (x, y, taken: c switch { 'L' => (bool?)false, '.' => null, _ => throw new InvalidOperationException() }))).ToDictionary(e => (e.x, e.y), e => e.taken);

        private static HashSet<(int dX, int dY)> AdjacentDifferences { get; } = new() { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) };

        public override object ExecutePart1()
        {
            Dictionary<(int X, int Y), bool?> previous = new(), current = new(Input); ;
            while (!previous.OrderBy(e => e.Key).SequenceEqual(current.OrderBy(e => e.Key)))
            {
                previous = current;
                current = current.Select(e => (k: e.Key, v: e.Value switch
                {
                    true => (bool?)(!AdjacentDifferences.Where(n => current.GetValueOrDefault((e.Key.X + n.dX, e.Key.Y + n.dY)) == true).Skip(3).Any()),
                    false => AdjacentDifferences.All(n => current.GetValueOrDefault((e.Key.X + n.dX, e.Key.Y + n.dY)) != true),
                    null => null
                })).ToDictionary(e => e.k, e => e.v);
            }
            return current.Count(e => e.Value == true);
        }

        public override object ExecutePart2()
        {
            Dictionary<(int X, int Y), bool?> previous = new(), current = new(Input);
            while (!previous.OrderBy(e => e.Key).SequenceEqual(current.OrderBy(e => e.Key)))
            {
                static int IncreaseSigned(int value) => value switch
                {
                    < 0 => value - 1,
                    0 => 0,
                    > 0 => value + 1
                };

                previous = current;
                current = current.Select(e => (k: e.Key, v: e.Value switch
                {
                    true => (bool?)(!AdjacentDifferences.Where(n =>
                    {
                        while (true)
                        {
                            if (!current.TryGetValue((e.Key.X + n.dX, e.Key.Y + n.dY), out var value) || value == false)
                                return false;
                            if (value == true)
                                return true;

                            n = (IncreaseSigned(n.dX), IncreaseSigned(n.dY));
                        }
                    }).Skip(4).Any()),
                    false => AdjacentDifferences.All(n =>
                    {
                        while (true)
                        {
                            if (!current.TryGetValue((e.Key.X + n.dX, e.Key.Y + n.dY), out var value) || value == false)
                                return true;
                            if (value == true)
                                return false;

                            n = (IncreaseSigned(n.dX), IncreaseSigned(n.dY));
                        }
                    }),
                    null => null
                })).ToDictionary(e => e.k, e => e.v);
            }
            return current.Count(e => e.Value == true);
        }

        public static void PrintSeating(int index, int totalColumns, Dictionary<(int X, int Y), bool?> values, bool halt)
        {
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Iteration #{index}".PadRight(totalColumns, '-'));
            Console.ResetColor();
            Console.WriteLine(string.Join("\n", values.GroupBy(e => e.Key.Y).OrderBy(g => g.Key).Select(row => new string(row.OrderBy(e => e.Key.X).Select(col => col.Value switch { true => '#', false => 'L', null => '.' }).ToArray()))));
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(new string('-', totalColumns));
            Console.ResetColor();
            if (halt)
                Console.ReadLine();
        }
    }
}
