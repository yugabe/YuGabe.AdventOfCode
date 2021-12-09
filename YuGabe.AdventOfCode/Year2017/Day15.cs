using System.Numerics;

namespace YuGabe.AdventOfCode.Year2017
{
    public class Day15 : Day
    {
        public override object ExecutePart1()
        {
            var generatorValues = Input.Replace("\r\n", "\n").Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(r => new BigInteger(int.Parse(r.Split(" ").Last()))).ToArray();
            var g1 = generatorValues[0];
            var g2 = generatorValues[1];
            var factors = new[] { 16807, 48271 };
            var f1 = factors[0];
            var f2 = factors[1];
            var matches = 0;

            for (var i = 0; i < 40_000_000; i++)
            {
                g1 = (g1 * f1) % 2147483647;
                g2 = (g2 * f2) % 2147483647;
                if ((g1 & 0b1111_1111_1111_1111) == (g2 & 0b1111_1111_1111_1111))
                    matches++;
                //generatorValues = generatorValues.Select((g, x) => (g * factors[x]) % 2147483647).ToArray();
                //if (generatorValues.Select(v => v & 0b1111_1111_1111_1111).Distinct().Count() == 1)
                //    matches++;
            }
            return matches;
        }

        public override object ExecutePart2()
        {
            var generatorValues = Input.Replace("\r\n", "\n").Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(r => new BigInteger(int.Parse(r.Split(" ").Last()))).ToArray();
            var g1 = generatorValues[0];
            var g2 = generatorValues[1];
            var factors = new[] { 16807, 48271 };
            var f1 = factors[0];
            var f2 = factors[1];
            var matches = 0;

            for (var i = 0; i < 5_000_000; i++)
            {
                do
                {
                    g1 = (g1 * f1) % 2147483647;
                }
                while ((g1 % 4) != 0);
                do
                {
                    g2 = (g2 * f2) % 2147483647;
                }
                while ((g2 % 8) != 0);
                if ((g1 & 0b1111_1111_1111_1111) == (g2 & 0b1111_1111_1111_1111))
                    matches++;
            }
            return matches;
        }
    }
}
