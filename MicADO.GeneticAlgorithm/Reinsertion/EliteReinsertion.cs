using System;
using System.Collections.Generic;
using System.Linq;
using MicADO.GeneticAlgorithm.Chromosome;
using MicADO.GeneticAlgorithm.Populations;

namespace MicADO.GeneticAlgorithm.Reinsertion
{
  public class EliteReinsertion : IReinsertionStrategy
  {
    public IPopulation Reinsert(IEnumerable<IDeploymentChromosome> offspring, IPopulation oldPopulation)
    {
      var partOfOldPopulationToMaintain = oldPopulation.MinimalSize - offspring.Count();
      var newPopulation = offspring.ToList();
      if(partOfOldPopulationToMaintain > 0)
      {
        var bestParents = oldPopulation.Deployments.OrderByDescending(p => p.Fitness).Take(partOfOldPopulationToMaintain);

        foreach(var p in bestParents)
        {
          newPopulation.Add(p);
        }
      }

      return new Population(newPopulation, oldPopulation.MinimalSize, oldPopulation.MaximalSize);
    }
  }
}