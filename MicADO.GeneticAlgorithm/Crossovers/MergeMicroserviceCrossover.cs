using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using MicADO.GeneticAlgorithm.Chromosome;
using MicADO.GeneticAlgorithm.Chromosome.Gene;
using MicADO.GeneticAlgorithm.Misc;

namespace MicADO.GeneticAlgorithm.Crossovers
{
  public class MergeMicroserviceCrossover : ICrossover
  {
    private readonly IRandomProvider _randomProvider;

    public int ParentsNumber => 2;

    public int ChildrenNumber => 2;

    public MergeMicroserviceCrossover(IRandomProvider randomProvider)
    {
      _randomProvider = randomProvider;
    }

    public IEnumerable<IDeploymentChromosome> Cross(IEnumerable<IDeploymentChromosome> parents)
    {
      var first = parents.First();
      var second = parents.ElementAt(1);

      var firstGeneIndex = _randomProvider.GetRandom(0, first.Genes.Count);
      var secondGeneIndex = _randomProvider.GetRandom(0, second.Genes.Count);

      yield return Cross(first, second, firstGeneIndex, secondGeneIndex);
      yield return Cross(second, first, secondGeneIndex, firstGeneIndex);
    }

    internal IDeploymentChromosome Cross(IDeploymentChromosome firstParent, IDeploymentChromosome secondParent, int firstGeneIndex, int secondGeneIndex)
    {
      var firstParentGenes = firstParent.Genes.ToArray();
      var secondParentGenes = secondParent.Genes.ToArray();

      if(firstParentGenes.Length != secondParentGenes.Length)
      {
        throw new ArgumentException("Both Chromosomes should have the same length");
      }

      var firstMicroserviceId = firstParentGenes[firstGeneIndex].MicroserviceId;
      var secondMicroserviceId = secondParentGenes[secondGeneIndex].MicroserviceId;

      var FeatureIdsToBeMovedFromFirstParent = firstParentGenes.Where(g => g.MicroserviceId == firstMicroserviceId).Select(g => g.FeatureId);
      var FeatureIdsToBeMovedFromSecondParent = secondParentGenes.Where(g => g.MicroserviceId == secondMicroserviceId).Select(g => g.FeatureId);
      var featuresToBeMoved = FeatureIdsToBeMovedFromFirstParent.Union(FeatureIdsToBeMovedFromSecondParent).Distinct();

      // Which microserviceId we use here does not matter, since UpdateGenes will fix them to the correct one
      var newDeploymentGenes = featuresToBeMoved.Select(f => new DeploymentGene(f, firstMicroserviceId)).ToArray();
      return firstParent.UpdateGenes(newDeploymentGenes);
    }
  }
}