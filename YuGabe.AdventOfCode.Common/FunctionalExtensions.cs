using System;

namespace YuGabe.AdventOfCode
{
    public static class FunctionalExtensions
    {
        public static TResult FeedTo<T1, T2, TResult>(this (T1, T2) tuple, Func<T1, T2, TResult> function) => function(tuple.Item1, tuple.Item2);
        public static TResult FeedTo<T1, T2, T3, TResult>(this (T1, T2, T3) tuple, Func<T1, T2, T3, TResult> function) => function(tuple.Item1, tuple.Item2, tuple.Item3);
        public static TResult FeedTo<T1, T2, T3, T4, TResult>(this (T1, T2, T3, T4) tuple, Func<T1, T2, T3, T4, TResult> function) => function(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);
    }
}
