using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace YuGabe.AdventOfCode.Common;
public class Map2D<T> : IReadOnlyDictionary<(int X, int Y), Map2D<T>.Node>
{
    public sealed class Node
    {
        public Node(Map2D<T> map, (int X, int Y) key, T value) => (Map, Key, Value) = (map, key, value);

        private Map2D<T> Map { get; }
        public (int X, int Y) Key { get; }
        public int X => Key.X;
        public int Y => Key.Y;
        public T Value { get; set; }

        public Node? Up => Map.GetValueOrDefault((X, Y - 1));
        public Node? Right => Map.GetValueOrDefault((X + 1, Y));
        public Node? Down => Map.GetValueOrDefault((X, Y + 1));
        public Node? Left => Map.GetValueOrDefault((X - 1, Y));
        public IEnumerable<Node> AllUp => new DynamicEnumerable<Node>(this, c => c.Up).Skip(1);
        public IEnumerable<Node> AllRight => new DynamicEnumerable<Node>(this, c => c.Right).Skip(1);
        public IEnumerable<Node> AllDown => new DynamicEnumerable<Node>(this, c => c.Down).Skip(1);
        public IEnumerable<Node> AllLeft => new DynamicEnumerable<Node>(this, c => c.Left).Skip(1);
        public IEnumerable<IEnumerable<Node>> AllCardinalNeighbors
        {
            get
            {
                yield return AllUp;
                yield return AllRight;
                yield return AllDown;
                yield return AllLeft;
            }
        }
        public IEnumerable<Node?> Neighbors
        {
            get
            {
                yield return Up;
                yield return Right;
                yield return Down;
                yield return Left;
            }
        }
        public override string ToString() => $"[{X},{Y}]: {Value}";
    }

    public Map2D(Dictionary<(int X, int Y), T> map)
    {
        Map = map.ToDictionary(kv => kv.Key, kv => new Node(this, kv.Key, kv.Value)).AsReadOnly();
        ((MinX, MinY), (MaxX, MaxY)) = map.Keys.MinMax();
    }

    private IReadOnlyDictionary<(int X, int Y), Node> Map { get; }

    public IEnumerable<(int X, int Y)> Keys => Map.Keys;
    public IEnumerable<Map2D<T>.Node> Values => Map.Values;
    public int Count => Map.Count;

    public int MinX { get; }
    public int MinY { get; }
    public int MaxX { get; }
    public int MaxY { get; }

    public IEnumerable<Map2D<T>.Node> GetRow(int y) => Range(MinX, MaxX - MinX).Select(x => this[x, y]);
    public IEnumerable<Map2D<T>.Node> GetColumn(int x) => Range(MinY, MaxY - MinY).Select(y => this[x, y]);

    public Node this[(int X, int Y) key] => Map[key];
    public Node this[int X, int Y] => Map[(X, Y)];

    public bool ContainsKey((int X, int Y) key) => Map.ContainsKey(key);
    public bool TryGetValue((int X, int Y) key, [MaybeNullWhen(false)] out Map2D<T>.Node value) => Map.TryGetValue(key, out value);
    public IEnumerator<KeyValuePair<(int X, int Y), Map2D<T>.Node>> GetEnumerator() => Map.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Map.GetEnumerator();
}
