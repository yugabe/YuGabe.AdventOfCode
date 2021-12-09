#nullable disable
namespace YuGabe.AdventOfCode.Year2018
{
    public class Day4 : Day<Day4.LogEntry[]>
    {
        public override LogEntry[] ParseInput(string input)
            => input.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(l => new LogEntry
            {
                Date = DateTime.Parse(l.Substring(1, 16)),
                Action = string.Join(" ", l.Split()[^2..^0]),
                GuardId = l[25] == '#' ? int.Parse(new string(l[26..].TakeWhile(c => c != ' ').ToArray())) : (int?)null
            }).ToArray();

        public override object ExecutePart1()
        {
            var guardsSleepingMinutes = Input.Where(e => e.GuardId != null).Select(e => e.GuardId).Distinct().ToDictionary(e => e, e => new int[60]);
            int guardId = -1;
            DateTime startedSleeping = default;
            foreach (var item in Input.OrderBy(p => p.Date))
            {
                switch (item.Action)
                {
                    case LogEntry.ActionTypes.BeginsShift:
                        guardId = item.GuardId.Value;
                        break;
                    case LogEntry.ActionTypes.FallsAsleep:
                        startedSleeping = item.Date;
                        break;
                    case LogEntry.ActionTypes.WakesUp:
                        for (var minute = startedSleeping.Minute; minute <= item.Date.Minute; minute++)
                            guardsSleepingMinutes[guardId][minute]++;
                        break;
                    default:
                        throw new ApplicationException();
                }
            }

            var max = guardsSleepingMinutes.OrderByDescending(e => e.Value.Sum()).First();
            return max.Key * max.Value.Select((e, i) => (e, i)).OrderByDescending(e => e.e).First().i;
        }

        public override object ExecutePart2()
        {
            var guardsSleepingMinutes = Input.Where(e => e.GuardId != null).Select(e => e.GuardId).Distinct().ToDictionary(e => e, e => new int[60]);
            int guardId = -1;
            DateTime startedSleeping = default;
            foreach (var item in Input.OrderBy(p => p.Date))
            {
                switch (item.Action)
                {
                    case LogEntry.ActionTypes.BeginsShift:
                        guardId = item.GuardId.Value;
                        break;
                    case LogEntry.ActionTypes.FallsAsleep:
                        startedSleeping = item.Date;
                        break;
                    case LogEntry.ActionTypes.WakesUp:
                        for (var minute = startedSleeping.Minute; minute <= item.Date.Minute; minute++)
                            guardsSleepingMinutes[guardId][minute]++;
                        break;
                    default:
                        throw new ApplicationException();
                }
            }
            var flat = guardsSleepingMinutes.SelectMany(g => g.Value.Select((v, i) => (guard: g.Key, minute: i, value: v)));
            var res = flat.OrderByDescending(f => f.value).First();
            return res.guard * res.minute; // too low: 3203
        }

        public class LogEntry
        {
            public DateTime Date;
            public int? GuardId;
            public string Action;
            public struct ActionTypes
            {
                public const string FallsAsleep = "falls asleep";
                public const string BeginsShift = "begins shift";
                public const string WakesUp = "wakes up";
            }
        }
    }
}
