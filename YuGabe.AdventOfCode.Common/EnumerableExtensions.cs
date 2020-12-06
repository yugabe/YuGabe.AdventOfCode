using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

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
                if (previousBoundarySet)
                    partitionIterator = partitionIterator.Prepend(previousBoundary!);
                var partition = partitionIterator.ToList().AsReadOnly();
                source = source.Skip(partition.Count);

                if (!source.TryGetFirst(out var boundary))
                {
                    yield return partition.Any() ? partition : throw new InvalidOperationException("Invalid partitioning state.");
                    break;
                }

                source = source.Skip(1);

                if (!partitioningPredicate(boundary))
                    throw new InvalidOperationException("Invalid partitioning state.");

                if (partitioningMethod == PartitioningMethod.KeepWithLast)
                {
                    yield return partition.Append(boundary);
                }
                else if (partitioningMethod == PartitioningMethod.Ignore)
                {
                    yield return partition;
                }
                else if (partitioningMethod == PartitioningMethod.KeepSingle)
                {
                    yield return partition;
                    yield return new[] { boundary }.ToList().AsReadOnly();
                }
                else if (partitioningMethod == PartitioningMethod.KeepWithNext)
                {
                    yield return partition;
                    previousBoundary = boundary;
                    previousBoundarySet = true;
                }
            }
        }

        public static bool TryGetFirst<T>(this IEnumerable<T> source, [NotNullWhen(true)] out T? value)
        {
            value = default;
            foreach (var item in source)
            {
                value = item!;
                return true;
            }
            return false;
        }
    }
}
