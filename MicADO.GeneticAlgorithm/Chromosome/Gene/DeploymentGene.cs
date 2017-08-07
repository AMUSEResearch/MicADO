using MicADO.Models.Misc;

namespace MicADO.GeneticAlgorithm.Chromosome.Gene
{
  public class DeploymentGene : IDeploymentGene
  {
    public DeploymentGene(FeatureIdentifier featureId, MicroserviceIdentifier microserviceId)
    {
      FeatureId = featureId;
      MicroserviceId = microserviceId;
    }

    public FeatureIdentifier FeatureId { get; }

    public MicroserviceIdentifier MicroserviceId { get; }

    public override bool Equals(object obj)
    {
      DeploymentGene gene = obj as DeploymentGene;
      return gene != null && (FeatureId.Equals(gene.FeatureId) && MicroserviceId.Equals(gene.MicroserviceId));
    }

    public override string ToString()
    {
      return $"{FeatureId} -> {MicroserviceId}";
    }

    public override int GetHashCode()
    {
      unchecked
      {
        var hashCode = HashConstants.OffsetBasis;
        hashCode = (hashCode ^ (FeatureId.GetHashCode())) * HashConstants.Prime;
        hashCode = (hashCode ^ (MicroserviceId.GetHashCode())) * HashConstants.Prime;
        return hashCode;
      }
    }
  }
}