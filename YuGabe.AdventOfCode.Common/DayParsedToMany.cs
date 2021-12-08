namespace YuGabe.AdventOfCode;

public abstract class DayParsedToMany<T> : Day<T[]>
{
    public override T[] ParseInput(string rawInput) => rawInput.ToMany<T>();
}
