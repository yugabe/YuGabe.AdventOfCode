using YuGabe.AdventOfCode.Common;
using Node = YuGabe.AdventOfCode.Common.Map2D<YuGabe.AdventOfCode.Year2022.Day22.Tile>.Node;
using static YuGabe.AdventOfCode.Year2022.Day22.Direction;

namespace YuGabe.AdventOfCode.Year2022;
public class Day22 : Day<Day22.Note>
{
    public record Note(Map2D<Tile> Map, Instruction[] Instructions);
    public enum Tile { None, Open, Wall }
    public enum Rotation { Left = -1, Unknown, Right = 1 }
    public enum Direction { Right, Down, Left, Up }
    public abstract record Instruction;
    public record Move(int Steps) : Instruction;
    public record Turn(Rotation Rotation) : Instruction;

    public override Note ParseInput(string rawInput)
    {
        var blocks = rawInput.Split("\n\n");
        var map = new Map2D<Tile>(blocks[0].Split('\n').Select((l, y) => (l, y)).SelectMany(e => e.l.Select((c, x) => (c, x, e.y))).Where(e => e.c != ' ').ToDictionary(e => (e.x + 1, e.y + 1), e => e.c switch { '.' => Tile.Open, '#' => Tile.Wall, _ => throw new InvalidOperationException() }));
        var instructions = new List<Instruction>();
        for (var i = 0; i < blocks[1].Length; i++)
        {
            var c = blocks[1][i];
            if (c is 'R' or 'L')
                instructions.Add(new Turn(c == 'R' ? Rotation.Right : Rotation.Left));
            else
            {
                var num = new string(blocks[1][i..].TakeWhile(char.IsNumber).ToArray());
                i += num.Length - 1;
                instructions.Add(new Move(int.Parse(num)));
            }
        }
        return new(map, instructions.ToArray());
    }

    public override object ExecutePart1()
        => Execute((node, dir) => (Input.Map.Where(dir switch
        {
            Up or Down => t => t.Key.X == node.X,
            Left or Right => t => t.Key.Y == node.Y,
            _ => throw new InvalidOperationException()
        }).OrderBy<KeyValuePair<(int X, int Y), Node>, int>(dir switch
        {
            Up => t => -t.Key.Y,
            Right => t => t.Key.X,
            Down => t => t.Key.Y,
            Left => t => -t.Key.X,
            _ => throw new InvalidOperationException()
        }).First().Value, dir));

    public override object ExecutePart2()
        => Execute((node, dir) => (node.X, node.Y, dir) switch
        {
            (1, > 100 and <= 150, Left) => (Input.Map[(51, 151 - node.Y)], Right),
            (1, > 150 and <= 200, Left) => (Input.Map[(node.Y - 100, 1)], Down),
            (51, > 0 and <= 50, Left) => (Input.Map[(1, 151 - node.Y)], Right),
            (51, > 50 and <= 100, Left) => (Input.Map[(node.Y - 50, 101)], Down),
            (50, > 150 and <= 200, Right) => (Input.Map[(node.Y - 100, 150)], Up),
            (100, > 50 and <= 100, Right) => (Input.Map[(node.Y + 50, 50)], Up),
            (100, > 100 and <= 150, Right) => (Input.Map[(150, 151 - node.Y)], Left),
            (150, > 0 and <= 50, Right) => (Input.Map[(100, 151 - node.Y)], Left),
            ( > 50 and <= 100, 1, Up) => (Input.Map[(1, node.X + 100)], Right),
            ( > 100 and <= 150, 1, Up) => (Input.Map[(node.X - 100, 200)], Up),
            ( > 0 and <= 50, 101, Up) => (Input.Map[(51, 50 + node.X)], Right),
            ( > 100 and <= 150, 50, Down) => (Input.Map[(100, node.X - 50)], Left),
            ( > 50 and <= 100, 150, Down) => (Input.Map[(50, node.X + 100)], Left),
            ( > 0 and <= 50, 200, Down) => (Input.Map[(node.X + 100, 1)], Down),
            _ => throw new InvalidOperationException()
        });

    public object Execute(Func<Node, Direction, (Node, Direction)> wrap)
    {
        var position = Input.Map.OrderBy(t => t.Key.Y).ThenBy(t => t.Key.X).First().Value;
        var direction = Right;
        var mapAsString = string.Join("\n", Range(1, 200).Select(y => string.Join("", Range(1, 150).Select(x => Input.Map.TryGetValue(((x + 1), (y + 1)), out var node) ? node.Value == Tile.Open ? '.' : '#' : ' '))));
        Console.WindowWidth = Console.BufferWidth = Math.Max(Math.Max(160, Console.WindowWidth), Console.BufferWidth);
        Console.SetCursorPosition(0, 5);
        Console.Write(mapAsString);

        foreach (var (instruction, index) in Input.Instructions.WithIndexes())
        {
            Console.SetCursorPosition(0, 0);
            Console.Write($"Position: ");
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Write($"({position.X}, {position.Y})");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write($", direction: ");
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(direction);
            Console.BackgroundColor = ConsoleColor.Black;

            Console.Write($"Instruction #{index + 1} of {Input.Instructions.Length}: ");
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(instruction);
            Console.BackgroundColor = ConsoleColor.Black;

            Console.SetCursorPosition(position.X - 1, 5 + position.Y - 1);
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.Write('.');
            Console.BackgroundColor = ConsoleColor.Black;

            if (instruction is Move { Steps: var steps })
            {
                for (var s = 0; s < steps; s++)
                {
                    var next = (Position: direction switch
                    {
                        Up => position.Up,
                        Right => position.Right,
                        Down => position.Down,
                        Left => position.Left,
                        _ => throw new InvalidOperationException()
                    }, Direction: direction);

                    if (next.Position == null)
                        next = wrap(position, direction);

                    Console.SetCursorPosition(0, 2);
                    Console.WriteLine($"Step {s + 1} / {steps} -> {next}");

                    Console.SetCursorPosition(next.Position!.X - 1, 5 + next.Position.Y - 1);
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    Console.Write(next.Position.Value == Tile.Open ? '.' : '#');
                    Console.BackgroundColor = ConsoleColor.Black;

                    if (next.Position!.Value == Tile.Open)
                        (position, direction) = next;
                    else
                        break;
                }
            }
            else if (instruction is Turn { Rotation: var rotation })
                direction = (Direction)((4 + (int)direction + (int)rotation) % 4);
        }

        return (1000 * position.Y) + (4 * position.X) + direction;
    }
}
