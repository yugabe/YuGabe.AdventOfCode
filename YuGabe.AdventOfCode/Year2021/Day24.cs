namespace YuGabe.AdventOfCode.Year2021;

using static YuGabe.AdventOfCode.ConsoleUtilities.AdvancedConsole;

public partial class Day24 : Day<Day24.Instruction[]>
{
    public static bool ValidateNativeV1(string modelNumber)
    {
        long w = 0, x = 0, y = 0, z = 0;
        foreach (var (number, index) in modelNumber.Select(c => long.Parse(c.ToString())).WithIndexes())
        {
            //inp w
            w = number;
            //mul x 0
            x *= 0;
            //add x z
            x += z;
            //mod x 26
            x %= 26;
            //div z [1,1,1,26,1,1,26,1,26,1,26,26,26,26]
            z /= (index is 3 or 6 or 8 or 10 or 11 or 12 or 13) ? 26 : 1;
            //add x [11,12,10,-8,15,15,-11,10,-3,15,-3,-1,-10,-16]
            x += index switch
            {
                0 => 11,
                1 => 12,
                2 => 10,
                3 => -8,
                4 => 15,
                5 => 15,
                6 => -11,
                7 => 10,
                8 => -3,
                9 => 15,
                10 => -3,
                11 => -1,
                12 => -10,
                13 => 16,
                _ => throw new InvalidOperationException()
            };
            //eql x w
            x = x == w ? 1 : 0;
            //eql x 0
            x = x == 0 ? 1 : 0;
            //mul y 0
            y *= 0;
            //add y 25
            y += 25;
            //mul y x
            y *= x;
            //add y 1
            y += 1;
            //mul z y
            z *= y;
            //mul y 0
            y *= 0;
            //add y w
            y += w;
            //add y [8,8,12,10,2,8,4,9,10,3,7,7,2,2]
            y += index switch
            {
                0 => 8,
                1 => 8,
                2 => 12,
                3 => 10,
                4 => 2,
                5 => 8,
                6 => 4,
                7 => 9,
                8 => 10,
                9 => 3,
                10 => 7,
                11 => 7,
                12 => 2,
                13 => 2,
                _ => throw new InvalidOperationException()
            };
            //mul y x
            y *= x;
            //add z y
            z += y;
        }
        return z == 0;
    }

    public static bool ValidateNativeV2(long modelNumber)
    {
        var z = 0L;
        var n1 = modelNumber / 10000000000000 % 10;
        var n2 = modelNumber / 1000000000000 % 10;
        var n3 = modelNumber / 100000000000 % 10;
        var n4 = modelNumber / 10000000000 % 10;
        var n5 = modelNumber / 1000000000 % 10;
        var n6 = modelNumber / 100000000 % 10;
        var n7 = modelNumber / 10000000 % 10;
        var n8 = modelNumber / 1000000 % 10;
        var n9 = modelNumber / 100000 % 10;
        var n10 = modelNumber / 10000 % 10;
        var n11 = modelNumber / 1000 % 10;
        var n12 = modelNumber / 100 % 10;
        var n13 = modelNumber / 10 % 10;
        var n14 = modelNumber % 10;
        if (n1 == 0 || n2 == 0 || n3 == 0 || n4 == 0 || n5 == 0 || n6 == 0 || n7 == 0 || n8 == 0 || n9 == 0 || n10 == 0 || n11 == 0 || n12 == 0 || n13 == 0 || n14 == 0)
            return false;
        z = (z * 26) + n1 + 8;
        z = (z * 26) + n2 + 8;
        z = (z * 26) + n3 + 12;
        z = ((z % 26) == n4 + 8) ? z / 26 : (z / 26 * 26) + n4 + 10;
        z = (z * 26) + n5 + 2;
        z = (z * 26) + n6 + 8;
        z = ((z % 26) == n7 + 11) ? z / 26 : (z / 26 * 26) + n7 + 4;
        z = (z * 26) + n8 + 9;
        z = ((z % 26) == n9 + 3) ? z / 26 : (z / 26 * 26) + n9 + 10;
        z = (z * 26) + n10 + 3;
        z = ((z % 26) == n11 + 3) ? z / 26 : (z / 26 * 26) + n11 + 7;
        z = ((z % 26) == n12 + 1) ? z / 26 : (z / 26 * 26) + n12 + 7;
        z = ((z % 26) == n13 + 10) ? z / 26 : (z / 26 * 26) + n13 + 2;
        z = ((z % 26) == n14 + 16) ? z / 26 : (z / 26 * 26) + n14 + 2;
        return z == 0;
    }

    public static string GetInstructionSimilarity(string input)
    {
        var lines = input.GetLinesToTuple3();

        var sb = new System.Text.StringBuilder();
        for (var l = 0; l < 18; l++)
        {
            var (i1, i2, i3) = (new List<string>(), new List<string>(), new List<string>());

            for (var it = 0; it < 14; it++)
            {
                var (t1, t2, t3) = lines[(it * 18) + l];
                i1.Add(t1);
                i2.Add(t2!);
                i3.Add(t3!);
            }

            sb.AppendLine(string.Join(" ", new[] { i1, i2, i3 }.Select(i => i1.Distinct().Count() == 1 ? i1.First() : $"[{string.Join(",", i1)}]")));
        }
        return sb.ToString();
    }

    public static long Execute(Func<IEnumerable<int>, long> selector)
    {
        var outerMatches = Range(0, 14).ToDictionary(e => e, e => new HashSet<long>());
        var possibleNumbersAtIndex = Range(0, 14).ToDictionary(e => e, e => new HashSet<int>());
        outerMatches[13].Add(0);
        for (var outerIndex = 13; outerIndex >= 0; outerIndex--)
        {
            var possibleZsForIndex = Range(-1, 15).ToDictionary(i => i, i => new HashSet<long>());
            possibleZsForIndex[-1].Add(0);
            for (var index = 0; index <= outerIndex; index++)
            {
                Console.WriteLine(index);
                foreach (var zLocal in possibleZsForIndex[index - 1].ToList())
                {
                    for (var number = 9; number >= 1; number--)
                    {
                        var z = zLocal;
                        z = index switch
                        {
                            0 => (z * 26) + number + 8,
                            1 => (z * 26) + number + 8,
                            2 => (z * 26) + number + 12,
                            3 => ((z % 26) == number + 8) ? z / 26 : (z / 26 * 26) + number + 10,
                            4 => (z * 26) + number + 2,
                            5 => (z * 26) + number + 8,
                            6 => ((z % 26) == number + 11) ? z / 26 : (z / 26 * 26) + number + 4,
                            7 => (z * 26) + number + 9,
                            8 => ((z % 26) == number + 3) ? z / 26 : (z / 26 * 26) + number + 10,
                            9 => (z * 26) + number + 3,
                            10 => ((z % 26) == number + 3) ? z / 26 : (z / 26 * 26) + number + 7,
                            11 => ((z % 26) == number + 1) ? z / 26 : (z / 26 * 26) + number + 7,
                            12 => ((z % 26) == number + 10) ? z / 26 : (z / 26 * 26) + number + 2,
                            13 => ((z % 26) == number + 16) ? z / 26 : (z / 26 * 26) + number + 2,
                            _ => throw new InvalidOperationException()
                        };
                        possibleZsForIndex[index].Add(z);
                        if (index == outerIndex)
                        {
                            if (outerMatches[index].Contains(z))
                            {
                                possibleNumbersAtIndex[index].Add(number);
                                if (index != 0)
                                    outerMatches[index - 1].Add(zLocal);
                            }
                            if (index == 0 && zLocal == 0)
                                return possibleNumbersAtIndex.OrderBy(kv => kv.Key).Where(kv => kv.Value.Any()).Select(kv => selector(kv.Value)).Aggregate(1L, (acc, c) => (acc * 10) + c);
                        }
                    }
                }
            }
        }
        throw new InvalidOperationException();
    }

    public override object ExecutePart1() => Execute(e => e.Max());
    public override object ExecutePart2() => Execute(e => e.Min());
}
