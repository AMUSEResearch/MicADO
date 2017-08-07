using MicADO.GeneticAlgorithm.Chromosome;
using MicADO.Models.Deployment;

namespace MicADO.GeneticAlgorithm.Populations.AdamPopulationCreater
{
  /// <summary>
  /// Strategy that determines how to create the initial population from the initial deployment
  /// </summary>
  public interface IInitialPopulationCreator
  {
    IPopulation CreateInitialPopulation(IDeploymentChromosome initialDeployment, int minimalSize, int maximalSize);
  }
}