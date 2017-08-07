using System.Collections.Generic;
using MicADO.GeneticAlgorithm.Chromosome;
using MicADO.GeneticAlgorithm.Populations;

namespace MicADO.GeneticAlgorithm.Reinsertion
{
  public interface IReinsertionStrategy
  {
    IPopulation Reinsert(IEnumerable<IDeploymentChromosome> offspring, IPopulation oldPopulation);
  }
}