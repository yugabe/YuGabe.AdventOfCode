
namespace YuGabe.AdventOfCode
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class InnerSplitAttribute : Attribute
    {
        public InnerSplitAttribute(string separator = " ", StringSplitOptions stringSplitOptions = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        {
            Separator = separator;
            StringSplitOptions = stringSplitOptions;
        }

        public string Separator { get; }
        public StringSplitOptions StringSplitOptions { get; }
    }
}