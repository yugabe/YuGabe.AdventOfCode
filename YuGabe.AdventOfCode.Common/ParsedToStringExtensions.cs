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
                sb.Append(property.GetValue(obj));
                if (i < parameters.Length - 1)
                    sb.Append(parameter.GetCustomAttribute<SplitAttribute>()?.Separator ?? ";");
            }
        }
        return sb.ToString();
    }
}

