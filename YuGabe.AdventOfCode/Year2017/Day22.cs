using System.Text;

namespace YuGabe.AdventOfCode.Year2017
{
    public class Day22 : Day
    {
        public enum Direction { Up, Right, Down, Left }

        public override object ExecutePart1()
        {
            var nodes = new Dictionary<(int x, int y), bool>(10_000);
            var rows = Input.Trim().Split("\n");
            for (var ry = 0; ry < rows.Length; ry++)
                for (var rx = 0; rx < rows[ry].Length; rx++)
                    nodes[(rx, ry)] = rows[ry][rx] == '#';

            var direction = Direction.Up;
            var (x, y) = (rows[0].Length / 2, rows.Length / 2);
            var infections = 0;

            void Render()
            {
                Console.WindowWidth = nodes.Keys.Max(k => k.x) - nodes.Keys.Min(k => k.x) + 5;
                var sb = new StringBuilder();
                var lx = nodes.Keys.Min(k => k.x);
                var rx = nodes.Keys.Max(k => k.x);
                var ly = nodes.Keys.Min(k => k.y);
                var ry = nodes.Keys.Max(k => k.y);
                for (var b = ly; b <= ry; b++)
                {
                    for (var a = lx; a <= rx; a++)
                    {
                        nodes.TryGetValue((a, b), out var val);
                        var c = val ? '#' : '.';
                        if (a == x && b == y)
                        {
                            Console.Write(sb.ToString());
                            sb.Clear();
                            var oc = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(c);
                            Console.ForegroundColor = oc;
                        }
                        else sb.Append(c);
                    }
                    sb.Append('\n');
                }
                sb.Append('\n');
                Console.WriteLine(sb);
            }

            for (var i = 0; i < 10_000; i++)
            {
                direction = (Direction)(((int)direction + (nodes[(x, y)] ? 1 : 3)) % 4);

                nodes[(x, y)] = !nodes[(x, y)];
                if (nodes[(x, y)])
                    infections++;

                x += direction == Direction.Right ? 1 : direction == Direction.Left ? -1 : 0;
                y += direction == Direction.Down ? 1 : direction == Direction.Up ? -1 : 0;

                if (!nodes.ContainsKey((x, y)))
                    nodes[(x, y)] = false;
            }
            Render();

            return infections;
        }

        public enum NodeState { Clean, Weakened, Infected, Flagged }

        public override object ExecutePart2()
        {
            var nodes = new Dictionary<(int x, int y), NodeState>(10_000);
            var rows = Input.Trim().Split("\n");
            for (var ry = 0; ry < rows.Length; ry++)
                for (var rx = 0; rx < rows[ry].Length; rx++)
                    nodes[(rx, ry)] = rows[ry][rx] == '#' ? NodeState.Infected : NodeState.Clean;

            var direction = Direction.Up;
            var (x, y) = (rows[0].Length / 2, rows.Length / 2);
            var infections = 0;

            //var renderLookup = new Dictionary<NodeState, Color>
            //{
            //    [NodeState.Clean] = Color.NavajoWhite,
            //    [NodeState.Flagged] = Color.Orange,
            //    [NodeState.Infected] = Color.OrangeRed,
            //    [NodeState.Weakened] = Color.LightYellow
            //};


            for (var i = 0; i < 10_000_000; i++)
            {
                //void Render()
                //{
                //    //var bmp = new System.Drawing.Bitmap(380, 380);

                //    var sb = new StringBuilder();
                //    for (var b = -170; b <= 210; b++)
                //    {
                //        for (var a = -170; a <= 210; a++)
                //        {
                //            nodes.TryGetValue((a, b), out var val);
                //            //bmp.SetPixel(a, b, a == x && b == y ? renderLookup[val] : System.Drawing.Color.Orange );
                //        }
                //    }
                //    //bmp.Save($"frame{i.ToString().PadLeft(7)}", System.Drawing.Imaging.ImageFormat.Png);
                //}
                direction = (Direction)((((int)direction) + ((int)nodes[(x, y)]) + 3) % 4);

                nodes[(x, y)] = (NodeState)(((int)nodes[(x, y)] + 1) % 4);
                if (nodes[(x, y)] == NodeState.Infected)
                    infections++;

                x += direction == Direction.Right ? 1 : direction == Direction.Left ? -1 : 0;
                y += direction == Direction.Down ? 1 : direction == Direction.Up ? -1 : 0;

                if (!nodes.ContainsKey((x, y)))
                    nodes[(x, y)] = NodeState.Clean;
            }
            Console.WriteLine($"x: {nodes.Keys.Min(k => k.x)}-{nodes.Keys.Max(k => k.x)}, y: {nodes.Keys.Min(k => k.y)}-{nodes.Keys.Max(k => k.y)}");

            return infections;
        }
    }
}
