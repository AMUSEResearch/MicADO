namespace MicADO.GeneticAlgorithm.Misc
{
  public interface IRandomProvider
  {
    /// <summary>
    /// Returns a random floating-point number that is greater than or equal to 0.0, and less than 1.0.
    /// </summary>
    /// <returns></returns>
    double GetRandom();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="lower">The inclusive lower bound of the random number returned.</param>
    /// <param name="upper">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
    /// <returns></returns>
    int GetRandom(int lower, int upper);
  }
}