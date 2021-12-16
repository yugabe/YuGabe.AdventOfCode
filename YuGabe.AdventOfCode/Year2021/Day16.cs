namespace YuGabe.AdventOfCode.Year2021;

public class Day16 : Day<bool[]>
{
    public override bool[] ParseInput(string rawInput) => rawInput.SelectMany(c => Convert.ToString(Convert.ToUInt16(c.ToString(), 16), 2).PadLeft(4, '0').Select(c => c == '1')).ToArray();

    public record Packet(string RawPacket, long Version, int PacketType, long? LiteralValue, Packet[]? SubPackets)
    {
        public virtual long TotalVersions => Version + SubPackets?.Sum(p => p.TotalVersions) ?? 0;
        public int Length => RawPacket.Length;
        private IEnumerable<long> Values => SubPackets?.Select(p => p.Value) ?? Empty<long>();
        public long Value => LiteralValue ?? PacketType switch
        {
            0 => Values.Sum(),
            1 => Values.Aggregate((acc, p) => acc * p),
            2 => Values.Min(),
            3 => Values.Max(),
            5 => Values.First() > Values.ElementAt(1) ? 1 : 0,
            6 => Values.First() < Values.ElementAt(1) ? 1 : 0,
            7 => Values.First() == Values.ElementAt(1) ? 1 : 0,
            _ => throw new NotImplementedException()
        };

        public static IEnumerable<Packet> ParseBinary(bool[] input, int depth = 0, long? limit = null)
        {
            var currentIndex = 0;
            while (currentIndex < input.Length - 8)
            {
                var (start, version, typeId) = (currentIndex, input[currentIndex..(currentIndex += 3)].ToInt(), input[currentIndex..(currentIndex += 3)].ToInt());

                if (typeId == 4)
                {
                    var chunks = 0;
                    while (input[currentIndex + (chunks++ * 5)]) ;
                    currentIndex += chunks * 5;
                    yield return new(input[start..currentIndex].ToBinaryString(), version, 4, input[(currentIndex - (chunks * 5))..currentIndex].Where((_, i) => i % 5 != 0).ToLong(), null);
                }
                else
                {
                    var (numberOfSubPackets, nextBits) = input[currentIndex++] ? ((int?)input[currentIndex..(currentIndex += 11)].ToInt(), (int?)null) : (null, input[currentIndex..(currentIndex += 15)].ToInt());
                    var children = ParseBinary(input[currentIndex..(nextBits == null ? ^0 : currentIndex + nextBits.Value)], depth + 1, numberOfSubPackets).ToArray();
                    yield return new(input[start..(currentIndex += children.Sum(c => c.Length))].ToBinaryString(), version, typeId, null, children);
                }
                if (limit != null && --limit <= 0)
                    yield break;
            }
        }
    }

    public override object ExecutePart1() => Packet.ParseBinary(Input).Single().TotalVersions;
    public override object ExecutePart2() => Packet.ParseBinary(Input).Single().Value;
}
