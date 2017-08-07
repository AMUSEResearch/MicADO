using System.Collections;
using System.Collections.Generic;
using MicADO.GeneticAlgorithm.Chromosome.Gene;
using MicADO.Models.Misc;
using NUnit.Framework;

namespace MicADO.GeneticAlgorithm.Tests.Chromosomes.Gene
{
  [TestFixture]
  public class DeploymentGeneTests
  {
    private class EqualsTestCases : IEnumerable<TestCaseData>
    {
      public IEnumerator<TestCaseData> GetEnumerator()
      {
        var featureId = new FeatureIdentifier("a");
        var microserviceId = new MicroserviceIdentifier("b");
        var gene = new DeploymentGene(featureId, microserviceId);

        yield return new TestCaseData(gene, gene).Returns(true);
        yield return new TestCaseData(gene, new DeploymentGene(featureId, microserviceId)).Returns(true);
        yield return new TestCaseData(gene, new DeploymentGene(featureId, new MicroserviceIdentifier("c"))).Returns(false);
        yield return new TestCaseData(gene, new DeploymentGene(new FeatureIdentifier("b"), microserviceId)).Returns(false);
        yield return new TestCaseData(gene, null).Returns(false);
        yield return new TestCaseData(gene, 3).Returns(false);
      }

      IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    [Test]
    public void Constructor_Sets_Fields_Correctly()
    {
      var featureIdentifier = new FeatureIdentifier("a");
      var microserviceIdentifier = new MicroserviceIdentifier("b");

      var sot = new DeploymentGene(featureIdentifier, microserviceIdentifier);

      Assert.AreEqual(featureIdentifier, sot.FeatureId);
      Assert.AreEqual(microserviceIdentifier, sot.MicroserviceId);
    }

    [Test]
    public void ToString_Returns_NonEmptyString()
    {
      var featureIdentifier = new FeatureIdentifier("a");
      var microserviceIdentifier = new MicroserviceIdentifier("b");

      var sot = new DeploymentGene(featureIdentifier, microserviceIdentifier);

      Assert.False(string.IsNullOrEmpty(sot.ToString()));
    }

    [Test]
    [TestCaseSource(typeof(EqualsTestCases))]
    public bool Equals_Returns_Correctly(DeploymentGene gene, object other)
    {
      return gene.Equals(other);
    }

    [Test]
    public void GetHashCode_Returns_Same_ForIdenticalGene()
    {
      var featureIdentifier = new FeatureIdentifier("a");
      var microserviceIdentifier = new MicroserviceIdentifier("b");

      var sot = new DeploymentGene(featureIdentifier, microserviceIdentifier);

      var sameFeatureIdentifier = new FeatureIdentifier("a");
      var sameMicroserviceIdentifier = new MicroserviceIdentifier("b");

      var same = new DeploymentGene(sameFeatureIdentifier, sameMicroserviceIdentifier);

      Assert.AreEqual(sot.GetHashCode(), same.GetHashCode());
    }
  }
}