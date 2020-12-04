using System.Linq;
using Tidy.AdventOfCode;

namespace YuGabe.AdventOfCode.Year2015
{
    public class Day1 : Day
    {
        public override object ExecutePart1()
        
        {
            return Input.Count(c => c == '(') - Input.Count(c => c == ')');
        }

        public override object ExecutePart2()
        {
            var currentLevel = 0;
            var index = 0;
            foreach(var c in Input)
            {
                index++;
                if (c == '(') currentLevel++;
                else currentLevel--;
                if (currentLevel < 0)
                    return index;
            }
            return 0;
        }
    }
}
