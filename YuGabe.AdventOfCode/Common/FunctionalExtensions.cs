namespace YuGabe.AdventOfCode
{
    public static class FunctionalExtensions
    {
        public static TResult FeedTo<T, TResult>(this T item, Func<T, TResult> function) => function(item);
        public static TResult FeedTo<T1, T2, TResult>(this (T1, T2) tuple, Func<T1, T2, TResult> function) => function(tuple.Item1, tuple.Item2);
        public static TResult FeedTo<T1, T2, T3, TResult>(this (T1, T2, T3) tuple, Func<T1, T2, T3, TResult> function) => function(tuple.Item1, tuple.Item2, tuple.Item3);
        public static TResult FeedTo<T1, T2, T3, T4, TResult>(this (T1, T2, T3, T4) tuple, Func<T1, T2, T3, T4, TResult> function) => function(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);

        public static T Interleave<T>(this T item, Action<T> action)
        {
            action(item);
            return item;
        }

        public static (T first, T second) Split2<T>(this IEnumerable<T> items)
        {
            var array = items.Take(3).ToArray();
            if (array.Length != 2)
                throw new InvalidOperationException();
            return (array[0], array[1]);
        }
    }
}
