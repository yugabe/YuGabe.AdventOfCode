namespace YuGabe.AdventOfCode.Common;

public interface ITreeNode<T> where T : ITreeNode<T>
{
    public IEnumerable<T> Children { get; }
}

public static class TreeExtensions
{
    public static IEnumerable<T> EnumerateDepthFirst<T>(this T root) where T : ITreeNode<T> 
    {
        var stack = new Stack<T>();
        stack.Push(root);
        while(stack.TryPop(out var node))
        {
            yield return node;
            foreach (var child in node.Children)
                stack.Push(child);
        }
    }

    public static IEnumerable<T> EnumerateBreadthFirst<T>(this T root) where T : ITreeNode<T>
    {
        var queue = new Queue<T>();
        queue.Enqueue(root);
        while(queue.TryDequeue(out var node))
        {
            yield return node;
            foreach (var child in node.Children)
                queue.Enqueue(child);
        }
    }
}
