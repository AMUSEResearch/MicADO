using System;
using MicADO.GeneticAlgorithm.Populations;

namespace MicADO.GeneticAlgorithm.State
{
  public class GenerationCountState : IGeneticAlgorithmState
  {
    public int GenerationCount { get; private set; }

    public void UpdateOnGenerationRan(IPopulation newPopulation)
    {
      GenerationCount++;
    }
  }
}