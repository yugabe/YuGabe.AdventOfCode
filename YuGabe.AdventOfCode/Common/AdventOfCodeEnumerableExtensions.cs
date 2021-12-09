namespace System.Linq
{
    public static class AdventOfCodeEnumerableExtensions
    {
        public static IEnumerable<(T Element, int Index)> WithIndexes<T>(this IEnumerable<T> source) => source.Select((e, i) => (e, i));
    }
}
