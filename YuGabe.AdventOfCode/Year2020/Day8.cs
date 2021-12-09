namespace YuGabe.AdventOfCode.Year2020
{
    public class Day8 : Day<Day8.Instruction[]>
    {
        public enum InstructionType
        {
            Acc,
            Nop,
            Jmp
        }

        public record Instruction(InstructionType Type, int Value) { }

        public class MachineState
        {
            public int ProgramCounter { get; set; }
            public int Accumulator { get; set; }
        }

        public override Instruction[] ParseInput(string rawInput) => rawInput.Split('\n').Select(line => line.Split(' ')).Select(tokens => new Instruction(Enum.Parse<InstructionType>(tokens[0], true), int.Parse(tokens[1]))).ToArray();

        public override object ExecutePart1() => ExecuteProgram(Input);

        private static int ExecuteProgram(Instruction[] instructions)
        {
            var state = new MachineState();

            var visited = new HashSet<int>();

            while (true)
            {
                if (!visited.Add(state.ProgramCounter))
                    return state.Accumulator;
                RunInstruction(state, instructions[state.ProgramCounter]);
                if (state.ProgramCounter > instructions.Length - 1)
                    throw new SuccessfulExecutionException(state);
                if (state.ProgramCounter < 0)
                    throw new IndexOutOfRangeException();
            }
        }

        public sealed class SuccessfulExecutionException : Exception
        {
            public SuccessfulExecutionException(MachineState state)
            {
                State = state;
            }

            public MachineState State { get; }
        }

        public override object ExecutePart2()
        {
            foreach(var (instruction, index) in Input.WithIndexes())
            {
                if (instruction.Type is InstructionType.Jmp or InstructionType.Nop)
                {
                    try
                    {
                        var input = Input.ToArray();
                        input[index] = new Instruction(instruction.Type is InstructionType.Jmp ? InstructionType.Nop : InstructionType.Jmp, instruction.Value);
                        ExecuteProgram(input);
                    }
                    catch (SuccessfulExecutionException s)
                    {
                        return s.State.Accumulator;
                    }
                    catch (IndexOutOfRangeException) { }
                }
            }
            throw new InvalidOperationException();
        }

        private static void RunInstruction(MachineState state, Instruction instruction) =>
            (state.ProgramCounter, state.Accumulator) = instruction switch
            {
                { Type: InstructionType.Acc } => (state.ProgramCounter + 1, state.Accumulator + instruction.Value),
                { Type: InstructionType.Jmp } => (state.ProgramCounter + instruction.Value, state.Accumulator),
                { Type: InstructionType.Nop } => (state.ProgramCounter + 1, state.Accumulator),
                _ => throw new InvalidOperationException()
            };
    }
}
