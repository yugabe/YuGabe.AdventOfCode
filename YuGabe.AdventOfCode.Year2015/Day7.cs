using System.Collections.Generic;
using System.Linq;
using Tidy.AdventOfCode;

#nullable disable
namespace YuGabe.AdventOfCode.Year2015
{
    public class Day7 : Day<Dictionary<string, Day7.Wire>>
    {
        public override Dictionary<string, Wire> ParseInput(string input)
        {
            var constants = new List<Wire>();
            string GetTokenId(string id)
            {
                if (!constants.Any(c => c.Id == id) && ushort.TryParse(id, out var value))
                {
                    var constant = new Constant { Id = id, ConstantValue = value };
                    constants.Add(constant);
                }
                return id;
            }

            var values = //"123 -> x\n456 -> y\nx AND y -> d\nx OR y -> e\nx LSHIFT 2 -> f\ny RSHIFT 2 -> g\nNOT x -> h\nNOT y -> i"
                input.Split('\n').Select(r =>
            {
                var tokens = r.Split(' ');
                Wire wire = null;
                if (tokens.Any(t => t == "NOT"))
                    wire = new Not { NId = GetTokenId(tokens[1]) };
                else if (tokens.Any(t => t == "AND"))
                    wire = new And { AId = GetTokenId(tokens[0]), BId = GetTokenId(tokens[2]) };
                else if (tokens.Any(t => t == "OR"))
                    wire = new Or { AId = GetTokenId(tokens[0]), BId = GetTokenId(tokens[2]) };
                else if (tokens.Any(t => t == "LSHIFT"))
                    wire = new LShift { IId = GetTokenId(tokens[0]), ShiftValue = ushort.Parse(tokens[2]) };
                else if (tokens.Any(t => t == "RSHIFT"))
                    wire = new RShift { IId = GetTokenId(tokens[0]), ShiftValue = ushort.Parse(tokens[2]) };
                else
                    wire = ushort.TryParse(tokens[0], out var value) ? (Wire)new Constant { ConstantValue = value } : new Redirect { RId = tokens[0] };
                wire.Id = tokens.Last();
                wire.Raw = r;
                return wire;
            }).Concat(constants).ToDictionary(w => w.Id);

            foreach (var value in values)
                value.Value.Wires = values;

            return values;
        }

        public override object ExecutePart1()
        {
            return Input["a"].GetValue();
        }

        public override object ExecutePart2()
        {
            Input["b"] = new Constant { ConstantValue = 16076 };
            return Input["a"].GetValue();
        }

        public abstract class Wire
        {
            public Dictionary<string, Wire> Wires { get; set; }
            public string Id { get; set; }
            private ushort? Cached;
            public abstract ushort Value { get; }
            public string Raw { get; set; }
            public override string ToString() => $"{GetType().Name} {Id} [{Raw}]";
            public virtual ushort GetValue()
            {
                if (Cached == null)
                    return (Cached = Value).Value;
                else
                    return Cached.Value;
            }
        }

        public class Constant : Wire
        {
            public ushort ConstantValue { get; set; }
            public override ushort Value => ConstantValue;
            public override string ToString() => $"{base.ToString()} (={ConstantValue})";
        }

        public class Redirect : Wire
        {
            public string RId { get; set; }
            public Wire RWire => Wires[RId];
            public override ushort Value => RWire.GetValue();
            public override string ToString() => $"{base.ToString()} (->{RId})";
        }

        public class Not : Wire
        {
            public string NId { get; set; }
            public Wire NWire => Wires[NId];
            public override ushort Value => (ushort)~NWire.GetValue();
            public override string ToString() => $"{base.ToString()} (~{NId})";
        }

        public class And : Wire
        {
            public string AId { get; set; }
            public Wire AWire => Wires[AId];
            public string BId { get; set; }
            public Wire BWire => Wires[BId];
            public override ushort Value => (ushort)(AWire.GetValue() & BWire.GetValue());
            public override string ToString() => $"{base.ToString()} ({AId}&{BId})";
        }

        public class Or : Wire
        {
            public string AId { get; set; }
            public Wire AWire => Wires[AId];
            public string BId { get; set; }
            public Wire BWire => Wires[BId];
            public override ushort Value => (ushort)(AWire.GetValue() | BWire.GetValue());
            public override string ToString() => $"{base.ToString()} ({AId}|{BId})";
        }

        public class LShift : Wire
        {
            public string IId { get; set; }
            public Wire IWire => Wires[IId];
            public ushort ShiftValue { get; set; }
            public override ushort Value => (ushort)(IWire.GetValue() << ShiftValue);
            public override string ToString() => $"{base.ToString()} ({IId}<<{ShiftValue})";
        }

        public class RShift : Wire
        {
            public string IId { get; set; }
            public Wire IWire => Wires[IId];
            public ushort ShiftValue { get; set; }
            public override ushort Value => (ushort)(IWire.GetValue() >> ShiftValue);
            public override string ToString() => $"{base.ToString()} ({IId}>>{ShiftValue})";
        }
    }
}
