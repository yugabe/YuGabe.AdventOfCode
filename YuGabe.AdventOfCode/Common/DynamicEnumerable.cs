using System.Collections;

namespace YuGabe.AdventOfCode.Common;
public class DynamicEnumerable<T> : IEnumerable<T>
{
    public DynamicEnumerable(T startValue, Func<T, T?> next, Func<T?, bool>? breakCondition = null)
    {
        StartValue = startValue;
        Next = next;
        BreakCondition = breakCondition;
    }

    private T StartValue { get; }
    private Func<T, T?> Next { get; }
    private Func<T?, bool>? BreakCondition { get; }

    public IEnumerator<T> GetEnumerator()
    {
        var current = StartValue;
        while (true)
        {
            yield return current;
            current = Next(current);
            if (current == null || BreakCondition?.Invoke(current) == true)
                yield break;
        }
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
