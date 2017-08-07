using MicADO.Models.Deployment;

namespace MicADO.GeneticAlgorithm.Chromosome.Factory
{
  public interface IDeploymentChromosomeFactory
  {
    IDeploymentChromosome Create(DeploymentModel deploymentModel);
  }
}