using System;
using System.Collections.Generic;
using System.Linq;
using MicADO.GeneticAlgorithm.Chromosome;

namespace MicADO.GeneticAlgorithm.Populations
{
  public class Population : IPopulation
  {
    public IEnumerable<IDeploymentChromosome> Deployments { get; }

    public IDeploymentChromosome BestDeployment => Deployments.OrderByDescending(d => d.Fitness).First();

    public int Count { get; }

    public int MinimalSize { get; }

    public int MaximalSize { get; }

    public Population(IEnumerable<IDeploymentChromosome> deployments, int minimalSize, int maximalSize)
    {
      Deployments = deployments;
      Count = deployments.Count();
      MinimalSize = minimalSize;
      MaximalSize = maximalSize;
    }
  }
}