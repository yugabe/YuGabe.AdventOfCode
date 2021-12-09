namespace YuGabe.AdventOfCode.Year2017
{
    public class Day9 : Day
    {
        public override object ExecutePart1()
        {
            var garbage = false;
            int totalScore = 0, currentScore = 0;
            for (var i = 0; i < Input.Length; i++)
            {
                switch (Input[i])
                {
                    case '!':
                        i++; continue;
                    case '<':
                        garbage = true; continue;
                    case '>':
                        garbage = false; continue;
                    case '{':
                        if (!garbage)
                        {
                            ++currentScore;
                            totalScore += currentScore;
                        }
                        continue;
                    case '}':
                        if (!garbage)
                            --currentScore;
                        continue;
                }
            }
            return totalScore;
        }

        public override object ExecutePart2()
        {
            var garbage = false;
            var totalGarbage = 0;
            for (var i = 0; i < Input.Length; i++)
            {
                switch (Input[i])
                {
                    case '!':
                        i++; continue;
                    case '<':
                        if (garbage) ++totalGarbage;
                        garbage = true; continue;
                    case '>':
                        garbage = false; continue;
                    default:
                        if (garbage)
                            ++totalGarbage;
                        break;
                }
            }
            return totalGarbage;
        }
    }
}
