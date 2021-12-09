using System.Text;

namespace YuGabe.AdventOfCode.Year2015
{
    public class Day10 : Day
    {
        public override object ExecutePart1()
        {
            Input = Input.Trim();
            for (var i = 0; i < 40; i++)
            {
                var current = Input;
                var sb = new StringBuilder();
                while (current.Length > 0)
                {
                    var c = current.First();
                    var count = current.TakeWhile(e => e == c).Count();
                    sb.Append(count);
                    sb.Append(c);
                    current = current[count..];
                }
                Input = sb.ToString();
            }
            return Input.Length;
        }

        public override object ExecutePart2()
        {
            Input = Input.Trim();
            for (var i = 0; i < 50; i++)
            {
                var sb = new StringBuilder();
                for (var y = 0; y < Input.Length; y++)
                {
                    var count = 1;
                    var c = Input[y];
                    for (var ry = y + 1; ry < Input.Length; ry++)
                    {
                        if (Input[ry] == c)
                        {
                            y++;
                            count++;
                        }
                        else break;
                    }
                    sb.Append(count);
                    sb.Append(c);
                }
                Input = sb.ToString();
                Console.WriteLine($"{i}: {Input.Length}");
            }
            return Input.Length;
        }
    }
}
