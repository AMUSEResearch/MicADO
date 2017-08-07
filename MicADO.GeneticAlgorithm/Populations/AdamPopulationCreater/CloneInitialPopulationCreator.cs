using System.Linq;
using MicADO.GeneticAlgorithm.Chromosome;

namespace MicADO.GeneticAlgorithm.Populations.AdamPopulationCreater
{
  /// <summary>
  /// Creates an initial population by cloning the initial deployment
  /// </summary>
  public class CloneInitialPopulationCreator : IInitialPopulationCreator
  {
    public IPopulation CreateInitialPopulation(IDeploymentChromosome initialDeployment, int minimalSize, int maximalSize)
    {
      return new Population(Enumerable.Repeat(initialDeployment, minimalSize), minimalSize, maximalSize);
    }
  }
}