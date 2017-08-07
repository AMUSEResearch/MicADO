using System.Linq;
using FakeItEasy;
using MicADO.GeneticAlgorithm.Chromosome;
using MicADO.GeneticAlgorithm.Chromosome.Gene;
using MicADO.GeneticAlgorithm.Misc;
using MicADO.GeneticAlgorithm.Mutations;
using MicADO.Models.Features;
using MicADO.Models.Misc;
using NUnit.Framework;

namespace MicADO.GeneticAlgorithm.Tests.Mutations
{
  [TestFixture]
  public class ScatterMicroserviceMutationTests
  {
    [Test]
    public void Mutate_Operates_Correctly()
    {
      var featureModel = new FeatureModel(Enumerable.Empty<Feature>(), Enumerable.Empty<PropertyRelation>());
      var chromosome = new DeploymentChromosome(featureModel, new[]
      {
        new DeploymentGene(new FeatureIdentifier("a"), new MicroserviceIdentifier("a")),
        new DeploymentGene(new FeatureIdentifier("b"), new MicroserviceIdentifier("a")),
        new DeploymentGene(new FeatureIdentifier("c"), new MicroserviceIdentifier("a")),
      });

      var randomProvider = A.Fake<IRandomProvider>();
      A.CallTo(() => randomProvider.GetRandom(0, 0)).WithAnyArguments().ReturnsNextFromSequence(1, 0, 0);
      var sot = new ScatterMicroserviceMutation(randomProvider);
      var result = sot.Mutate(chromosome);
      var expected = new DeploymentChromosome(featureModel, new[]
      {
        new DeploymentGene(new FeatureIdentifier("a"), new MicroserviceIdentifier("a")),
        new DeploymentGene(new FeatureIdentifier("b"), new MicroserviceIdentifier("b")),
        new DeploymentGene(new FeatureIdentifier("c"), new MicroserviceIdentifier("b")),
      });

      Assert.AreEqual(expected, result);
    }
  }
}