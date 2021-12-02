namespace YuGabe.AdventOfCode;

public static class ParsingExtensions
{
    public static (string token1, string? token2) SplitToTuple2(this string text)
    {
        var tokens = text.Split();
        return (tokens[0], tokens.ElementAtOrDefault(1));
    }

    public static (string token1, string? token2, string? token3) SplitToTuple3(this string text)
    {
        var tokens = text.Split();
        return (tokens[0], tokens.ElementAtOrDefault(1), tokens.ElementAtOrDefault(2));
    }

    public static (string token1, string? token2, string? token3, string? token4) SplitToTuple4(this string text)
    {
        var tokens = text.Split();
        return (tokens[0], tokens.ElementAtOrDefault(1), tokens.ElementAtOrDefault(2), tokens.ElementAtOrDefault(3));
    }

    public static string[] GetLines(this string text) => text.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    public static (string token1, string? token2)[] GetLinesToTuple2(this string text) => text.GetLines().Select(l => l.SplitToTuple2()).ToArray();
    public static (string token1, string? token2, string? token3)[] GetLinesToTuple3(this string text) => text.GetLines().Select(l => l.SplitToTuple3()).ToArray();
    public static (string token1, string? token2, string? token3, string? token4)[] GetLinesToTuple4(this string text) => text.GetLines().Select(l => l.SplitToTuple4()).ToArray();
    public static T[] SelectLinesFromTuple2<T>(this string text, Func<(string token1, string? token2), T> selector) => text.GetLines().Select(l => l.SplitToTuple2()).Select(selector).ToArray();
    public static T[] SelectLinesFromTuple3<T>(this string text, Func<(string token1, string? token2, string? token3), T> selector) => text.GetLines().Select(l => l.SplitToTuple3()).Select(selector).ToArray();
    public static T[] SelectLinesFromTuple4<T>(this string text, Func<(string token1, string? token2, string? token3, string? token4), T> selector) => text.GetLines().Select(l => l.SplitToTuple4()).Select(selector).ToArray();
}