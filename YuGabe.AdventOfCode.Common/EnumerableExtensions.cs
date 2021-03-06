﻿using System;
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

        public static (TResult? min, TResult? max) MinMaxE<T, TResult>(this IEnumerable<T> source, Func<T, TResult> elementSelector) =>
            (source.Min(elementSelector), source.Max(elementSelector));
        public static (T? min, T? max) MinMax<T>(this IEnumerable<T> source) =>
            (source.Min(), source.Max());

        public static IEnumerable<(T? previous, T current, T? next)> WithNeighbors<T>(this IEnumerable<T> source)
        {
            T? previous = default;
            foreach(var item in source)
            {
                yield return (previous, item, source.Skip(1).FirstOrDefault());
                previous = item;
            }
        }
    }
}
