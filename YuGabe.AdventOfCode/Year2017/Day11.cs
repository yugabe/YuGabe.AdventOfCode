namespace YuGabe.AdventOfCode.Year2017
{
    public class Day11 : Day
    {
        public override object ExecutePart1()
        {
            var steps = Input.Split(',').GroupBy(c => c).Select(c => (direction: c.Key, count: c.Count())).ToDictionary(c => c.direction, c => c.count);
            var nw = Math.Abs(steps["nw"] - steps["se"]);
            var n = Math.Abs(steps["n"] - steps["s"]);
            var ne = Math.Abs(steps["ne"] - steps["sw"]);

            while (nw > 0 && ne > 0)
            {
                nw--;
                ne--;
                n++;
            }
            return nw + ne + n;
        }

        public override object ExecutePart2()
        {
            var steps = new Dictionary<string, int>
            {
                ["nw"] = 0,
                ["n"] = 0,
                ["ne"] = 0,
                ["se"] = 0,
                ["s"] = 0,
                ["sw"] = 0
            };

            return Input.Split(',').Max(s =>
            {
                steps[s]++;
                var nw = Math.Abs(steps["nw"] - steps["se"]);
                var n = Math.Abs(steps["n"] - steps["s"]);
                var ne = Math.Abs(steps["ne"] - steps["sw"]);

                while (nw > 0 && ne > 0)
                {
                    nw--;
                    ne--;
                    n++;
                }
                return nw + ne + n;
            });
        }
    }
}
