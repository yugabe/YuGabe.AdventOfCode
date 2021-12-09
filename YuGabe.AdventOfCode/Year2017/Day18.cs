using System;
using System.Collections.Generic;
using System.Linq;
using Tidy.AdventOfCode;

namespace YuGabe.AdventOfCode.Year2017
{
    public class Day18 : Day
    {
        public override object ExecutePart1()
        {
            var instructions = Input.Split("\n").Select(r => r.Split(' ')).ToArray();
            long lastSound = 0;
            var registers = instructions.SelectMany(i => i).Where(i => i.Length == 1 && !long.TryParse(i, out _)).Distinct().ToDictionary(i => i, i => (long)0);
            long Value(string value) => long.TryParse(value, out var val) ? val : registers[value];
            for (long i = 0; i < instructions.Length && i >= 0;)
            {
                var inst = instructions[i];
                switch (inst[0])
                {
                    case "snd":
                        lastSound = Value(inst[1]);
                        break;
                    case "set":
                        registers[inst[1]] = Value(inst[2]);
                        break;
                    case "add":
                        registers[inst[1]] += Value(inst[2]);
                        break;
                    case "mul":
                        registers[inst[1]] *= Value(inst[2]);
                        break;
                    case "mod":
                        registers[inst[1]] %= Value(inst[2]);
                        break;
                    case "rcv":
                        if (Value(inst[1]) != 0)
                            return lastSound;
                        break;
                    case "jgz":
                        if (Value(inst[1]) > 0)
                        {
                            i += Value(inst[2]);
                            continue;
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }
                i++;
            }
            throw new InvalidOperationException();
        }

        public class Program
        {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            public Program(long id, string[][] instructions)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            {
                Id = id;
                Instructions = instructions;
                Registers = instructions.SelectMany(i => i).Where(i => i.Length == 1 && !long.TryParse(i, out _)).Distinct().ToDictionary(i => i, i => (long)0);
                Registers["p"] = id;
            }
            public Program OtherProgram { get; set; }
            public long Id { get; }
            public Dictionary<string, long> Registers { get; }
            public Queue<long> MessageQueue { get; } = new Queue<long>();
            public string[][] Instructions { get; }
            public long ProgramCounter { get; private set; } = 0;
            public long SendCounter { get; private set; } = 0;
            private long GetValue(string value) => long.TryParse(value, out var val) ? val : Registers[value];
            public bool TryExecuteOne()
            {
                if (ProgramCounter < 0 || Instructions.Length <= ProgramCounter)
                    return false;
                var inst = Instructions[ProgramCounter];
                switch (inst[0])
                {
                    case "snd":
                        OtherProgram.MessageQueue.Enqueue(GetValue(inst[1]));
                        SendCounter++;
                        break;
                    case "set":
                        Registers[inst[1]] = GetValue(inst[2]);
                        break;
                    case "add":
                        Registers[inst[1]] += GetValue(inst[2]);
                        break;
                    case "mul":
                        Registers[inst[1]] *= GetValue(inst[2]);
                        break;
                    case "mod":
                        Registers[inst[1]] %= GetValue(inst[2]);
                        break;
                    case "rcv":
                        if (!MessageQueue.TryDequeue(out var val))
                            return false;
                        Registers[inst[1]] = val;
                        break;
                    case "jgz":
                        if (GetValue(inst[1]) > 0)
                        {
                            ProgramCounter += GetValue(inst[2]);
                            return true;
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }
                ProgramCounter++;
                return true;
            }
        }

        public override object ExecutePart2()
        {
            var instructions = Input.Split("\n").Select(r => r.Split(' ')).ToArray();
            var p0 = new Program(0, instructions);
            var p1 = new Program(1, instructions);
            p0.OtherProgram = p1;
            p1.OtherProgram = p0;

            while (p0.TryExecuteOne() || p1.TryExecuteOne()) ;

            return p1.SendCounter;
        }
    }
}
