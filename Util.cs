namespace RandomProbability;

public static class Util
{
    // key - number, value - how many times
    public static Dictionary<int, int> GetFrequencyMap(ICollection<int> numbers)
    {
        return numbers.GroupBy(n => n)
            .ToDictionary(g => g.Key, g => g.Count());
    }
    
    public static Dictionary<int, double> GetFrequencyPercentageMap(ICollection<int> numbers)
    {
        var frequencyMap = GetFrequencyMap(numbers);
        int totalNumbers = numbers.Count;

        return frequencyMap.ToDictionary(
            f => f.Key,
            f => (double)f.Value / totalNumbers);
    }
}