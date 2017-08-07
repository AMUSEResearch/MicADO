using System.Collections.Generic;
using MicADO.GeneticAlgorithm.Chromosome;

namespace MicADO.GeneticAlgorithm.Crossovers
{
  public interface ICrossover
  {
    int ParentsNumber { get; }

    int ChildrenNumber { get; }

    IEnumerable<IDeploymentChromosome> Cross(IEnumerable<IDeploymentChromosome> parents);
  }
}