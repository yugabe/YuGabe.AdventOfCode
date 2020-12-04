using System;
using System.Linq;
using System.Text;
using Tidy.AdventOfCode;

namespace YuGabe.AdventOfCode.Year2017
{
#pragma warning disable CS0162 // Unreachable code detected
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable IDE0059 // Unnecessary assignment of a value
#pragma warning disable CS0164 // This label has not been referenced
#pragma warning disable CA1822 // Mark members as static
    public class Day23 : Day
    {
        public override object ExecutePart1()
        {
            var instructions = Input.Split("\n").Select(r => r.Split(' ')).ToArray();
            long muls = 0;
            var registers = instructions.SelectMany(i => i).Where(i => i.Length == 1 && !long.TryParse(i, out _)).Distinct().ToDictionary(i => i, i => (long)0);
            long Value(string value) => long.TryParse(value, out var val) ? val : registers[value];
            for (long i = 0; i < instructions.Length && i >= 0;)
            {
                var inst = instructions[i];
                switch (inst[0])
                {
                    case "set":
                        registers[inst[1]] = Value(inst[2]);
                        break;
                    case "sub":
                        registers[inst[1]] -= Value(inst[2]);
                        break;
                    case "mul":
                        registers[inst[1]] *= Value(inst[2]);
                        muls++;
                        break;
                    case "jnz":
                        if (Value(inst[1]) != 0)
                        {
                            i += Value(inst[2]);
                            continue;
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }
                i++;
            }
            return muls;
        }

        public override object ExecutePart2()
        {
            return Enumerable.Range(0, 1001).Select(n => 107_900 + 17 * n).Count(n => Enumerable.Range(2, n - 2).Any(e => n % e == 0 && n != e));

            // oh my god this was... wow.
            var total = 0;
            for (var bi = 107_900; bi <= 124_900; bi += 17)
            {
                for (var o = 2; o < bi; o++)
                {
                    if (bi % o == 0)
                    {
                        total++;
                        o = 107_900;
                    }
                }
            }
            return total;

            return Optimized42();

            var instructions = Input.Split("\n").Select(r => r.Split(' ')).ToArray();
            var registers = instructions.SelectMany(i => i).Where(i => i.Length == 1 && !long.TryParse(i, out _)).Distinct().ToDictionary(i => i, i => (long)0);
            registers["a"] = 1;
            registers["b"] = 107900;
            registers["c"] = 124900;
            registers["d"] = 3;
            registers["e"] = 58768;
            registers["f"] = 0;
            registers["g"] = -49132;
            registers["h"] = 0;

            long steps = 1_000_000;
            long Value(string value) => long.TryParse(value, out var val) ? val : registers[value];
            for (long i = 0; i < instructions.Length && i >= 0;)
            {
                var inst = instructions[i];
                switch (inst[0])
                {
                    case "set":
                        registers[inst[1]] = Value(inst[2]);
                        break;
                    case "sub":
                        registers[inst[1]] -= Value(inst[2]);
                        break;
                    case "mul":
                        registers[inst[1]] *= Value(inst[2]);
                        break;
                    case "jnz":
                        if (Value(inst[1]) != 0)
                        {
                            i += Value(inst[2]);
                            continue;
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }
                i++;
                Console.WriteLine($"step {steps} @ {i}\n{String.Join(" ", registers.Keys.OrderBy(k => k).Select(s => s.PadRight(7)))}\n{String.Join(" ", registers.OrderBy(k => k.Key).Select(s => s.Value.ToString().PadRight(7)))}\n");
                if (++steps % 10000 == 0)
                    Console.ReadLine();
            }
            return registers["h"];
        }

        public void ReverseEngineered()
        {
            long a = 1, b = 0, c = 0, d = 0, e = 0, f = 0, g = 0, h = 0;

        l1: { b = 79; }
        l2: { c = b; }
        l3: { if (a != 0) goto l5; }
        l4: { goto l9; }
        l5: { b *= 100; }
        l6: { b += 100_000; }
        l7: { c = b; }
        l8: { c += 17000; }
        l9: { f = 1; }
        l10: { d = 2; }
        l11: { e = 2; }
        l12: { g = d; }
        l13: { g *= e; }
        l14: { g -= b; }
        l15: { if (g != 0) goto l17; }
        l16: { f = 0; }
        l17: { e++; }
        l18: { g = e; }
        l19: { g -= b; }
        l20: { if (g != 0) goto l12; }
        l21: { d++; }
        l22: { g = d; }
        l23: { g -= b; }
        l24: { if (g != 0) goto l11; }
        l25: { if (f != 0) goto l27; }
        l26: { h++; }
        l27: { g = b; }
        l28: { g -= c; }
        l29: { if (g != 0) goto l31; }
        l30: { goto l33; }
        l31: { b += 17; }
        l32: { goto l9; }
        l33: { Console.WriteLine(h); }
        }

        public long Optimized()
        {
            long b = 0, c = 0, d = 0, e = 0, f = 0, g = 0, h = 0;

            b = 107_900;
            c = 124_000;

            goto l9;

        l9:
            {
                f = 1;
                d = 2;
                goto l11;
            }
        l11:
            {
                e = 2; goto l12;
            }
        l12:
            {
                g = d * e - b;
                if (g == 0) f = 0;
                e++;
                g = e - b;
                if (g != 0) goto l12;
                d++;
                g = d - b;
                if (g != 0) goto l11;
                if (f == 0) h++;
                g = b - c;
                if (g == 0)
                    return h;
                b += 17;
                goto l9;
            }
        }

        public long Optimized2()
        {
            long b = 107_900, d = 2, h = 0;

            while (true)
            {
                d = b;
                if ((d - 1) * (b - 1) == b) h++;
                if (b == 124_000)
                    return h;

                b += 17;
            }
            throw new NotImplementedException();
        }

        public long Optimized3()
        {
            long b = 107_900, c = 124_900, d = 0, e = 0, f = 0, g = 0, h = 0;

        l9:
            {
                f = 1;
                d = 2;
                goto l11;
            }
        l11: { e = 2; goto l12; }
        l12:
            {
                g = d;
                g *= e;
                g -= b;
                if (g == 0) f = 0;
                e++;
                g = e;
                g -= b;
                if (g != 0) goto l12;
                d++;
                g = d;
                g -= b;
                if (g != 0) goto l11;
                if (f == 0) h++;
                g = b;
                g -= c;
                if (g == 0)
                    return h;
                b += 17;
                goto l9;
            }
        }

        public long Optimized4()
        {
            long b = 107_900, c = 124_900, d = 2, e = 2, f = 1, g = 0, h = 0;

            goto l12;
        l12:
            {
                g = d;
                g *= e;
                g -= b;
                if (g == 0)
                    f = 0;
                e++;
                g = e;
                g -= b;
                if (g != 0)
                    goto l12;
                d++;
                g = d;
                g -= b;
                if (g != 0)
                {
                    e = 2;
                    goto l12;
                }
                if (f == 0)
                    h++;
                g = b;
                g -= c;
                if (g == 0)
                    return h;
                b += 17;
                f = 1;
                d = 2;
                e = 2;
                goto l12;
            }
        }

        public long Optimized42()
        {
            long b = 107_900, c = 124_900, d = 2, e = 2, f = 1, h = 0;
            var sb = new StringBuilder();
            var counter = 0;
            while (true)
            {
                if (counter % 10000 > 9000)
                    sb.Append($"\n------------------{counter.ToString().PadRight(10, '-')}---------------------\n|   b   |   c   |   d   |   e   |   f   |   h   |\n|{String.Join("|", new[] { b, c, d, e, f, h }.Select(n => n.ToString().PadRight(7)))}|\n-------------------------------------------------\n");

                if (counter % 10000 == 0)
                {
                    Console.WriteLine(sb);
                    sb.Clear();
                }
                if (d * e == b)
                    f = 0;
                if (f == 0)
                    e = b;
                else { e++; counter++; }
                if (e == b)
                {
                    if (++d == b)
                    {
                        counter++;
                        if (f == 0)
                            h++;
                        if (b == c)
                            return h;
                        b += 17;
                        f = 1;
                        d = 2;
                    }
                    e = 2;
                }
            }
        }
    }
}