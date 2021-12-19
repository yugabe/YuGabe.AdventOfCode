namespace YuGabe.AdventOfCode.Year2021;

using System.Numerics;

public class Day19 : Day<Day19.Scanner[]>
{
    public record Scanner(int Number, Vector3[] RawCoordinates)
    {
        public bool IsFixed { get; private set; }
        public Vector3? Position { get; private set; }
        public Quaternion? Rotation { get; private set; }
        public void Fix(Vector3 position, Quaternion rotation) => (IsFixed, Position, Rotation) = (true, position, rotation);
    }
    public override Scanner[] ParseInput(string rawInput) => rawInput.Split("\n\n", SSO.RemoveEmptyEntries | SSO.TrimEntries).Select(b => b.SplitAtNewLines().FeedTo(l => new Scanner(int.Parse(l[0].Split(" ")[2]), l[1..].Select(c => c.SplitToTuple3(",").FeedTo(t => new Vector3(int.Parse(t.token1), int.Parse(t.token2!), int.Parse(t.token3!)))).ToArray()))).ToArray();

    public static Quaternion[] All90DegreeRotations { get; } = new (int RotX, int RotY, int RotZ)[] { (0, 0, 0), (0, 0, 1), (0, 0, 2), (0, 0, 3), (0, 1, 0), (0, 1, 1), (0, 1, 2), (0, 1, 3), (0, 2, 0), (0, 2, 1), (0, 2, 2), (0, 2, 3), (0, 3, 0), (0, 3, 1), (0, 3, 2), (0, 3, 3), (1, 0, 0), (1, 0, 1), (1, 0, 2), (1, 0, 3), (1, 2, 0), (1, 2, 1), (1, 2, 2), (1, 2, 3) }.Select(r => Quaternion.CreateFromAxisAngle(Vector3.UnitX, (float)(0.5 * Math.PI * r.RotX)) * Quaternion.CreateFromAxisAngle(Vector3.UnitY, (float)(0.5 * Math.PI * r.RotY)) * Quaternion.CreateFromAxisAngle(Vector3.UnitZ, (float)(0.5 * Math.PI * r.RotZ))).ToArray();

    public static HashSet<Vector3> MatchWorldCoordinates(Scanner[] scanners)
    {
        scanners[0].Fix(Vector3.Zero, Quaternion.Identity);
        var knownWorldCoordinates = scanners[0].RawCoordinates.ToHashSet();

        while (scanners.Any(s => !s.IsFixed))
        {
            foreach (var ((scanner, rotation, rotations), offset) in scanners.Where(s => !s.IsFixed)
                .SelectMany(s => All90DegreeRotations.Select(r => (s, r, rotations: s.RawCoordinates.Select(c => Vector3.Transform(c, r)).Select(v => v with { X = (float)Math.Round(v.X), Y = (float)Math.Round(v.Y), Z = (float)Math.Round(v.Z) }))).ToList())
                .SelectMany(srr => knownWorldCoordinates.SelectMany(k => srr.rotations.Select(r => (srr, offset: k - r)))))
            {
                if (rotations.Where(r => knownWorldCoordinates.Contains(r + offset)).Skip(11).Any())
                {
                    scanner.Fix(offset, rotation);
                    foreach (var r in rotations)
                        knownWorldCoordinates.Add(offset + r);
                    break;
                }
            }
        }
        return knownWorldCoordinates;
    }

    public override object ExecutePart1() => MatchWorldCoordinates(Input).Count;
    public override object ExecutePart2() => MatchWorldCoordinates(Input).FeedTo(_ => Input.Select(i => i.Position!.Value).CartesianProduct(Input.Select(i => i.Position!.Value)).Max(v => Math.Abs(v.Left.X - v.Right.X) + Math.Abs(v.Left.Y - v.Right.Y) + Math.Abs(v.Left.Z - v.Right.Z)));
}
