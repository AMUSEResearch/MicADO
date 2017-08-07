using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MicADO.GeneticAlgorithm.Chromosome;
using MicADO.GeneticAlgorithm.Chromosome.Gene;
using MicADO.GeneticAlgorithm.Populations;
using MicADO.GeneticAlgorithm.Reinsertion;
using MicADO.Models.Features;
using NUnit.Framework;

namespace MicADO.GeneticAlgorithm.Tests.Reinsertion
{
  [TestFixture]
  public class EliteReinsertionTests
  {
    [Test]
    public void Reinsert_WithLargeEnoughOffspring_Returns_NewPopulationWithOffspring()
    {
      var deployment = new DeploymentChromosome(A.Fake<FeatureModel>(), Enumerable.Empty<IDeploymentGene>());
      var deployments = Enumerable.Repeat(deployment, 20);
      var oldPopulation = new Population(Enumerable.Empty<IDeploymentChromosome>(), 20, 40);
      var offspring = deployments;
      var sot = new EliteReinsertion();
      var result = sot.Reinsert(offspring, oldPopulation);
      Assert.AreEqual(deployments, result.Deployments);
    }

    [Test]
    public void Reinsert_WithTooSmallOffspring_Returns_NewPopulationWithOffspringAndBestDeployments()
    {
      var oldDeployments = new[]
      {
        new DeploymentChromosome(A.Fake<FeatureModel>(), Enumerable.Empty<IDeploymentGene>()) { Fitness = 0.8 }
      };
      var offspring = new[]
      {
        new DeploymentChromosome(A.Fake<FeatureModel>(), Enumerable.Empty<IDeploymentGene>()) { Fitness = 1 }
      };
      var oldPopulation = new Population(oldDeployments, 2, 4);
      var sot = new EliteReinsertion();
      var result = sot.Reinsert(offspring, oldPopulation);
      CollectionAssert.AreEqual(offspring.Union(oldDeployments), result.Deployments);
    }
  }
}