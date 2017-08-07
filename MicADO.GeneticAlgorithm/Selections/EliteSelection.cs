using System;
using System.Collections.Generic;
using System.Linq;
using MicADO.GeneticAlgorithm.Chromosome;
using MicADO.GeneticAlgorithm.Populations;

namespace MicADO.GeneticAlgorithm.Selections
{
  public class EliteSelection : ISelectionStrategy
  {
    public IEnumerable<IDeploymentChromosome> SelectChromosomes(int number, IPopulation population)
    {
      return population.Deployments.OrderByDescending(d => d.Fitness).Take(number);
    }
  }
}