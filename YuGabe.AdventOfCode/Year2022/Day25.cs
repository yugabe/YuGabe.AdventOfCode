namespace YuGabe.AdventOfCode.Year2022;
public class Day25 : Day.NewLineSplitParsed<string>
{
    public override object ExecutePart1() => LongToSnafu(Input.Sum(SnafuToLong));

    public static long SnafuToLong(string snafu)
    {
        var result = 0L;
        var rolling = 1L;
        for (var i = snafu.Length - 1; i >= 0; i--)
        {
            result += rolling * snafu[i] switch
            {
                '=' => -2,
                '-' => -1,
                '0' => 0,
                '1' => 1,
                '2' => 2,
                _ => throw new InvalidOperationException()
            };
            rolling *= 5;
        }
        return result;
    }

    public static string LongToSnafu(long number)
    {
        var chars = new List<char>();
        var rem = false;
        while (number > 0)
        {
            number += rem ? 1 : 0;
            (var c, rem) = (number % 5) switch
            {
                0 => ('0', false),
                1 => ('1', false),
                2 => ('2', false),
                3 => ('=', true),
                4 => ('-', true),
                _ => throw new InvalidOperationException()
            };

            chars.Add(c);
            number /= 5;
        }
        if (rem)
            chars.Add('1');
        chars.Reverse();
        return new(chars.ToArray());
    }
}
