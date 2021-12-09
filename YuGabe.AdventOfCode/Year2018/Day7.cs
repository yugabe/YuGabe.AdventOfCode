namespace YuGabe.AdventOfCode.Year2018
{
    public class Day7 : Day<Day7.Instruction[]>
    {
        public override Instruction[] ParseInput(string input)
            => input.Trim().Split('\n').Select(l => new Instruction(l)).ToArray();

        public override object ExecutePart1()
        {
            var dict = Input.SelectMany(p => new[] { p.Letter, p.Prerequisite }).Distinct().ToDictionary(e => e, e => new HashSet<char>());
            foreach (var item in Input)
                dict[item.Letter].Add(item.Prerequisite);

            var result = "";
            while (dict.Any())
            {
                var next = dict.Where(d => !d.Value.Any()).Min(d => d.Key);
                dict.Remove(next);
                foreach (var item in dict)
                    item.Value.Remove(next);
                result += next;
            }
            return result;
        }

        public class Worker
        {
            public char? Step;
            public int? RemainingTime;
        }

        public override object ExecutePart2()
        {
            var dict = Input.SelectMany(p => new[] { p.Letter, p.Prerequisite }).Distinct().ToDictionary(e => e, e => new HashSet<char>());
            foreach (var item in Input)
                dict[item.Letter].Add(item.Prerequisite);

            var workers = Enumerable.Range(1, 5).Select(_ => new Worker()).ToList();
            var totalTime = 0;

            while (dict.Any() || workers.Any(w => w.Step != null))
            {
                var ready = dict.Where(d => !d.Value.Any()).OrderBy(e => e.Key).ToList();
                if (ready.Any() && workers.Any(w => w.Step == null))
                {
                    var worker = workers.First(w => w.Step == null);
                    worker.Step = ready[0].Key;
                    worker.RemainingTime = 60 + 1 + ready[0].Key - 'A';
                    dict.Remove(ready[0].Key);
                }
                else
                {
                    var worker = workers.Where(w => w.Step != null).OrderBy(w => w.RemainingTime).First();
                    var time = worker.RemainingTime!.Value;
                    worker.RemainingTime = null;
                    totalTime += time;

                    foreach (var w in workers.Where(n => n.RemainingTime != null))
                        w.RemainingTime -= time;

                    if (workers.Any(w => w.RemainingTime == 0))
                        throw new ApplicationException(); // Multiple elves finished at the same time.

                    foreach (var item in dict)
                        item.Value.Remove(worker.Step!.Value);
                    worker.Step = null;
                }
            }
            return totalTime;
        }

        public class Instruction
        {
            public Instruction(string rawLine)
            {
                var items = rawLine.Split();
                Prerequisite = items[1][0];
                Letter = items[^3][0];
                TotalTimeToComplete = 60 + (Letter - 'A');
            }
            public readonly char Letter;
            public readonly char Prerequisite;
            public readonly int TotalTimeToComplete;
        }
    }
}
