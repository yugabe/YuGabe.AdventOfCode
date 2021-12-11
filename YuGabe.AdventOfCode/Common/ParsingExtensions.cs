using System.Reflection;

namespace YuGabe.AdventOfCode;

public static class ParsingExtensions
{
    private const SSO RemoveAndTrim = SSO.RemoveEmptyEntries | SSO.TrimEntries;

    public static (string token1, string? token2) SplitToTuple2(this string text, string? separator = " ", SSO splitOptions = RemoveAndTrim)
    {
        var tokens = text.Split(separator, splitOptions);
        return (tokens[0], tokens.ElementAtOrDefault(1));
    }

    public static (string token1, string? token2, string? token3) SplitToTuple3(this string text, string? separator = " ", SSO splitOptions = RemoveAndTrim)
    {
        var tokens = text.Split(separator, splitOptions);
        return (tokens[0], tokens.ElementAtOrDefault(1), tokens.ElementAtOrDefault(2));
    }

    public static (string token1, string? token2, string? token3, string? token4) SplitToTuple4(this string text, string? separator = " ", SSO splitOptions = RemoveAndTrim)
    {
        var tokens = text.Split(separator, splitOptions);
        return (tokens[0], tokens.ElementAtOrDefault(1), tokens.ElementAtOrDefault(2), tokens.ElementAtOrDefault(3));
    }

    public static string[] SplitAtNewLines(this string text, string? separator = "\n", SSO splitOptions = SSO.TrimEntries | SSO.RemoveEmptyEntries)
        => text.Split(separator, splitOptions);

    public static (string token1, string? token2)[] GetLinesToTuple2(this string text, string? lineSeparator = "\n", SSO lineSplitOptions = RemoveAndTrim, string? separator = " ", SSO splitOptions = RemoveAndTrim)
        => text.SplitAtNewLines(lineSeparator, lineSplitOptions).Select(l => l.SplitToTuple2(separator, splitOptions)).ToArray();
    public static (string token1, string? token2, string? token3)[] GetLinesToTuple3(this string text, string? lineSeparator = "\n", SSO lineSplitOptions = RemoveAndTrim, string? separator = " ", SSO splitOptions = RemoveAndTrim)
        => text.SplitAtNewLines(lineSeparator, lineSplitOptions).Select(l => l.SplitToTuple3(separator, splitOptions)).ToArray();
    public static (string token1, string? token2, string? token3, string? token4)[] GetLinesToTuple4(this string text, string? lineSeparator = "\n", SSO lineSplitOptions = RemoveAndTrim, string? separator = " ", SSO splitOptions = RemoveAndTrim)
        => text.SplitAtNewLines(lineSeparator, lineSplitOptions).Select(l => l.SplitToTuple4(separator, splitOptions)).ToArray();
    public static T[] SelectLinesFromTuple2<T>(this string text, Func<(string token1, string? token2), T> selector, string? lineSeparator = "\n", SSO lineSplitOptions = RemoveAndTrim, string? separator = " ", SSO splitOptions = RemoveAndTrim)
        => text.SplitAtNewLines(lineSeparator, lineSplitOptions).Select(l => l.SplitToTuple2(separator, splitOptions)).Select(selector).ToArray();
    public static T[] SelectLinesFromTuple3<T>(this string text, Func<(string token1, string? token2, string? token3), T> selector, string? lineSeparator = "\n", SSO lineSplitOptions = RemoveAndTrim, string? separator = " ", SSO splitOptions = RemoveAndTrim)
        => text.SplitAtNewLines(lineSeparator, lineSplitOptions).Select(l => l.SplitToTuple3(separator, splitOptions)).Select(selector).ToArray();
    public static T[] SelectLinesFromTuple4<T>(this string text, Func<(string token1, string? token2, string? token3, string? token4), T> selector, string? lineSeparator = "\n", SSO lineSplitOptions = RemoveAndTrim, string? separator = " ", SSO splitOptions = RemoveAndTrim)
        => text.SplitAtNewLines(lineSeparator, lineSplitOptions).Select(l => l.SplitToTuple4(separator, splitOptions)).Select(selector).ToArray();

    private static MethodInfo GenericToMethod { get; } = typeof(ParsingExtensions).GetMethod(nameof(To), 1, BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string) }, null) ?? throw new MissingMethodException(nameof(ParsingExtensions), nameof(To));
    private static MethodInfo GenericToManyMethod { get; } = typeof(ParsingExtensions).GetMethod(nameof(ToMany), 1, BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string), typeof(string), typeof(SSO) }, null) ?? throw new MissingMethodException(nameof(ParsingExtensions), nameof(ToMany));

    private static object To(Type type, string? text) => GenericToMethod.MakeGenericMethod(type).Invoke(null, new[] { text })!;
    private static object ToMany(Type type, string? text, string? separator, SSO splitOptions) => GenericToManyMethod.MakeGenericMethod(type).Invoke(null, new object?[] { text, separator, splitOptions })!;

    public static T To<T>(this string? text)
    {
        var converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T));
        if (converter.CanConvertFrom(typeof(string)))
            return (T)converter.ConvertFrom(text!)!;

        var ctor = typeof(T).GetConstructors().Single();
        var ctorParams = ctor.GetParameters();
        if (ctorParams.Length == 1 && ctorParams[0].ParameterType == typeof(string))
            return (T)ctor.Invoke(null, new[] { text })!;

        return (T)ctor.Invoke(ctorParams.Select(parameter =>
        {
            if (parameter.GetCustomAttribute<SplitAttribute>() is { } split && text?.Split(split.Separator, split.StringSplitOptions)?.ElementAtOrDefault(split.Index) is var value)
            {
                if (parameter.GetCustomAttribute<InnerSplitAttribute>() is { } innerSplit)
                    return ToMany(parameter.ParameterType.GetInterfaces().FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))?.GenericTypeArguments.ElementAtOrDefault(0) ?? throw new NotSupportedException($"The provided constructor parameter {parameter.Name} cannot be assigned to from an {nameof(IEnumerable<object>)}, as its type doesn't implement it."), value, innerSplit.Separator, innerSplit.StringSplitOptions);

                return To(parameter.ParameterType, value);
            }

            return To(parameter.ParameterType, text);
        }).ToArray())!;

    }
    public static T[] ToMany<T>(this string text, string? separator = "\n", SSO splitOptions = SSO.TrimEntries | SSO.RemoveEmptyEntries) => text.SplitAtNewLines(separator, splitOptions).Select(line => line.To<T>()).ToArray();
}
