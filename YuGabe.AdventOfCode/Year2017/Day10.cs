using System.Text;

namespace YuGabe.AdventOfCode.Year2017
{
    public class Day10 : Day
    {
        public override object ExecutePart1()
        {
            var list = Enumerable.Range(0, 256).ToArray();
            int skip = 0, pos = 0;
            foreach (var length in Input.Split(',').Select(int.Parse))
            {
                foreach (var e in list.Concat(list).Skip(pos).Take(length).Reverse().ToArray())
                {
                    list[pos] = e;
                    pos = (pos + 1) % list.Length;
                }
                pos = (pos + skip++) % list.Length;
            }

            return list[0] * list[1];
        }

        public override object ExecutePart2()
        {
            var list = Enumerable.Range(0, 256).Select(i => (byte)i).ToArray();
            int skip = 0, pos = 0;
            foreach (var _ in Enumerable.Range(0, 64))
            {
                foreach (var length in Encoding.ASCII.GetBytes(Input).Concat(new byte[] { 17, 31, 73, 47, 23 }))
                {
                    foreach (var e in list.Concat(list).Skip(pos).Take(length).Reverse().ToArray())
                    {
                        list[pos] = e;
                        pos = (pos + 1) % list.Length;
                    }
                    pos = (pos + skip++) % list.Length;
                }
            }
            return string.Join("", Enumerable.Range(0, 16).Select(i => list.Skip(i * 16).Take(16).Aggregate((a, b) => (byte)(a ^ b))).Select(n => n.ToString("X2")));
        }
    }
}
