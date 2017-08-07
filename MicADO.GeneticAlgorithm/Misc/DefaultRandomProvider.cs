using System;
using System.Diagnostics.CodeAnalysis;

namespace MicADO.GeneticAlgorithm.Misc
{
  /// <summary>
  /// Wraps System.Random as IRandomProvider
  /// </summary>
  [ExcludeFromCodeCoverage]
  public class DefaultRandomProvider : IRandomProvider
  {
    private Random _randomGenerator = new Random();
    public double GetRandom() => _randomGenerator.NextDouble();

    public int GetRandom(int lower, int upper) => _randomGenerator.Next(lower, upper);
  }
}