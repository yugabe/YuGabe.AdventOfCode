using System.Collections;

namespace YuGabe.AdventOfCode.Year2022;
public class Day13 : Day<(Day13.Packet Left, Day13.Packet Right)[]>
{
    public override (Packet Left, Packet Right)[] ParseInput(string rawInput) =>
        rawInput.SelectLinesFromTuple2(tokens => (Packet.Parse(tokens.token1), Packet.Parse(tokens.token2!)), "\n\n", separator: "\n");
    public abstract class Packet : IComparable<Packet>
    {
        public static Packet Parse(string packet)
        {
            var index = 0;
            return Parse(packet, ref index);
        }
        private static Packet Parse(string packet, ref int index)
        {
            if (packet[index] is >= '0' and <= '9')
            {
                var from = index;
                while (index + 1 < packet.Length && packet[index + 1] is >= '0' and <= '9')
                    index++;
                return new IntPacket() { Value = int.Parse(packet[from..++index]) };
            }
            else if (packet[index] == '[')
            {
                var list = new ListPacket();
                if (packet[++index] == ']')
                {
                    index++;
                    return list;
                }

                while (true)
                {
                    list.Add(Parse(packet, ref index));
                    var current = packet[index++];
                    if (current == ']')
                        return list;
                    if (current != ',')
                        throw new Exception($"Invalid character while parsing list: '{current}' [{index - 1}]");
                }
            }
            throw new Exception($"Invalid character while parsing packet: '{packet[index]}' [{index}]");
        }

        public int CompareTo(Packet? other) => other is null ? 1 : this == other ? 0 : this < other ? -1 : 1;

        public static bool operator <(Packet left, Packet right) => (left, right) switch
        {
            (IntPacket leftInt, IntPacket rightInt) => leftInt.Value < rightInt.Value,
            (IntPacket leftInt, ListPacket rightList) => new ListPacket() { leftInt } < rightList,
            (ListPacket leftList, IntPacket rightInt) => leftList < new ListPacket() { rightInt },
            (ListPacket leftList, ListPacket rightList) => (leftList, rightList) switch
            {
                ([], { Length: var rightLength }) => rightLength > 0,
                (not [], []) => false,
                ([var leftHead, .. var leftTail], [var rightHead, .. var rightTail]) => leftHead < rightHead || (leftHead == rightHead && new ListPacket(leftTail.ToArray()) < new ListPacket(rightTail.ToArray())),
                _ => throw new Exception("Invalid case.")
            },
            _ => throw new InvalidOperationException($"Cannot compare {left.GetType().Name} and {right.GetType().Name}.")
        };
        public static bool operator ==(Packet left, Packet right) => (left, right) switch
        {
            (IntPacket leftInt, IntPacket rightInt) => leftInt.Value == rightInt.Value,
            (IntPacket leftInt, ListPacket rightList) => new ListPacket() { leftInt } == rightList,
            (ListPacket leftList, IntPacket rightInt) => leftList == new ListPacket() { rightInt },
            (ListPacket leftList, ListPacket rightList) => (leftList, rightList) switch
            {
                ([], []) => true,
                ({ Length: var leftLength }, { Length: var rightLength }) when leftLength != rightLength => false,
                ([var leftHead, .. var leftTail], [var rightHead, .. var rightTail]) => leftHead == rightHead && new ListPacket(leftTail.ToArray()) == new ListPacket(rightTail.ToArray()),
                _ => throw new Exception("Invalid case.")
            },
            _ => throw new InvalidOperationException($"Cannot compare {left.GetType().Name} and {right.GetType().Name}.")
        };
        public static bool operator !=(Packet left, Packet right) => !(left == right);
        public static bool operator >(Packet left, Packet right) => right < left;

        public override bool Equals(object? obj) => obj is not null && ReferenceEquals(this, obj);

        public override int GetHashCode() => this switch
        {
            IntPacket intPacket => intPacket.Value.GetHashCode(),
            ListPacket listPacket => listPacket.Children.GetHashCode(),
            _ => throw new Exception("Invalid type.")
        };

        public static bool operator <=(Packet left, Packet right) => !(left > right);
        public static bool operator >=(Packet left, Packet right) => !(left < right);
    }
    public sealed class IntPacket : Packet
    {
        public required int Value { get; init; }

        public override string ToString() => Value.ToString();
    }
    public sealed class ListPacket : Packet, IEnumerable<Packet>
    {
        public ListPacket(IEnumerable<Packet>? children = null) => Children = (children ?? Empty<Packet>()).ToArray();
        public Packet[] Children { get; private set; }
        public int Length => Children.Length;
        public void Add(Packet child) => Children = Children.Append(child).ToArray();
        public override string ToString() => $"[{string.Join(",", Children.Select(c => c.ToString()))}]";
        public IEnumerator<Packet> GetEnumerator() => ((IEnumerable<Packet>)Children).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Children).GetEnumerator();
        public Packet this[int index] => Children[index];
        public Span<Packet> this[Range range] => Children[range];
    }

    public override object ExecutePart1() => Input.WithIndexes().Where(p => p.Element.Left < p.Element.Right).Sum(e => e.Index + 1);

    public override object ExecutePart2()
    {
        var dividers = ParseInput("[[2]]\n[[6]]").SelectMany(e => new[] { e.Left, e.Right }).ToHashSet();
        return Input.SelectMany(e => new[] { e.Left, e.Right }).Concat(dividers).Order().WithIndexes().Where(e => dividers.Contains(e.Element)).Aggregate(1, (acc, e) => acc * (e.Index + 1));
    }
}
