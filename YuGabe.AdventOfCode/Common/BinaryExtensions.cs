namespace YuGabe.AdventOfCode;

public static class BinaryExtensions
{
    public static byte ToByte(this IEnumerable<bool> bits)
        => bits.Aggregate((byte)0, (acc, c) => (byte)(acc * 2 + (c ? 1 : 0)));

    public static int ToInt(this IEnumerable<bool> bits)
        => bits.Aggregate(0, (acc, c) => acc * 2 + (c ? 1 : 0));

    public static long ToLong(this IEnumerable<bool> bits)
        => bits.Aggregate(0L, (acc, c) => acc * 2 + (c ? 1 : 0));
}
