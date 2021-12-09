namespace YuGabe.AdventOfCode.Year2018
{
    public class Day5 : Day
    {
        public override object ExecutePart1() => ExecutePart1(Input);
        public static object ExecutePart1(string input)
        {
            var chars = new LinkedList<char>(input);
            var current = chars.First;
            while (true)
            {
                if (current?.Next == null)
                    return chars.Count;

                if (char.IsLower(current.Value) ^ char.IsLower(current.Next.Value) && current.Value.ToString().ToLower() == current.Next.Value.ToString().ToLower())
                {
                    chars.Remove(current.Next);
                    if (current.Previous != null)
                    {
                        current = current.Previous;
                        chars.Remove(current.Next!);
                    }
                    else if (current.Next != null)
                    {
                        current = current.Next;
                        chars.Remove(current.Previous!);
                    }
                    else
                    {
                        chars.Remove(current);
                    }
                }
                else
                {
                    current = current.Next;
                }
            }
        }

        public override object ExecutePart2() =>
            Input.Distinct().Min(c => ExecutePart1(Input.Replace(c.ToString().ToLower(), "").Replace(c.ToString().ToUpper(), "")))!;
    }
}
