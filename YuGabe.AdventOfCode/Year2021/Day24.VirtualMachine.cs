namespace YuGabe.AdventOfCode.Year2021;

public partial class Day24 : Day<Day24.Instruction[]>
{
    public abstract record Parameter { }
    public record VariableParameter(char Name) : Parameter
    {
        public override string ToString() => Name.ToString();
        public static implicit operator VariableParameter(char value) => new(value);
        public static implicit operator char(VariableParameter value) => value.Name;
    }
    public record ConstantParameter(long Value) : Parameter
    {
        public override string ToString() => Value.ToString();
    }
    public abstract record Instruction
    {
        public abstract void Execute(MachineState state, Queue<long> input);
        public static Instruction Parse(string instruction)
        {
            var (token1, token2, token3) = instruction.SplitToTuple3();
            return token1 switch
            {
                "inp" => new Inp(new(token2!.Single())),
                "add" => new Add(ToParameter(token2), ToParameter(token3)),
                "mul" => new Mul(ToParameter(token2), ToParameter(token3)),
                "div" => new Div(ToParameter(token2), ToParameter(token3)),
                "mod" => new Mod(ToParameter(token2), ToParameter(token3)),
                "eql" => new Eql(ToParameter(token2), ToParameter(token3)),
                _ => throw new InvalidOperationException()
            };
            static Parameter ToParameter(string? rawValue) => long.TryParse(rawValue ?? throw new ArgumentException(null, rawValue), out var longValue) ? new ConstantParameter(longValue) : new VariableParameter(rawValue.Single());
        }
    }
    public abstract record BinaryStoreInstruction(Parameter Left, Parameter Right) : Instruction
    {
        public abstract long ExecuteBinaryOperation(long leftValue, long rightValue);
        public override void Execute(MachineState state, Queue<long> input) => state[Left is VariableParameter variable ? variable : throw new InvalidOperationException()] = ExecuteBinaryOperation(state[Left], state[Right]);
        public sealed override string ToString() => $"{GetType().Name.ToLower()} {Left} {Right}";
    }
    public record Inp(VariableParameter Variable) : Instruction
    {
        public override void Execute(MachineState state, Queue<long> input) => state[Variable] = input.Dequeue();
        public override string ToString() => $"inp {Variable}";
    }
    public record Add(Parameter Left, Parameter Right) : BinaryStoreInstruction(Left, Right)
    {
        public override long ExecuteBinaryOperation(long leftValue, long rightValue) => leftValue + rightValue;
    }
    public record Mul(Parameter Left, Parameter Right) : BinaryStoreInstruction(Left, Right)
    {
        public override long ExecuteBinaryOperation(long leftValue, long rightValue) => leftValue * rightValue;
    }
    public record Div(Parameter Left, Parameter Right) : BinaryStoreInstruction(Left, Right)
    {
        public override long ExecuteBinaryOperation(long leftValue, long rightValue) => leftValue / rightValue;
    }
    public record Mod(Parameter Left, Parameter Right) : BinaryStoreInstruction(Left, Right)
    {
        public override long ExecuteBinaryOperation(long leftValue, long rightValue) => leftValue % rightValue;
    }
    public record Eql(Parameter Left, Parameter Right) : BinaryStoreInstruction(Left, Right)
    {
        public override long ExecuteBinaryOperation(long leftValue, long rightValue) => leftValue == rightValue ? 1 : 0;
    }

    public class MachineState
    {
        private Dictionary<VariableParameter, long> State { get; } = new[] { 'w', 'x', 'y', 'z' }.ToDictionary(c => new VariableParameter(c), _ => 0L);
        public long this[VariableParameter parameter] { get => State.TryGetValue(parameter, out var value) ? value : 0; set => State[parameter] = value; }
        public long this[Parameter parameter] => parameter is VariableParameter variable ? this[variable] : parameter is ConstantParameter constant ? constant.Value : throw new ArgumentException("Invalid parameter type.", nameof(parameter));
        public override string ToString() => string.Join(", ", State.OrderBy(kv => (char)kv.Key).Select(kv => $"[{kv.Key}] = {kv.Value,-12}"));
    }

    public record Machine(IReadOnlyCollection<Instruction> Instructions)
    {
        public void Execute(MachineState state, Queue<long> input)
        {
            foreach (var instruction in Instructions)
                instruction.Execute(state, input);
        }
    }

    public string RawInput { get; private set; } = "";
    public override Instruction[] ParseInput(string rawInput) => (RawInput = rawInput).SplitAtNewLines().Select(Instruction.Parse).ToArray();

    public static bool Validate(Machine monadValidator, string modelNumber)
    {
        if (modelNumber?.Length != 14)
            return false;
        var state = new MachineState();
        var input = new Queue<long>(modelNumber.Select(c => long.Parse(c.ToString())).ToArray());
        monadValidator.Execute(state, input);
        return state['z'] == 0;
    }
}
