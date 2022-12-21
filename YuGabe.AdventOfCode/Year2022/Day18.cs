namespace YuGabe.AdventOfCode.Year2022;
public class Day18 : Day<HashSet<Point3D<int>>>
{
    public override HashSet<Point3D<int>> ParseInput(string rawInput) => rawInput.Split("\n").Select(l => l.Split(',').Select(int.Parse).ToArray()).Select(b => new Point3D<int>(b[0], b[1], b[2])).ToHashSet();

    public override object ExecutePart1() => Input.Sum(b => b.Neighbors.Count(n => !Input.Contains(n)));

    public override object ExecutePart2()
    {
        var ((xMin, xMax), (yMin, yMax), (zMin, zMax)) = (Input.MinMax(p => p.X), Input.MinMax(p => p.Y), Input.MinMax(p => p.Z));

        var outsideAir = new HashSet<Point3D<int>>() { (xMin - 1, yMin - 1, zMin - 1) };
        foreach (var air in Range(xMin - 1, xMax - xMin + 1).SelectMany(x => Range(yMin - 1, yMax - yMin + 1).SelectMany(y => Range(zMin - 1, zMax - zMin + 1).Select(z => new Point3D<int>(x, y, z)))).Where(current => !Input.Contains(current) && current.Neighbors.Any(outsideAir.Contains)))
            outsideAir.Add(air);

        var airQueue = new Queue<Point3D<int>>(outsideAir.SelectMany(air => air.Neighbors.Where(neighbor => !Input.Contains(neighbor) && !outsideAir.Contains(neighbor))));
        while (airQueue.TryDequeue(out var p))
        {
            outsideAir.Add(p);
            airQueue.Enqueue(p.Neighbors.Where(n => n.X >= xMin - 1 && n.X <= xMax + 1 && n.Y >= yMin - 1 && n.Y <= yMax + 1 && n.Z >= zMin - 1 && n.Z <= zMax + 1 && !Input.Contains(n) && !outsideAir.Contains(n)));
        }

        return Input.Sum(b => b.Neighbors.Count(outsideAir.Contains));
    }
}
