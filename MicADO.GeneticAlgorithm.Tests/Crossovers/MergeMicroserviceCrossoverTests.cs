using System;
using System.Linq;
using FakeItEasy;
using MicADO.GeneticAlgorithm.Chromosome;
using MicADO.GeneticAlgorithm.Chromosome.Gene;
using MicADO.GeneticAlgorithm.Crossovers;
using MicADO.GeneticAlgorithm.Misc;
using MicADO.Models.Features;
using MicADO.Models.Misc;
using NUnit.Framework;

namespace MicADO.GeneticAlgorithm.Tests.Crossovers
{
  [TestFixture]
  public class MergeMicroserviceCrossoverTests
  {
    [Test]
    public void Constructor_Sets_Fields_Correctly()
    {
      var randomProvider = A.Fake<IRandomProvider>();
      var crossover = new MergeMicroserviceCrossover(randomProvider);
      Assert.AreEqual(2, crossover.ChildrenNumber);
      Assert.AreEqual(2, crossover.ParentsNumber);
    }

    [Test]
    public void Cross_WithChromosomesOfDifferentLength_ThrowsException()
    {
      Assert.Throws<ArgumentException>(() =>
      {
        var chromosome = A.Fake<IDeploymentChromosome>();
        A.CallTo(() => chromosome.Genes).Returns(Enumerable.Repeat(A.Fake<IDeploymentGene>(), 2).ToArray());
        var secondChromsome = A.Fake<IDeploymentChromosome>();
        A.CallTo(() => secondChromsome.Genes).Returns(Enumerable.Repeat(A.Fake<IDeploymentGene>(), 1).ToArray());
        var sot = new MergeMicroserviceCrossover(A.Fake<IRandomProvider>());
        sot.Cross(new[] { chromosome, secondChromsome }).ToArray();
      });
    }

    [Test]
    public void Cross_WithSingleGene_Returns_SameChromosome()
    {
      var featureModel = new FeatureModel(new[]
      {
        new Feature(
          new FeatureIdentifier("a"),
          "a",
          new[] { new Property(new PropertyIdentifier("p1"), "p1") }
        )
      }, Enumerable.Empty<PropertyRelation>());
      var deploymentGenes = featureModel.Features.Select(f => new DeploymentGene(f.Id, new MicroserviceIdentifier(f.Id.Id))).ToArray();
      var chromosome = new DeploymentChromosome(featureModel, deploymentGenes);

      var randomProvider = A.Fake<IRandomProvider>();
      A.CallTo(() => randomProvider.GetRandom()).Returns(0);
      var sot = new MergeMicroserviceCrossover(randomProvider);
      var result = sot.Cross(new[] { chromosome, chromosome }).ToArray();
      CollectionAssert.AreEqual(new[] { chromosome, chromosome }, result);
    }

    [Test]
    public void Cross_WithMultipleGene_Returns_Correctly()
    {
      var featureModel = new FeatureModel(Enumerable.Empty<Feature>(), Enumerable.Empty<PropertyRelation>());
      var chromosome = new DeploymentChromosome(featureModel, new[]
      {
        new DeploymentGene(new FeatureIdentifier("a"), new MicroserviceIdentifier("a")),
        new DeploymentGene(new FeatureIdentifier("b"), new MicroserviceIdentifier("b")),
        new DeploymentGene(new FeatureIdentifier("c"), new MicroserviceIdentifier("c")),
      });

      var randomProvider = A.Fake<IRandomProvider>();
      A.CallTo(() => randomProvider.GetRandom(0, 0)).WithAnyArguments().ReturnsNextFromSequence(0, 1, 0, 1);
      var sot = new MergeMicroserviceCrossover(randomProvider);
      var result = sot.Cross(new[] { chromosome, chromosome }).ToArray();

      var expected = new DeploymentChromosome(featureModel, new[]
      {
        new DeploymentGene(new FeatureIdentifier("a"), new MicroserviceIdentifier("a")),
        new DeploymentGene(new FeatureIdentifier("b"), new MicroserviceIdentifier("a")),
        new DeploymentGene(new FeatureIdentifier("c"), new MicroserviceIdentifier("c")),
      });

      CollectionAssert.AreEqual(new[] { expected, expected }, result);
    }
  }
}