namespace YuGabe.AdventOfCode.Common;
public class PrimeNumbers
{
    public PrimeNumbers(long initialPrimeUpperBounds = 23)
    {
        GeneratePrimes(initialPrimeUpperBounds);
    }

    private long _highestPrimeYet = 23;
    private readonly List<long> _primeList = new() { 2, 3, 5, 7, 11, 13, 17, 19, 23 };
    private readonly HashSet<long> _primeSet = new() { 2, 3, 5, 7, 11, 13, 17, 19, 23 };
    public IReadOnlySet<long> PrimeSet => _primeSet;
    public IReadOnlyList<long> PrimeList => _primeList.AsReadOnly();

    public bool IsPrime(long number)
    {
        if (_highestPrimeYet <= number + 1)
        {
            return _primeSet.Contains(number);
        }
        else
        {
            GeneratePrimes(number);
            return _primeSet.Contains(number);
        }
    }

    public IEnumerable<long> GetPrimeFactors(long number)
    {
        if (number < 0)
            throw new ArgumentOutOfRangeException(nameof(number));
        else if (number == 0)
            yield break;
        else if (number == 1)
            yield return 1;
        else if (_primeSet.Contains(number))
            yield return number;
        else
        {
            var sqrt = (long)Math.Sqrt(number);
            GeneratePrimes(sqrt);
            foreach (var prime in _primeList)
            {
                if (prime > sqrt)
                    yield break;
                if ((number % prime) == 0)
                {
                    yield return prime;
                    foreach (var lower in GetPrimeFactors(number / prime))
                        yield return lower;
                    yield break;
                }
            }
            yield return number;
        }
    }

    public void GeneratePrimes(long upperBounds)
    {
        for (var candidate = _highestPrimeYet + 2; candidate <= upperBounds; candidate += 2)
        {
            if (candidate % 3 != 0 && candidate % 5 != 0 && candidate % 7 != 0 && candidate % 11 != 0 && candidate % 13 != 0 && candidate % 17 != 0 && candidate % 19 != 0 && candidate % 23 != 0 && _primeList.All(p => (candidate % p) != 0))
            {
                _highestPrimeYet = candidate;
                _primeList.Add(candidate);
                _primeSet.Add(candidate);
            }
        }
    }
}
