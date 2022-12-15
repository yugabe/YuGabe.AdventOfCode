using System.Numerics;

namespace YuGabe.AdventOfCode;

public record struct Point2D<T>(T X, T Y) where T : INumber<T>
{
    public static implicit operator (T X, T Y)(Point2D<T> value) => (value.X, value.Y);
    public static implicit operator Point2D<T>((T X, T Y) value) => new(value.X, value.Y);

    public Point2D<T> Up => new(X, Y - T.One);
    public Point2D<T> Right => new(X + T.One, Y);
    public Point2D<T> Down => new(X, Y + T.One);
    public Point2D<T> Left => new(X - T.One, Y);

    public IEnumerable<Point2D<T>> LineTo(Point2D<T> other)
    {
        if (X == other.X)
        {
            for (var y = Y; Y <= other.Y ? y <= other.Y : y >= other.Y; y += Y < other.Y ? T.One : -T.One)
                yield return (X, y);
        }
        else if (Y == other.Y)
            for (var x = X; X <= other.X ? x <= other.X : x >= other.X; x += X < other.X ? T.One : -T.One)
                yield return (x, Y);
        else
            throw new InvalidOperationException("Lines can only go straight on either X or Y axis.");
    }

    public override string ToString() => $"({X},{Y})";
}
public record struct Point3D<T>(T X, T Y, T Z)
{
    public static implicit operator (T X, T Y, T Z)(Point3D<T> value) => (value.X, value.Y, value.Z);
    public static implicit operator Point3D<T>((T X, T Y, T Z) value) => new(value.X, value.Y, value.Z);
}
public record struct Point4D<T>(T X, T Y, T Z, T W)
{
    public static implicit operator (T X, T Y, T Z, T W)(Point4D<T> value) => (value.X, value.Y, value.Z, value.W);
    public static implicit operator Point4D<T>((T X, T Y, T Z, T W) value) => new(value.X, value.Y, value.Z, value.W);
}
