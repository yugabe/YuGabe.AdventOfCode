using System.Collections.Generic;
using System.Linq;
using Tidy.AdventOfCode;

namespace YuGabe.AdventOfCode.Year2017
{
    public class Day16 : Day
    {
        public override object ExecutePart1()
        {
            var state = Enumerable.Range((int)'a', 16).Select(n => (char)n).ToArray();
            foreach (var instruction in Input.Trim().Split(','))
            {
                if (!instruction.Contains('/'))
                {
                    var pos = int.Parse(new string(instruction.Skip(1).ToArray()));
                    state = state.Skip(state.Length - pos).Concat(state.Take(state.Length - pos)).ToArray();
                }
                else
                {
                    var from = instruction[0] == 'x' ? int.Parse(new string(instruction.Skip(1).TakeWhile(c => c != '/').ToArray())) : new string(state).IndexOf(instruction[1]);
                    var to = instruction[0] == 'x' ? int.Parse(new string(instruction.SkipWhile(c => c != '/').Skip(1).ToArray())) : new string(state).IndexOf(instruction[3]);
                    var oldFrom = state[from];
                    state[from] = state[to];
                    state[to] = oldFrom;
                }
            }
            return new string(state);
        }

        public override object ExecutePart2()
        {
            var state = Enumerable.Range((int)'a', 16).Select(n => (char)n).ToArray();
            var instructions = Input.Trim().Split(',');
            var states = new HashSet<string>();
            var currentCycle = 0;

            while (currentCycle != 1_000_000_000)
            {
                if (!states.TryGetValue(new string(state.ToArray()), out var _))
                {
                    states.Add(new string(state.ToArray()));
                    currentCycle++;
                    Cycle();
                }
                else
                {
                    var period = currentCycle;
                    while (currentCycle + period < 1_000_000_000)
                        currentCycle += period;
                    states.Clear();
                }
            }

            return new string(state.ToArray());

            string Cycle()
            {
                foreach (var instruction in instructions)
                {
                    if (!instruction.Contains('/'))
                    {
                        var pos = int.Parse(new string(instruction.Skip(1).ToArray()));
                        state = state.Skip(state.Length - pos).Concat(state.Take(state.Length - pos)).ToArray();
                    }
                    else
                    {
                        var from = instruction[0] == 'x' ? int.Parse(new string(instruction.Skip(1).TakeWhile(c => c != '/').ToArray())) : new string(state).IndexOf(instruction[1]);
                        var to = instruction[0] == 'x' ? int.Parse(new string(instruction.SkipWhile(c => c != '/').Skip(1).ToArray())) : new string(state).IndexOf(instruction[3]);
                        var oldFrom = state[from];
                        state[from] = state[to];
                        state[to] = oldFrom;
                    }
                }
                return new string(state);
            }
        }
    }
}
