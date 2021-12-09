namespace YuGabe.AdventOfCode.Year2015
{
    public class Day14 : Day<List<Day14.Reindeer>>
    {
        public override List<Reindeer> ParseInput(string input)
        {
            return input.Trim().Split("\n").Select(p => new Reindeer(p)).ToList();
        }

        public override object ExecutePart1()
        {
            return Input.Max(deer =>
            {
                for (var time = 0; time < 2503; time++)
                {
                    if (!deer.Resting)
                        deer.TotalDistance += deer.Speed;
                    if (--deer.RemainingTimeInState == 0)
                        deer.RemainingTimeInState = (deer.Resting = !deer.Resting) ? deer.RestTimeLimit : deer.SpeedTimeLimit;
                }
                return deer.TotalDistance;
            });
        }

        public override object ExecutePart2()
        {
            for (var time = 0; time < 2503; time++)
            {
                foreach (var deer in Input)
                {
                    if (!deer.Resting)
                        deer.TotalDistance += deer.Speed;
                    if (--deer.RemainingTimeInState == 0)
                        deer.RemainingTimeInState = (deer.Resting = !deer.Resting) ? deer.RestTimeLimit : deer.SpeedTimeLimit;
                }
                Input.OrderByDescending(r => r.TotalDistance).First().Points++;
            }
            return Input.Max(d => d.Points);
        }

        public class Reindeer
        {
            public Reindeer(string desc)
            {
                var split = desc.Split(" ");
                Name = split[0];
                Speed = int.Parse(split[3]);
                SpeedTimeLimit = int.Parse(split[6]);
                RestTimeLimit = int.Parse(split[13]);
                RemainingTimeInState = SpeedTimeLimit;
            }

            public string Name { get; }
            public int Speed { get; }
            public int SpeedTimeLimit { get; }
            public int RestTimeLimit { get; }
            public bool Resting { get; set; } = false;
            public int RemainingTimeInState { get; set; }
            public int TotalDistance { get; set; } = 0;
            public int Points { get; set; }
        }
    }
}
