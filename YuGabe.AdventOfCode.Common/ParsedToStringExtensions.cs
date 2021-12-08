using System.Collections;
using System.Reflection;

namespace YuGabe.AdventOfCode;

public static class ParsedToStringExtensions
{
    public static string ParsedToString(this object obj)
    {
        var type = obj.GetType();
        var parameters = type.GetConstructors().Single().GetParameters();
        var sb = new System.Text.StringBuilder();
        for (var i = 0; i < parameters.Length; i++)
        {
            var parameter = parameters[i];
            var property = type.GetProperty(parameter.Name ?? "");
            if (property != null)
            {
                var value = property.GetValue(obj);
                if (parameter.GetCustomAttribute<InnerSplitAttribute>() is { } innerSplit && value is IEnumerable<object> values)
                    sb.Append(string.Join(innerSplit.Separator, values));
                else 
                    sb.Append(value);
                if (i < parameters.Length - 1)
                    sb.Append(parameter.GetCustomAttribute<SplitAttribute>()?.Separator ?? ";");
            }
        }
        return sb.ToString();
    }
}

