namespace RandomProbability;

public class ProbabilityWeightedRandomGenerator
{
    private readonly Random _random;
    private readonly IReadOnlyDictionary<int, double> _probabilityWeights;

    public ProbabilityWeightedRandomGenerator(int minNumber, int maxNumber, IReadOnlyDictionary<int, double> probabilities)
    {
        ValidateProbabilities(probabilities);
        _random = new Random();
        _probabilityWeights = CreateProbabilityWeights(minNumber, maxNumber, probabilities);
    }

    private static void ValidateProbabilities(IReadOnlyDictionary<int, double> probabilities)
    {
        if (probabilities.Values.Sum() > 1.0)
        {
            throw new ArgumentException("Probabilities must not exceed 1", nameof(probabilities));
        }
    }

    private static Dictionary<int, double> CreateProbabilityWeights(int minNumber, int maxNumber, IReadOnlyDictionary<int, double> probabilities)
    {
        var probabilityWeights = new Dictionary<int, double>();

        // Calculate the remaining probability
        double remainingProbability = 1.0;
        foreach (var (value, probability) in probabilities)
        {
            if (value < minNumber || value > maxNumber)
            {
                throw new ArgumentException($"Value {value} is out of range [{minNumber}, {maxNumber}]", nameof(probabilities));
            }
            remainingProbability -= probability;
            probabilityWeights.Add(value, probability);
        }

        // Distribute the remaining probability evenly among the rest of the numbers
        int remainingValuesCount = maxNumber - minNumber + 1 - probabilities.Count;
        double remainingProbabilityPerValue = remainingProbability / remainingValuesCount;
        for (int i = minNumber; i <= maxNumber; i++)
        {
            if (!probabilityWeights.TryGetValue(i, out _))
            {
                probabilityWeights.Add(i, remainingProbabilityPerValue);
            }
        }

        return probabilityWeights;
    }

    public int Next()
    {
        var randomValue = _random.NextDouble();
        double cumulativeProbability = 0.0;
        foreach (var (value, probability) in _probabilityWeights)
        {
            cumulativeProbability += probability;
            if (cumulativeProbability >= randomValue)
            {
                return value;
            }
        }
        throw new InvalidOperationException("Should not reach this point");
    }
}