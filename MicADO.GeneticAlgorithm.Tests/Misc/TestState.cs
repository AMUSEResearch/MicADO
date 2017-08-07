using System;
using MicADO.GeneticAlgorithm.Populations;
using MicADO.GeneticAlgorithm.State;

namespace MicADO.GeneticAlgorithm.Tests.Misc
{
  public class TestState : IGeneticAlgorithmState
  {
    public int Count { get; private set; }
    public void UpdateOnGenerationRan(IPopulation newPopulation)
    {
      Count++;
    }
  }
}