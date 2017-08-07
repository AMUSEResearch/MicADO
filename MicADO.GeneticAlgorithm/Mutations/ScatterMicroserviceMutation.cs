using System;
using System.Collections.Generic;
using System.Linq;
using MicADO.GeneticAlgorithm.Chromosome;
using MicADO.GeneticAlgorithm.Chromosome.Gene;
using MicADO.GeneticAlgorithm.Misc;
using MicADO.Models.Misc;

namespace MicADO.GeneticAlgorithm.Mutations
{
  public class ScatterMicroserviceMutation : IMutation
  {
    private readonly IRandomProvider _randomProvider;

    public ScatterMicroserviceMutation(IRandomProvider randomProvider)
    {
      _randomProvider = randomProvider;
    }

    public IDeploymentChromosome Mutate(IDeploymentChromosome deployment)
    {
      var genes = deployment.Genes;
      var microserviceIds = genes.Select(g => new MicroserviceIdentifier(g.FeatureId.ToString())).ToArray();
      var newGenes = new List<IDeploymentGene>();
      foreach(var gene in genes)
      {
        var clonedGene = new DeploymentGene(gene.FeatureId, microserviceIds[_randomProvider.GetRandom(0, microserviceIds.Length)]);
        newGenes.Add(clonedGene);
      }
      return deployment.UpdateGenes(newGenes);
    }
  }
}