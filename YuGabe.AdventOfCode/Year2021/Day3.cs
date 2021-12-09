namespace YuGabe.AdventOfCode.Year2021;

public class Day3 : Day<bool[][]>
{
    public override object ExecutePart1()
    {
        var gamma = Enumerable.Range(0, Input[0].Length).Select(i => Input.Count(r => r[i]) > Input.Length / 2).ToInt();
        var epsilon = Math.Pow(2, Input[0].Length) - 1 - gamma;
        var consumption = gamma * epsilon;
        return consumption;
    }

    public override object ExecutePart2()
    {
        var (o2GeneratorNumbers, co2ScrubberNumbers) = (Input.ToArray(), Input.ToArray());

        for (var i = 0; i < Input[0].Length; i++)
        {
            if (o2GeneratorNumbers.Length * co2ScrubberNumbers.Length == 0)
                throw null!;

            if (o2GeneratorNumbers.Length > 1)
            {
                var value = o2GeneratorNumbers.Count(r => r[i]) >= o2GeneratorNumbers.Count(r => !r[i]);
                o2GeneratorNumbers = o2GeneratorNumbers.Where(r => r[i] == value).ToArray();
            }

            if (co2ScrubberNumbers.Length > 1)
            {
                var value = co2ScrubberNumbers.Count(r => r[i]) < co2ScrubberNumbers.Count(r => !r[i]);
                co2ScrubberNumbers = co2ScrubberNumbers.Where(r => r[i] == value).ToArray();
            }
        }

        return o2GeneratorNumbers.Single().ToInt() * co2ScrubberNumbers.Single().ToInt();
    }

    public override bool[][] ParseInput(string rawInput)
        => rawInput.SplitAtNewLines().Select(l => l.Select(c => c == '1').ToArray()).ToArray();
}
