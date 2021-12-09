namespace YuGabe.AdventOfCode.Year2018
{
    public class Day8 : Day<int[]>
    {
        public override int[] ParseInput(string input)
            => input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

        public override object ExecutePart1()
        {
            var metaTotal = 0;

            for (var i = -1; i < Input.Length; i++)
            {
                ParseItem();

                void ParseItem()
                {
                    var children = Input[++i];
                    var meta = Input[++i];
                    for (var c = 0; c < children; c++)
                        ParseItem();
                    for (var m = 0; m < meta; m++)
                        metaTotal += Input[++i];
                }
            }
            return metaTotal;
        }

        public override object ExecutePart2()
        {
            var i = -1;
            return ParseItem();

            int ParseItem()
            {
                var (childrenNum, meta) = (Input[++i], Input[++i]);
                if (childrenNum == 0)
                    return Enumerable.Range(0, meta).Sum(__ => Input[++i]);

                var children = Enumerable.Range(0, childrenNum).Select(_ => ParseItem()).ToArray();
                return Enumerable.Range(0, meta).Select(_ => Input[++i] - 1).Sum(ix => ix < children.Length && ix >= 0 ? children[ix] : 0);
            }
        }
    }
}
