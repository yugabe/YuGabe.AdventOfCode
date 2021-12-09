namespace YuGabe.AdventOfCode.Year2020
{
    using static Day12.Command;
    public class Day12 : Day<(Day12.Command command, int value)[]>
    {
        private static Dictionary<char, Command> Commands { get; } = Enum.GetValues<Command>().ToDictionary(c => c.ToString()[0], c => c);

        public override (Command command, int value)[] ParseInput(string rawInput) => rawInput.Split('\n').Select(l => (Commands[l[0]], int.Parse(l[1..]))).ToArray();

        public enum Command { North = 0, East = 1, South = 2, West = 3, Left, Right, Forward }

        public override object ExecutePart1() =>
            Input.Aggregate((x: 0, y: 0, direction: East), ExecuteCommandPart1).FeedTo((x, y, _) => Graphs.GetManhattanDistance((x, y)));

        public override object ExecutePart2() =>
            Input.Aggregate((waypoint: (x: 10, y: -1), ship: (x: 0, y: 0)), (acc, item) => item.command switch
                {
                    North => ((acc.waypoint.x, acc.waypoint.y - item.value), acc.ship),
                    East => ((acc.waypoint.x + item.value, acc.waypoint.y), acc.ship),
                    South => ((acc.waypoint.x, acc.waypoint.y + item.value), acc.ship),
                    West => ((acc.waypoint.x - item.value, acc.waypoint.y), acc.ship),
                    Right => (RotateRightPart2(acc.waypoint, item.value), acc.ship),
                    Left => (RotateRightPart2(acc.waypoint, 360 - item.value), acc.ship),
                    Forward => (acc.waypoint, (acc.ship.x + acc.waypoint.x * item.value, acc.ship.y + acc.waypoint.y * item.value)),
                    _ => throw new InvalidOperationException()
                }).FeedTo((_, ship) => Graphs.GetManhattanDistance(ship));

        public static (int x, int y, Command direction) ExecuteCommandPart1((int x, int y, Command direction) acc, (Command command, int value) item) =>
            item.command switch
            {
                North => (acc.x, acc.y - item.value, acc.direction),
                East => (acc.x + item.value, acc.y, acc.direction),
                South => (acc.x, acc.y + item.value, acc.direction),
                West => (acc.x - item.value, acc.y, acc.direction),
                Left => (acc.x, acc.y, (Command)((((int)acc.direction) + 3 * (item.value / 90)) % 4)),
                Right => (acc.x, acc.y, (Command)((((int)acc.direction) + (item.value / 90)) % 4)),
                Forward => ExecuteCommandPart1(acc, (acc.direction, item.value)),
                _ => throw new InvalidOperationException()
            };

        public static (int x, int y) RotateRightPart2((int x, int y) position, int degrees) => (degrees % 360) switch
        {
            0 => (position.x, position.y),
            90 => (-position.y, position.x),
            180 => (-position.x, -position.y),
            270 => (position.y, -position.x),
            _ => throw new InvalidOperationException()
        };
    }
}
