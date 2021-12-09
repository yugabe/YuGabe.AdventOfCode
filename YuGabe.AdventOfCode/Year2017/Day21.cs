using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tidy.AdventOfCode;

namespace YuGabe.AdventOfCode.Year2017
{
    public class Day21 : Day<Dictionary<string, Day21.Rule>>
    {
        public override Dictionary<string, Rule> ParseInput(string input)
        {
            return input.Trim().Split("\n").Select(r => r.Split(" => ")).Select(Rule.Parse).ToDictionary(r => r.Source.Sanitized);
        }

        public override object ExecutePart1()
        {
            foreach (var rule in Input.ToList())
            {
                for (var i = 0; i < 8; i++)
                {
                    var rotated = rule.Value.Source.Rotate(i);
                    if (!Input.ContainsKey(rotated))
                        Input[rotated] = rule.Value;
                }
            }
            var state = ".#...####";
            for (var i = 0; i < 5; i++)
            {
                var size = (int)Math.Sqrt(state.Length);
                var targets = new List<string>();
                for (var m = 2; m <= 3; m++)
                {
                    if (size % m == 0)
                    {
                        for (var bx = 0; bx < size / m; bx++)
                            for (var by = 0; by < size / m; by++)
                            {
                                var chars = new List<char>();
                                for (var rbx = 0; rbx < m; rbx++)
                                    for (var rby = 0; rby < m; rby++)
                                        chars.Add(state[(bx * m + rbx) * size + (by * m + rby)]);
                                var box = new string(chars.ToArray());
                                targets.Add(Input[box].Target.Sanitized);
                            }
                        state = "";

                        foreach (var t in targets.Select((t, x) => (t, x)).GroupBy(e => e.x / m))
                        {
                            for (var rm = 0; rm < m + 1; rm++)
                            {
                                foreach (var e in t)
                                {
                                    state += e.t.Substring(rm * (m + 1), (m + 1));
                                }
                            }
                        }
                        break;
                    }
                }
            }

            return state.Count(c => c == '#');
        }

        public override object ExecutePart2()
        {
            foreach (var rule in Input.ToList())
                for (var i = 0; i < 8; i++)
                    Input[rule.Value.Source.Rotate(i)] = rule.Value;

            var state = ".#...####";
            for (var i = 0; i < 18; i++)
            {
                var size = (int)Math.Sqrt(state.Length);
                var targets = new List<string>();
                for (var m = 2; m <= 3; m++)
                {
                    if (size % m == 0)
                    {
                        for (var bx = 0; bx < size / m; bx++)
                            for (var by = 0; by < size / m; by++)
                            {
                                var chars = new List<char>();
                                for (var rbx = 0; rbx < m; rbx++)
                                    for (var rby = 0; rby < m; rby++)
                                        chars.Add(state[(bx * m + rbx) * size + (by * m + rby)]);
                                var box = new string(chars.ToArray());
                                targets.Add(Input[box].Target.Sanitized);
                            }

                        var sb = new StringBuilder();

                        var targetLength = targets.First().Length;

                        IEnumerable<string> currentTargets = targets;
                        while (currentTargets.Any())
                        {
                            for (var sp = 0; sp < targetLength; sp += m + 1)
                                foreach (var t in currentTargets.Take(size / m))
                                {
                                    sb.Append(t.Substring(sp, m + 1));
                                }
                            currentTargets = currentTargets.Skip(size / m);
                        }

                        state = sb.ToString();
                        break;
                    }
                }
                //Console.WriteLine($"{i}:");
                //size = (int)Math.Sqrt(state.Length);
                //for (var sx = 0; sx < Math.Pow(size, 2); sx += size)
                //    Console.WriteLine(state.Substring(sx, size));
            }

            return state.Count(c => c == '#'); // h:2805334, h:2804334 l: 2705334
        }

        public struct Pattern
        {
            public string Raw;
            public string Sanitized;
            public int Size;

            public static Pattern Parse(string pattern)
            {
                var sanitized = pattern.Replace("/", "");
                return new Pattern
                {
                    Raw = pattern,
                    Sanitized = sanitized,
                    Size = (int)Math.Sqrt(sanitized.Length)
                };
            }
            public string Rotate(int ticks)
            {
                string Reorder(string text, params int[] indexes) => new string(indexes.Select(i => text[i]).ToArray());
                if (Size == 2)
                {
                    switch (ticks)
                    {
                        case 0:
                            return Sanitized;
                        case 1:
                            return Reorder(Sanitized, 2, 0, 3, 1);
                        case 2:
                            return Reorder(Sanitized, 3, 2, 1, 0);
                        case 3:
                            return Reorder(Sanitized, 1, 3, 0, 2);
                        case 4:
                            return Reorder(Sanitized, 1, 0, 3, 2);
                        case 5:
                            return Reorder(Sanitized, 0, 2, 1, 3);
                        case 6:
                            return Reorder(Sanitized, 2, 3, 0, 1);
                        case 7:
                            return Reorder(Sanitized, 3, 1, 2, 0);
                        default:
                            break;
                    }
                }
                else if (Size == 3)
                {
                    switch (ticks)
                    {
                        case 0:
                            return Sanitized;
                        case 1:
                            return Reorder(Sanitized, 6, 3, 0, 7, 4, 1, 8, 5, 2);
                        case 2:
                            return Reorder(Sanitized, 8, 7, 6, 5, 4, 3, 2, 1, 0);
                        case 3:
                            return Reorder(Sanitized, 2, 5, 8, 1, 4, 7, 0, 3, 6);
                        case 4:
                            return Reorder(Sanitized, 2, 1, 0, 5, 4, 3, 8, 7, 6);
                        case 5:
                            return Reorder(Sanitized, 0, 3, 6, 1, 4, 7, 2, 5, 8);
                        case 6:
                            return Reorder(Sanitized, 6, 7, 8, 3, 4, 5, 0, 1, 2);
                        case 7:
                            return Reorder(Sanitized, 8, 5, 2, 7, 4, 1, 6, 3, 0);
                        default:
                            break;
                    }
                }
                throw new NotImplementedException();
            }

            public override string ToString() => Sanitized;
            public string ToString(bool pretty) => pretty ? Raw.Replace("/", "\n") : ToString();
        }

        public struct Rule
        {
            public int Size;
            public Pattern Source;
            public Pattern Target;

            public static Rule Parse(string[] tokens)
            {
                var rule = new Rule
                {
                    Source = Pattern.Parse(tokens[0]),
                    Target = Pattern.Parse(tokens[1])
                };
                rule.Size = rule.Source.Size;
                return rule;
            }

            public override string ToString() => $"{Source} => {Target}";
            public string ToString(bool pretty) => $"{Source.ToString(pretty)}\n | \n v \n{Target.ToString(pretty)}";
        }
    }

}
