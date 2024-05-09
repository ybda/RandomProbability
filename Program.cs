using Newtonsoft.Json;

namespace RandomProbability;

public static class Program
{
    private static void Main()
    {
        var generatedNumbers = GenerateNumbers();

        var frequencyMap = Util.GetFrequencyMap(generatedNumbers).OrderBy(kvp => kvp.Key).ToDictionary();
        var frequencyPercentageMap = Util.GetFrequencyPercentageMap(generatedNumbers).OrderBy(kvp => kvp.Key).ToDictionary();
        
        string json = JsonConvert.SerializeObject(frequencyMap, Formatting.Indented);
        Console.WriteLine(json);
        
        json = JsonConvert.SerializeObject(frequencyPercentageMap, Formatting.Indented);
        Console.WriteLine(json);
    }

    private static List<int> GenerateNumbers()
    {
        var randomGenerator = new ProbabilityWeightedRandomGenerator(0, 9, new Dictionary<int, double> {
            { 2, 0.005 },
            { 3, 0.4 },
            { 4, 0.5 },
        });

        return Enumerable.Range(0, 100_000).Select(_ => randomGenerator.Next()).ToList();
    }
}