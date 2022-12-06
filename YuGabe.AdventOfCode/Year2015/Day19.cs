using System.Text.RegularExpressions;

namespace YuGabe.AdventOfCode.Year2015;
public class Day19 : Day<(ILookup<string, string> Replacements, string Molecule)>
{
    public override (ILookup<string, string> Replacements, string Molecule) ParseInput(string rawInput) => rawInput.Split("\n\n").FeedTo(parts => (parts[0].Split('\n').Select(r => r.Split(" => ")).ToLookup(r => r[0], r => r[1]), parts[1]));

    public override object ExecutePart1() => GenerateMolecules(Input.Molecule, Input.Replacements).ToHashSet().Count;

    private static IEnumerable<string> GenerateMolecules(string originMolecule, ILookup<string, string> replacements)
    {
        foreach (var (key, value) in replacements.SelectMany(g => g.Select(v => (g.Key, v))))
        {
            for (var i = 0; i <= originMolecule.Length - key.Length; i++)
            {
                if (originMolecule[i..(i + key.Length)] == key)
                    yield return originMolecule[..i] + value + originMolecule[(i + key.Length)..];
            }
        }
    }

    private static IEnumerable<(string Molecule, int Step)> GenerateAllMolecules(string originMolecule, ILookup<string, string> replacements, int currentStep) =>
        GenerateMolecules(originMolecule, replacements).SelectMany(generated => GenerateAllMolecules(generated, replacements, currentStep + 1).Prepend((generated, currentStep + 1)));

    public override object ExecutePart2() => 
        GenerateAllMolecules(Input.Molecule, Input.Replacements.SelectMany(e => e.Select(r => (key: r, value: e.Key))).ToLookup(e => e.key, e => e.value), 0).First(m => m.Molecule == "e").Step;
}
