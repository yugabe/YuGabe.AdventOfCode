using YuGabe.AdventOfCode.Common;

namespace YuGabe.AdventOfCode.Year2022;
public class Day7 : Day.NewLineSplitParsed<string>
{
    public abstract record FSEntry(string Name)
    {
        public Dir? ParentDir { get; set; }
    }
    public record File(string Name, int Size) : FSEntry(Name);
    public record Dir(string Name) : FSEntry(Name), ITreeNode<Dir>
    {
        public string FullName => ParentDir == null ? Name : $"{ParentDir.FullName}/{Name}";
        private readonly List<FSEntry> _children = new();
        public IEnumerable<FSEntry> Children => _children;
        public void Add(FSEntry entry)
        {
            _children.Add(entry);
            entry.ParentDir = this;
        }
        public int TotalSize => Children.Sum(e => e is File f ? f.Size : e is Dir d ? d.TotalSize : throw new InvalidOperationException());
        public override string ToString() => $"{FullName} | {TotalSize} | {_children.OfType<File>().Count()} files, {_children.OfType<Dir>().Count()} dirs";

        IEnumerable<Dir> ITreeNode<Dir>.Children => Children.OfType<Dir>();
    }

    public override object ExecutePart1() => EnumerateRootDirectory().EnumerateDepthFirst().Select(d => d.TotalSize).Where(s => s < 100_000).Sum();

    private Dir EnumerateRootDirectory()
    {
        var root = new Dir("/");
        var currentDirectory = root;
        for (var i = 0; i < Input.Length; i++)
        {
            var line = Input[i];
            if (line.StartsWith("$ cd "))
            {
                currentDirectory = line[5..] switch
                {
                    "/" => root,
                    ".." => currentDirectory.ParentDir ?? throw new InvalidOperationException(),
                    var dir => currentDirectory.Children.OfType<Dir>().Single(d => d.Name == dir)
                };
            }
            else if (line is "$ ls")
            {
                foreach (var (token1, token2) in Input[(i + 1)..].TakeWhile(l => l[0] != '$').Select(l => l.SplitToTuple2()))
                    currentDirectory.Add(token1 is "dir" ? new Dir(token2!) : new File(token2!, int.Parse(token1)));
                i += currentDirectory.Children.Count();
            }
        }
        return root;
    }

    public override object ExecutePart2()
    {
        var root = EnumerateRootDirectory();
        var totalSize = root.TotalSize;
        return root.EnumerateDepthFirst().Select(d => d.TotalSize).Where(s => totalSize - s < 40_000_000).Order().First();
    }
}
