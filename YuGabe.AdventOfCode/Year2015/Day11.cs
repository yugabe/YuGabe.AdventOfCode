namespace YuGabe.AdventOfCode.Year2015
{
    public class Day11 : Day
    {
        public override object ExecutePart1() => GetNextPasswordAfter(Input);

        public override object ExecutePart2() => GetNextPasswordAfter(Input, 2);

        public static string GetNextPasswordAfter(string password, int count = 1)
        {
            var current = password.ToCharArray();
            do
            {
                for (var x = current.Length - 1; x > 0; x--)
                {
                    if (++current[x] == '{')
                        current[x] = 'a';
                    else break;
                }
            }
            while (!(
                current.WithIndexes().Skip(2).Any(e => current[e.Index - 1] == e.Element - 1 && current[e.Index - 2] == e.Element - 2) 
                && current.All(c => c is not 'i' and not 'o' and not 'l')
                && current.Where((e, i) => i != 0 && current[i - 1] == e && current.ElementAtOrDefault(i + 1) != e).Count() >= 2
                && --count == 0));
            return new string(current);
        }
    }
}
