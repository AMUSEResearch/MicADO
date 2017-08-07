using System.Collections.Generic;
using MicADO.GeneticAlgorithm.Chromosome;
using MicADO.Models.Deployment;

namespace MicADO.GeneticAlgorithm.Populations
{
  public interface IPopulation
  {
    IEnumerable<IDeploymentChromosome> Deployments { get; }

    IDeploymentChromosome BestDeployment { get; }

    int Count { get; }

    int MinimalSize { get; }

    int MaximalSize { get; }

  }
}