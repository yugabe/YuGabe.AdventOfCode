namespace YuGabe.AdventOfCode.Year2021;

public class Day18 : Day<Day18.Number[]>
{
    public abstract record Number
    {
        public static (Number Number, int Index) Parse(ReadOnlySpan<char> rawNumber)
        {
            if (char.IsNumber(rawNumber[0]))
                return (new Plain(int.Parse(rawNumber[0].ToString())), 1);
            else if (rawNumber[0] == '[')
            {
                var (left, leftIndex) = Parse(rawNumber[1..]);
                var (right, rightIndex) = rawNumber[++leftIndex] == ',' ? Parse(rawNumber[++leftIndex..]) : throw new InvalidOperationException();
                return rawNumber[leftIndex + rightIndex] == ']' ? (new Pair(left, right), leftIndex + rightIndex + 1) : throw new InvalidOperationException();
            }
            else throw new InvalidOperationException();
        }
        public static Pair operator +(Number left, Number right) => new Pair(left, right).Reduce();
        public Number Root => Parent?.Root ?? this;
        public Pair? Parent { get; set; }
        public Number Copy() => Parse(ToString()).Number;
        public void ReplaceWith(Number newNumber)
        {
            newNumber.Parent = Parent ?? throw new InvalidOperationException();
            if (ReferenceEquals(Parent.Left, this))
                Parent.Left = newNumber;
            else if (ReferenceEquals(Parent.Right, this))
                Parent.Right = newNumber;
            Parent = null;
        }
        public abstract long Magnitude { get; }
        public abstract IEnumerable<Number> GetElements(bool inOrder);

        public record Plain : Number
        {
            public Plain(long value) => Value = value;
            public long Value { get; set; }
            public override long Magnitude => Value;
            public override IEnumerable<Number> GetElements(bool inOrder) => Repeat(this, 1);
            public override string ToString() => Value.ToString();
        }

        public record Pair : Number
        {
            public Pair(Number left, Number right)
            {
                (Left, Right) = (left, right);
                Left.Parent = Right.Parent = this;
            }
            public Number Left { get; set; }
            public Number Right { get; set; }
            public Pair Reduce()
            {
                while (true)
                {
                    if (Root.GetElements(true).OfType<Pair>().FirstOrDefault(e => e.Parent?.Parent?.Parent?.Parent != null) is { } pairShouldExplode)
                    {
                        if (!(pairShouldExplode.Left is Plain leftNumber && pairShouldExplode.Right is Plain rightNumber))
                            throw new InvalidOperationException();
                        if (pairShouldExplode.Root.GetElements(false).SkipWhile(e => !ReferenceEquals(e, pairShouldExplode)).Skip(1).OfType<Plain>().FirstOrDefault() is { } leftPlainNeighbor)
                            leftPlainNeighbor.Value += leftNumber.Value;
                        if (pairShouldExplode.Root.GetElements(true).SkipWhile(e => !ReferenceEquals(e, pairShouldExplode)).Skip(1).OfType<Plain>().FirstOrDefault() is { } rightPlainNeighbor)
                            rightPlainNeighbor.Value += rightNumber.Value;
                        pairShouldExplode.ReplaceWith(new Plain(0));
                    }
                    else if (Root.GetElements(true).OfType<Plain>().FirstOrDefault(e => e.Value >= 10) is { } numberShouldSplit)
                        numberShouldSplit.ReplaceWith(new Pair(new Plain(numberShouldSplit.Value / 2), new Plain((int)Math.Ceiling((double)numberShouldSplit.Value / 2))));
                    else break;
                }
                return this;
            }
            public override long Magnitude => (3 * Left.Magnitude) + (2 * Right.Magnitude);
            public override IEnumerable<Number> GetElements(bool inOrder) => (inOrder ? Left.GetElements(inOrder).Concat(Right.GetElements(inOrder)) : Right.GetElements(inOrder).Concat(Left.GetElements(inOrder))).Append(this);
            public override string ToString() => $"[{Left},{Right}]";
        }
    }

    public override Number[] ParseInput(string rawInput) => rawInput.SplitAtNewLines().Select(line => Number.Parse(line.AsSpan()) is var (number, index) && index == line.Length ? number : throw new InvalidOperationException()).ToArray();
    public override object ExecutePart1() => Input.Aggregate((acc, val) => acc + val).Magnitude;
    public override object ExecutePart2() => Input.CartesianProduct(true).Max(e => (e.Left.Copy() + e.Right.Copy()).Magnitude);
}
