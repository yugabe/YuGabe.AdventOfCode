namespace YuGabe.AdventOfCode.Year2015;
public class Day20 : Day<int>
{
    public override int ParseInput(string rawInput) => int.Parse(rawInput);

    public int Execute(Func<int, int, int> deliverPresents)
    {
        var house = 0;
        while (true)
        {
            var presents = 0;
            for (var elf = 1; elf <= Math.Sqrt(house); elf++)
                if (house % elf == 0)
                    presents += deliverPresents(house, elf);
            if (presents > Input)
                return house;
            house += 20;
        }
    }

    public override object ExecutePart1()
        => Execute((house, elf) => (elf + (house / elf) is var otherElf && elf != otherElf ? otherElf : 0) * 10);

    public override object ExecutePart2() 
        => Execute((house, elf) => house / elf is var otherElf ? ((otherElf <= 50 ? elf : 0) + (house / otherElf <= 50 && elf != otherElf ? otherElf : 0)) * 11 : 0);
}
