namespace YuGabe.AdventOfCode.Year2021;

public class Day8 : DayParsedToMany<Day8.Note>
{
    public record Note([Split(0, " | "), InnerSplit(" ")] string[] Patterns, [Split(1, " | "), InnerSplit(" ")] string[] Digits) : Parsed { }

    public override object ExecutePart1() => Input.SelectMany(i => i.Digits).Count(d => d.Length is 2 or 3 or 4 or 7);

    public override object ExecutePart2() => Input.Sum(note =>
    {
        var allDigits = note.Patterns.ToDictionary(p => p, _ => -1);

        string SetUnknownDigit(int value, Func<string, bool> predicate)
        {
            var key = allDigits.Where(d => d.Value == -1).Select(d => d.Key).Single(predicate);
            allDigits.Remove(key);
            allDigits[key = new(key.OrderBy(c => c).ToArray())] = value;
            return key;
        }

        var _1 = SetUnknownDigit(1, k => k.Length == 2);
        var _4 = SetUnknownDigit(4, k => k.Length == 4);
        var _7 = SetUnknownDigit(7, k => k.Length == 3);
        var _8 = SetUnknownDigit(8, k => k.Length == 7);
        var _9 = SetUnknownDigit(9, k => k.Length == 6 && _4.All(k.Contains));
        var _0 = SetUnknownDigit(0, k => k.Length == 6 && _1.All(k.Contains));
        var _6 = SetUnknownDigit(6, k => k.Length == 6);
        var _3 = SetUnknownDigit(3, k => _1.All(k.Contains));
        var _5 = SetUnknownDigit(5, k => k.All(_9.Contains));
        var _2 = SetUnknownDigit(2, _ => true);

        return note.Digits.Aggregate(0, (acc, d) => acc * 10 + allDigits[new(d.OrderBy(c => c).ToArray())]);
    });
}
