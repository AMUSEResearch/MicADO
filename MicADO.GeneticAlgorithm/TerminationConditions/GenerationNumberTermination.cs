using System;
using MicADO.GeneticAlgorithm.Populations;
using MicADO.GeneticAlgorithm.State;

namespace MicADO.GeneticAlgorithm.TerminationConditions
{
  public class GenerationNumberTermination : ITerminationCondition<GenerationCountState>
  {
    public int MaximalNumberOfGenerations { get; }

    public GenerationNumberTermination(int maximalNumberOfGenerations)
    {
      MaximalNumberOfGenerations = maximalNumberOfGenerations;
    }

    public bool HasReached(IPopulation population, GenerationCountState state)
    {
      return state.GenerationCount >= MaximalNumberOfGenerations;
    }
  }
}