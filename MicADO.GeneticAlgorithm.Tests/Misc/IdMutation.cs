using System;
using System.Linq;
using MicADO.GeneticAlgorithm.Chromosome;
using MicADO.GeneticAlgorithm.Mutations;

namespace MicADO.GeneticAlgorithm.Tests.Misc
{
  public class IdMutation : IMutation
  {
    public IDeploymentChromosome Mutate(IDeploymentChromosome deployment)
    {
      return new DeploymentChromosome(deployment.FeatureModel, deployment.Genes.ToArray());
    }
  }
}