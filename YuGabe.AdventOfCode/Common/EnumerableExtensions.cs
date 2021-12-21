using System.Diagnostics.CodeAnalysis;

namespace YuGabe.AdventOfCode
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> SequentialPartition<T>(this IEnumerable<T> source, Func<T, bool> partitioningPredicate, PartitioningMethod partitioningMethod = PartitioningMethod.Ignore)
        {
            T? previousBoundary = default;
            var previousBoundarySet = false;
            while (source.Any())
            {
                var partitionIterator = source.TakeWhile(i => !partitioningPredicate(i));
                source = source.Skip(partitionIterator.Count());
                if (previousBoundarySet)
                {
                    partitionIterator = partitionIterator.Prepend(previousBoundary!);
                    previousBoundarySet = false;
                }
                var partition = partitionIterator.ToList().AsReadOnly();

                if (!source.TryGetFirst(out var boundary))
                {
                    yield return partition.Any() ? partition : throw new InvalidOperationException("Invalid partitioning state.");
                    break;
                }

                source = source.Skip(1);

                if (!partitioningPredicate(boundary))
                    throw new InvalidOperationException("Invalid partitioning state.");

                switch (partitioningMethod)
                {
                    case PartitioningMethod.Ignore:
                        if (partition.Count > 0)
                            yield return partition;
                        break;
                    case PartitioningMethod.KeepWithLast:
                        yield return partition.Append(boundary);
                        break;
                    case PartitioningMethod.KeepSingle:
                        if (partition.Count > 0)
                            yield return partition;
                        yield return new[] { boundary }.ToList().AsReadOnly();
                        break;
                    case PartitioningMethod.KeepWithNext:
                        if (partition.Count > 0)
                            yield return partition;
                        previousBoundary = boundary;
                        previousBoundarySet = true;
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        public static bool TryGetFirst<T>(this IEnumerable<T> source, [NotNullWhen(true)] out T? value)
        {
            foreach (var item in source)
            {
                value = item!;
                return true;
            }
            value = default;
            return false;
        }

        public static void Enqueue<T>(this Queue<T> queue, IEnumerable<T> items)
        {
            foreach (var item in items)
                queue.Enqueue(item);
        }

        public static void Enqueue<T>(this Queue<T> queue, params T[] items) => queue.Enqueue(items.AsEnumerable());

        public static (TResult? min, TResult? max) MinMax<T, TResult>(this IEnumerable<T> source, Func<T, TResult> elementSelector) =>
            (source.Min(elementSelector), source.Max(elementSelector));
        public static (T? min, T? max) MinMax<T>(this IEnumerable<T> source) =>
            (source.Min(), source.Max());

        public static IEnumerable<(T? previous, T current, T? next)> WithNeighbors<T>(this IEnumerable<T> source)
        {
            T? previous = default;
            foreach (var item in source)
            {
                yield return (previous, item, source.Skip(1).FirstOrDefault());
                previous = item;
            }
        }

        public static IEnumerable<IEnumerable<T>> Permutate<T>(this IEnumerable<T> source)
        {
            if (!source.Any())
                yield break;
            var first = source.First();
            if (source.Skip(1).Any())
            {
                foreach (var permutation in source.SelectMany((element, index) => source.Where((_, i) => i != index).Permutate().Select(others => others.Prepend(element))))
                    yield return permutation;
            }
            else
                yield return new[] { first };
        }

        public static IEnumerable<(T Left, T Right)> CartesianProduct<T>(this IEnumerable<T> source, bool skipSameIndexProduct) => source.SelectMany<T, (T, T)>(skipSameIndexProduct ? (l, li) => source.Where((r, ri) => li != ri).Select(r => (l, r)) : (l, li) => source.Select(r => (l, r)));

        public static IEnumerable<(T1 Left, T2 Right)> CartesianProduct<T1, T2>(this IEnumerable<T1> leftSource, IEnumerable<T2> rightSource) => leftSource.SelectMany(l => rightSource.Select(r => (l, r)));

        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue valueToAdd, Func<TValue, TValue> newValueFunc) => source[key] = source.TryGetValue(key, out var oldValue) ? newValueFunc(oldValue) : valueToAdd;

        public static IEnumerable<T> LoopInfinitely<T>(this IEnumerable<T> source)
        {
            while (true)
                foreach (var item in source)
                    yield return item;
        }
    }
}
