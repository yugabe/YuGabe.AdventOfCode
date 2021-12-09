namespace YuGabe.AdventOfCode
{
    public class Graphs
    {
        public static int GetManhattanDistance((int x, int y) a, (int x, int y) b = default) => Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
    }
}
