using System.Linq;
using Tidy.AdventOfCode;

namespace YuGabe.AdventOfCode.Year2020
{
    public class Day2 : Day<Day2.Password>.ForMany<Day2.Password>
    {
        public override object ExecutePart1() => Input.Count(p => p.IsValid);

        public override object ExecutePart2() => Input.Count(p => p.IsValidPart2);

        public class Password : IMultipleParser<Password>
        {
            public int From { get; init; }
            public int To { get; init; }
            public char Letter { get; init; }
            public string Text { get; init; } = null!;

            public Password[] ParseMany(string rawInput) => rawInput.Split('\n').Select(r => new Password
            {
                From = int.Parse(r.Split('-')[0]),
                To = int.Parse(r.Split('-')[1].Split(' ')[0]),
                Letter = r.Split(':')[0][^1],
                Text = r.Split(' ')[2]
            }).ToArray();

            public bool IsValid
            {
                get
                {
                    var count = Text.Count(l => l == Letter);
                    return count >= From && count <= To;
                }
            }

            public bool IsValidPart2 => Text[From - 1] == Letter ^ Text[To - 1] == Letter;
        }
    }
}
