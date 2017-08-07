using System.Collections.Generic;
using MicADO.GeneticAlgorithm.Chromosome;

namespace MicADO.GeneticAlgorithm.Mutations
{
  public interface IMutation
  {
   IDeploymentChromosome Mutate(IDeploymentChromosome deployment);
  }
}