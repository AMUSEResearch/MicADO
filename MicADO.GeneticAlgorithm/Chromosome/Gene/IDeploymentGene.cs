using MicADO.Models.Misc;

namespace MicADO.GeneticAlgorithm.Chromosome.Gene
{
  public interface IDeploymentGene
  {
    FeatureIdentifier FeatureId { get; }

    MicroserviceIdentifier MicroserviceId { get; } 
  }
}