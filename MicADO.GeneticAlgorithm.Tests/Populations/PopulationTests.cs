using System.Linq;
using MicADO.GeneticAlgorithm.Chromosome;
using MicADO.GeneticAlgorithm.Chromosome.Gene;
using MicADO.GeneticAlgorithm.Populations;
using NUnit.Framework;

namespace MicADO.GeneticAlgorithm.Tests.Populations
{
  [TestFixture]
  public class PopulationTests
  {
    [Test]
    public void Constructor_Sets_Fields_Correctly()
    {
      var deployments = new[]
      {
        new DeploymentChromosome(null, Enumerable.Empty<IDeploymentGene>())
      };
      var sot = new Population(deployments, 10, 20);

      Assert.AreEqual(deployments, sot.Deployments);
      Assert.AreEqual(deployments.First(), sot.BestDeployment);
      Assert.AreEqual(10, sot.MinimalSize);
      Assert.AreEqual(20, sot.MaximalSize);
      Assert.AreEqual(1, sot.Count);
    }
  }
}