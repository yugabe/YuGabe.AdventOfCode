using System.Globalization;
using System.Text.RegularExpressions;

namespace YuGabe.AdventOfCode.Year2020
{
    public class Day4 : Day<IEnumerable<Day4.Passport>>
    {
        public record Passport(IReadOnlyDictionary<string, string> Values)
        {
            public string? this[string key] => Values.GetValueOrDefault(key);

            public string? BirthYear => this["byr"];
            public bool BirthYearIsValid => int.TryParse(BirthYear, out var birthYear) && birthYear is >= 1920 and <= 2002;
            public string? IssueYear => this["iyr"];
            public bool IssueYearIsValid => int.TryParse(IssueYear, out var issueYear) && issueYear is >= 2010 and <= 2020;
            public string? ExpirationYear => this["eyr"];
            public bool ExpirationYearIsValid => int.TryParse(ExpirationYear, out var expirationYear) && expirationYear is >= 2020 and <= 2030;
            public string? Height => this["hgt"];
            public bool HeightIsValid => (Height?.EndsWith("cm") == true && int.TryParse(Height[..^2], out var heightCm) && heightCm is >= 150 and <= 193) || (Height?.EndsWith("in") == true && int.TryParse(Height[..^2], out var heightIn) && heightIn is >= 59 and <= 76);
            public string? HairColor => this["hcl"];
            public bool HairColorIsValid => HairColor?.StartsWith('#') == true && HairColor.Length == 7 && int.TryParse(HairColor[1..], NumberStyles.HexNumber, null, out _);
            public string? EyeColor => this["ecl"];
            public bool EyeColorIsValid => EyeColor is "amb" or "blu" or "brn" or "gry" or "grn" or "hzl" or "oth";
            public string? PassportId => this["pid"];
            public bool PassportIdIsValid => PassportId is not null && Regex.IsMatch(PassportId, "^\\d{9}$");
            public string? CountryId => this["cid"];

            public bool PassportIsValid => BirthYearIsValid && IssueYearIsValid && ExpirationYearIsValid && HeightIsValid && HairColorIsValid && EyeColorIsValid && PassportIdIsValid;
        }

        public override IEnumerable<Passport> ParseInput(string rawInput)
        {
            var lines = rawInput.Split(' ', '\n').AsEnumerable();
            while (lines.Any(l => !string.IsNullOrWhiteSpace(l)))
            {
                yield return new Passport(string.Join(' ', lines.TakeWhile(l => !string.IsNullOrWhiteSpace(l))).Split(' ').Select(i => i.Split(':')).ToDictionary(i => i[0], i => i[1]));
                lines = lines.SkipWhile(l => !string.IsNullOrWhiteSpace(l)).Skip(1);
            }
        }

        public override object ExecutePart1() => Input.Count(p => new[] { p.BirthYear, p.IssueYear, p.ExpirationYear, p.Height, p.HairColor, p.EyeColor, p.PassportId }.All(i => i is not null));

        public override object ExecutePart2() => Input.Count(p => p.PassportIsValid);
    }
}
