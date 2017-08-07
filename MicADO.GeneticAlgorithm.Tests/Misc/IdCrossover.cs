using System.Collections.Generic;
using MicADO.GeneticAlgorithm.Chromosome;
using MicADO.GeneticAlgorithm.Crossovers;

namespace MicADO.GeneticAlgorithm.Tests.Misc
{
  internal class IdCrossover : ICrossover
  {
    public int ParentsNumber => 2;

    public int ChildrenNumber => 2;

    public IEnumerable<IDeploymentChromosome> Cross(IEnumerable<IDeploymentChromosome> parents)
    {
      return parents;
    }
  }
}