using System.Collections.Generic;
using MicADO.GeneticAlgorithm.Chromosome;
using MicADO.GeneticAlgorithm.Populations;
using MicADO.Models.Deployment;

namespace MicADO.GeneticAlgorithm.Selections
{
  public interface ISelectionStrategy
  {
    IEnumerable<IDeploymentChromosome> SelectChromosomes(int number, IPopulation population);
  }
}