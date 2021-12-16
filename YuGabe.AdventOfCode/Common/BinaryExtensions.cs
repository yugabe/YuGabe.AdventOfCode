namespace YuGabe.AdventOfCode;

public static class BinaryExtensions
{
    public static byte ToByte(this IEnumerable<bool> bits) => bits.Aggregate((byte)0, (acc, c) => (byte)((acc * 2) + (c ? 1 : 0)));

    public static byte ToByte(this Span<bool> bits)
    {
        byte acc = 0;
        for (var i = 0; i < bits.Length; i++)
            acc = (byte)((acc * 2) + (bits[i] ? 1 : 0));
        return acc;
    }

    public static int ToInt(this IEnumerable<bool> bits) => bits.Aggregate(0, (acc, c) => (acc * 2) + (c ? 1 : 0));

    public static int ToInt(this Span<bool> bits)
    {
        var acc = 0;
        for (var i = 0; i < bits.Length; i++)
            acc = (acc * 2) + (bits[i] ? 1 : 0);
        return acc;
    }

    public static long ToLong(this IEnumerable<bool> bits) => bits.Aggregate(0L, (acc, c) => (acc * 2) + (c ? 1 : 0));

    public static long ToLong(this Span<bool> bits)
    {
        var acc = 0L;
        for (var i = 0; i < bits.Length; i++)
            acc = (acc * 2) + (bits[i] ? 1 : 0);
        return acc;
    }

    public static string ToBinaryString(this IEnumerable<bool> bits) => string.Join("", bits.Select(b => b ? '1' : '0'));
}
