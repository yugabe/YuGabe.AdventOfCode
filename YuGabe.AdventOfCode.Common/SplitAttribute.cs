
namespace YuGabe.AdventOfCode
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class SplitAttribute : Attribute
    {
        public SplitAttribute(int index, string separator = " ", StringSplitOptions stringSplitOptions = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        {
            Index = index;
            Separator = separator;
            StringSplitOptions = stringSplitOptions;
        }

        public int Index { get; }
        public string Separator { get; }
        public StringSplitOptions StringSplitOptions { get; }
    }
}