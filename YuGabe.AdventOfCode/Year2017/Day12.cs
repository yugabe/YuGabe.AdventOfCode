namespace YuGabe.AdventOfCode.Year2017
{
    public class Day12 : Day
    {
        public override object ExecutePart1()
        {
            var tree = Input.Replace("\r\n", "\n").Split("\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(r => r.Split(" ").Select(t => t.Trim(',')).ToArray())
                .ToDictionary(r => int.Parse(r[0]), r => r.Skip(2).Select(int.Parse).ToArray());
            var visits = new HashSet<int>();
            void Visit(int p)
            {
                if (visits.Add(p))
                {
                    foreach (var c in tree[p])
                        Visit(c);
                }
            }
            Visit(0);
            return visits.Count;
        }

        public override object ExecutePart2()
        {
            var tree = Input.Replace("\r\n", "\n").Split("\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(r => r.Split(" ").Select(t => t.Trim(',')).ToArray())
                .ToDictionary(r => int.Parse(r[0]), r => r.Skip(2).Select(int.Parse).ToArray());
            var visits = new HashSet<int>();
            bool Visit(int p)
            {
                if (visits.Add(p))
                {
                    foreach (var c in tree[p])
                        Visit(c);
                    return true;
                }
                return false;
            }
            return tree.Keys.Count(Visit);
        }
    }
}
