using System.Collections.Generic;
using System.Linq;
using Tidy.AdventOfCode;

namespace YuGabe.AdventOfCode.Year2020
{
    public class Day17 : Day<Day17.ConwaySpace3D>
    {
        public interface ICoordinate<T> where T : ICoordinate<T>
        {
            public T DistanceOf(T other);
        }

        public record Coordinate3D(int X, int Y, int Z) : ICoordinate<Coordinate3D>
        {
            public Coordinate3D DistanceOf(Coordinate3D other) => new Coordinate3D(X - other.X, Y - other.Y, Z - other.Z);

            public Coordinate4D To4D() => new Coordinate4D(X, Y, Z, 0);
        }

        public record Coordinate4D(int X, int Y, int Z, int W) : Coordinate3D(X, Y, Z), ICoordinate<Coordinate4D>
        {
            public Coordinate4D DistanceOf(Coordinate4D other) => new Coordinate4D(X - other.X, Y - other.Y, Z - other.Z, W - other.W);
        }

        public record ConwaySpace4D(IDictionary<Coordinate4D, bool> Values) : ConwaySpace<Coordinate4D>(Values)
        {
            protected override Coordinate4D[] NeighborCoordinateDiffs { get; } = Enumerable.Range(-1, 3).SelectMany(x => Enumerable.Range(-1, 3).SelectMany(y => Enumerable.Range(-1, 3).SelectMany(z => Enumerable.Range(-1, 3).Select(w => new Coordinate4D(x, y, z, w))))).Where(c => (c.X, c.Y, c.Z, c.W) != (0, 0, 0, 0)).ToArray();
        }

        public record ConwaySpace3D(IDictionary<Coordinate3D, bool> Values) : ConwaySpace<Coordinate3D>(Values)
        {
            protected override Coordinate3D[] NeighborCoordinateDiffs { get; } = Enumerable.Range(-1, 3).SelectMany(x => Enumerable.Range(-1, 3).SelectMany(y => Enumerable.Range(-1, 3).Select(z => new Coordinate3D(x, y, z)))).Where(c => (c.X, c.Y, c.Z) != (0, 0, 0)).ToArray();

            public ConwaySpace4D To4D() => new ConwaySpace4D(Values.ToDictionary(kv => kv.Key.To4D(), kv => kv.Value));
        }

        public abstract record ConwaySpace<T>(IDictionary<T, bool> Values) 
            where T : ICoordinate<T>
        {
            public bool this[T key] { get => Values.TryGetValue(key, out var value) && value; set => Values[key] = value; }

            protected abstract T[] NeighborCoordinateDiffs { get; }

            public IEnumerable<(T coordinate, bool Value)> GetNeighborsOf(T key) => 
                NeighborCoordinateDiffs.Select(c => key.DistanceOf(c)).Select(c => (c, this[c]));

            public ConwaySpace<T> RunCycle(int times = 1)
            {
                foreach(var _ in Enumerable.Range(0, times))
                {
                    foreach (var (c, _) in Values.Where(kv => !kv.Value))
                        Values.Remove(c);

                    foreach (var (coordinate, active, activeNeighborsCount) in Values.Where(v => v.Value).SelectMany(c => GetNeighborsOf(c.Key).Select(n => n.coordinate).Append(c.Key).Distinct().Select(c => (c, this[c], GetNeighborsOf(c).Count(c => c.Value)))).ToList())
                    {
                        this[coordinate] = activeNeighborsCount == 3 || (activeNeighborsCount == 2 && active);
                    }
                }
                return this;
            }

            public int Solve() => RunCycle(6).Values.Count(c => c.Value);
        }

        public override ConwaySpace3D ParseInput(string rawInput) => new ConwaySpace3D(
            rawInput.Split('\n').WithIndexes().SelectMany(x => x.Element.WithIndexes().Select(y => (x: x.Index, y: y.Index, c: y.Element))).ToDictionary(e => new Coordinate3D(e.x, e.y, 0), e => e.c == '#'));

        public override object ExecutePart1() => Input.Solve();

        public override object ExecutePart2() => Input.To4D().Solve();
    }
}
