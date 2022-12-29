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

    public static Point2D<T> operator +(Point2D<T> left, Point2D<T> right) => new(left.X + right.X, left.Y + right.Y);
    public static Point2D<T> operator -(Point2D<T> left, Point2D<T> right) => new(left.X - right.X, left.Y - right.Y);
    public static Point2D<T> operator *(Point2D<T> point, T value) => new(point.X * value, point.Y * value);

    public override string ToString() => $"({X},{Y})";

    public IEnumerable<Point2D<T>> CardinalNeighbors
    {
        get
        {
            yield return Up;
            yield return Right;
            yield return Down;
            yield return Left;
        }
    }
}
public record struct Point3D<T>(T X, T Y, T Z) where T : INumber<T>
{
    public static implicit operator (T X, T Y, T Z)(Point3D<T> value) => (value.X, value.Y, value.Z);
    public static implicit operator Point3D<T>((T X, T Y, T Z) value) => new(value.X, value.Y, value.Z);

    public static Point3D<T> operator +(Point3D<T> left, Point3D<T> right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    public static Point3D<T> operator -(Point3D<T> left, Point3D<T> right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
    public static Point3D<T> operator *(Point3D<T> point, T value) => new(point.X * value, point.Y * value, point.Z * value);

    private static Point3D<T>[] NeighborCoordinates { get; } = new Point3D<T>[]
    {
        new(-T.One, T.Zero, T.Zero), new(T.One, T.Zero, T.Zero),
        new(T.Zero, -T.One, T.Zero), new(T.Zero, T.One, T.Zero),
        new(T.Zero, T.Zero, -T.One), new(T.Zero, T.Zero, T.One),
    };
    public IEnumerable<Point3D<T>> Neighbors
    {
        get
        {
            var self = this;
            return NeighborCoordinates.Select(n => self + n);
        }
    }
}
public record struct Point4D<T>(T X, T Y, T Z, T W)
{
    public static implicit operator (T X, T Y, T Z, T W)(Point4D<T> value) => (value.X, value.Y, value.Z, value.W);
    public static implicit operator Point4D<T>((T X, T Y, T Z, T W) value) => new(value.X, value.Y, value.Z, value.W);
}
