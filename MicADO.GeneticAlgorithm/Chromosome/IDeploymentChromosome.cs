using System.Collections.Generic;
using MicADO.GeneticAlgorithm.Chromosome.Factory;
using MicADO.GeneticAlgorithm.Chromosome.Gene;
using MicADO.Models.Deployment;
using MicADO.Models.Features;
using MicADO.Models.Misc;

namespace MicADO.GeneticAlgorithm.Chromosome
{
  /// <summary>
  /// Interface to a deployment chromosome that should be implemented as immutable
  /// </summary>
  public interface IDeploymentChromosome
  {
    FeatureModel FeatureModel { get;  }

    double? Fitness { get; set; }

    IReadOnlyCollection<IDeploymentGene> Genes { get; }

    DeploymentModel ToDeploymentModel();

    /// <summary>
    /// This method should return a *new* IDeploymentChromosome with the modified gene
    /// </summary>
    /// <param name="featureId"></param>
    /// <param name="microserviceId"></param>
    /// <returns></returns>
    IDeploymentChromosome UpdateGene(IDeploymentGene gene);

    /// <summary>
    /// This method should return a *new* IDeploymentChromosome with all the modifications applied
    /// This method should be used in case multiple modifications are applied, since no intermediate IDeploymentChromosomes should be made
    /// </summary>
    /// <param name="genes"></param>
    /// <returns></returns>
    IDeploymentChromosome UpdateGenes(IReadOnlyCollection<IDeploymentGene> genes);

    IDeploymentGene GetGene(FeatureIdentifier featureId);
  }
}