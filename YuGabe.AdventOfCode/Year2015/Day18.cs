namespace YuGabe.AdventOfCode.Year2015;
public class Day18 : Day<Dictionary<(int X, int Y), bool>>
{
    public int MaxX { get; set; }
    public int MaxY { get; set; }

    public override Dictionary<(int X, int Y), bool> Input
    {
        get => base.Input;
        set
        {
            base.Input = value;
            MaxX = Input.Keys.Max(k => k.X);
            MaxY = Input.Keys.Max(k => k.Y);
        }
    }

    public override Dictionary<(int X, int Y), bool> ParseInput(string rawInput) => rawInput.SplitAtNewLines().SelectMany((y, yi) => y.Select((v, xi) => (xi, yi, v))).ToDictionary(e => (e.xi, e.yi), e => e.v == '#');

    public int Execute(bool stuckLights) => Range(0, 100).Aggregate(Input, (map, step) => map.ToDictionary(e => e.Key, e => (stuckLights && (e.Key.X == 0 || e.Key.X == MaxX) && (e.Key.Y == 0 || e.Key.Y == MaxY)) || (Range(-1, 3).SelectMany(xo => Range(-1, 3).Where(yo => xo != 0 || yo != 0).Select(yo => map.TryGetValue((e.Key.X - xo, e.Key.Y - yo), out var value) && value)).Count(e => e) is var neighborsOn && (neighborsOn == 3 || (map[e.Key] && neighborsOn == 2)))).Interleave(Print)).Count(e => e.Value);

    public override object ExecutePart1() => Execute(false);
    public override object ExecutePart2()
    {
        Input[(0, 0)] = Input[(0, MaxY)] = Input[(MaxX, 0)] = Input[(MaxX, MaxY)] = true;
        return Execute(true);
    }


    public void Print(Dictionary<(int X, int Y), bool> map)
    {
        Console.CursorLeft = Console.CursorTop = 0;
        Console.Write(Range(0, MaxY + 1).SelectMany(y => Range(0, MaxX + 1).Select(x => map[(x, y)] ? '#' : '.').Append('\n')).ToArray());
    }
}
