using System;
using System.Linq;
using System.Threading;
using Tidy.AdventOfCode;

namespace YuGabe.AdventOfCode.Year2020
{
    public class Day3 : Day<string>.NewLineSplitParsed
    {
        public override object ExecutePart1() =>
            TreesInSlope(3, 1);

        public override object ExecutePart2() => new[] { (1, 1), (3, 1), (5, 1), (7, 1), (1, 2) }.Aggregate(1, (acc, e) => acc * TreesInSlope(e.Item1, e.Item2));

        private int TreesInSlope(int right, int down) =>
            Input.WithIndexes().Count(c => c.Index % down == 0 && c.Element[c.Index * right / down % c.Element.Length] == '#');

#pragma warning disable IDE0051 // Remove unused private members
        private int TreesInSlopeVisualized(int right, int down)
#pragma warning restore IDE0051 // Remove unused private members
        {
            var padding = new string('-', Input[0].Length);
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Write($"{padding} {(right, down)} {padding}");
            Console.ResetColor();
            Console.WriteLine();
            var count = 0;
            return Input.WithIndexes().Count(c =>
            {
                var result = c.Index % down == 0 && c.Element[c.Index * right / down % c.Element.Length] == '#';
                if (result)
                    count++;
                var dontSkipRow = c.Index % down == 0;
                var itemIndex = c.Index * right / down % c.Element.Length;
                var item = c.Element[itemIndex];
                var hit = item == '#';

                if ((dontSkipRow && item == '#') != result)
                    throw new Exception();

                Console.ResetColor();
                Console.Write($"{c.Element} | {(dontSkipRow ? $"< | {c.Index,-4} | {item} : {itemIndex,-4} ({count})" : "S")}");
                if (dontSkipRow)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(itemIndex, Console.GetCursorPosition().Top);
                    Console.Write(item);
                    Console.Beep(hit ? 900 + (count * 10) : 100, 15);
                    Thread.Sleep(15);
                    Console.SetCursorPosition(0, Console.GetCursorPosition().Top + 1);
                }
                else
                {
                    Console.WriteLine();
                    Thread.Sleep(30);
                }
                Console.ResetColor();

                return result;
            });
        }
    }
}
