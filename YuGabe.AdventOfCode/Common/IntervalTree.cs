namespace YuGabe.AdventOfCode;

public class IntervalTree<T, TKey> where TKey : notnull
{
    public IntervalTree(IReadOnlyCollection<T> elements, Func<T, TKey> intervalStartSelector, Func<T, TKey> intervalEndSelector, Func<TKey, TKey, TKey> centerFromMinMaxSelector, IComparer<TKey>? comparer = null)
    {
        Comparer = (comparer ??= Comparer<TKey>.Default);
        if (elements.Any(e => Comparer.Compare(intervalStartSelector(e), Center) > 0 && Comparer.Compare(intervalEndSelector(e), Center) < 0))
            throw new InvalidOperationException();
        Min = elements.Min(intervalStartSelector) ?? throw new InvalidOperationException();
        Max = elements.Max(intervalEndSelector) ?? throw new InvalidOperationException();
        Center = centerFromMinMaxSelector(Min, Max); //(Min + Max) / 2;
        var leftElements = elements.Where(e => Comparer.Compare(intervalEndSelector(e), Center) < 0).ToArray();
        if (leftElements.Length > 0)
            LeftTree = new IntervalTree<T, TKey>(leftElements, intervalStartSelector, intervalEndSelector, centerFromMinMaxSelector, comparer);
        var rightElements = elements.Where(e => Comparer.Compare(intervalStartSelector(e), Center) > 0).ToArray();
        if (rightElements.Length > 0)
            RightTree = new IntervalTree<T, TKey>(rightElements, intervalStartSelector, intervalEndSelector, centerFromMinMaxSelector, comparer);
        var centerItems = elements.Where(e => Comparer.Compare(intervalEndSelector(e), Center) > 0 && Comparer.Compare(intervalStartSelector(e), Center) < 0).ToArray();
        CenterItemsByBeginning = centerItems.OrderBy(intervalStartSelector).ToArray();
        CenterItemsByEnd = centerItems.OrderBy(intervalEndSelector).ToArray();
        IntervalStartSelector = intervalStartSelector;
        IntervalEndSelector = intervalEndSelector;
    }

    public IComparer<TKey> Comparer { get; }
    public TKey Min { get; }
    public TKey Center { get; }
    public TKey Max { get; }
    public IntervalTree<T, TKey>? LeftTree { get; }
    public IEnumerable<T> CenterItemsByBeginning { get; }
    public IEnumerable<T> CenterItemsByEnd { get; }
    public IntervalTree<T, TKey>? RightTree { get; }
    private Func<T, TKey> IntervalStartSelector { get; }
    private Func<T, TKey> IntervalEndSelector { get; }

    public IEnumerable<T> this[TKey key]
    {
        get
        {
            if (Comparer.Compare(key, Center) < 0)
                return (LeftTree?[key] ?? Empty<T>()).Concat(CenterItemsByEnd.Where(e => Comparer.Compare(IntervalStartSelector(e), key) < 0));
            else if (Comparer.Compare(key, Center) > 0)
                return CenterItemsByEnd.Where(e => Comparer.Compare(IntervalEndSelector(e), key) > 0).Concat(RightTree?[key] ?? Empty<T>());
            return CenterItemsByBeginning;
        }
    }
}
