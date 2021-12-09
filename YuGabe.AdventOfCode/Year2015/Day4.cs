using System.Security.Cryptography;
using System.Text;
using Tidy.AdventOfCode;

namespace YuGabe.AdventOfCode.Year2015
{
    public class Day4 : Day
    {
        public override object ExecutePart1()
        {
            var hasher = MD5.Create();
            var currentNumber = 1;
            while (true)
            {
                var input = Encoding.UTF8.GetBytes(Input + currentNumber.ToString());
                var hash = hasher.ComputeHash(input);
                if (hash[0] == 0 && hash[1] == 0 && hash[2] < 16)
                    return currentNumber;
                currentNumber++;
            }
        }

        public override object ExecutePart2()
        {
            var hasher = MD5.Create();
            var currentNumber = 1;
            while (true)
            {
                var input = Encoding.UTF8.GetBytes(Input + currentNumber.ToString());
                var hash = hasher.ComputeHash(input);
                if (hash[0] == 0 && hash[1] == 0 && hash[2] == 0)
                    return currentNumber;
                currentNumber++;
            }
        }
    }
}
