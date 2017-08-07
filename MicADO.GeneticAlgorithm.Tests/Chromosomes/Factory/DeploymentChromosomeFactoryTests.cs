using System.Linq;
using FakeItEasy;
using MicADO.GeneticAlgorithm.Chromosome;
using MicADO.GeneticAlgorithm.Chromosome.Factory;
using MicADO.GeneticAlgorithm.Chromosome.Gene;
using MicADO.Models.Deployment;
using MicADO.Models.Features;
using MicADO.Models.Misc;
using NUnit.Framework;

namespace MicADO.GeneticAlgorithm.Tests.Chromosomes.Factory
{
  [TestFixture]
  public class DeploymentChromosomeFactoryTests
  {
    [Test]
    public void Create_WithoutDependencies_Creates_CorrectChromosome()
    {
      var aFeature = new Feature(new FeatureIdentifier("a"), "a", new[]
      {
        new Property(new PropertyIdentifier("p1"), "p1")
      });

      var bFeature = new Feature(new FeatureIdentifier("b"), "b", new []
      {
        new Property(new PropertyIdentifier("p2"), "p2")
      });
      var cFeature = new Feature(new FeatureIdentifier("c"), "c", new []
      {
        new Property(new PropertyIdentifier("p3"), "p3")
      });

      var featureModel = new FeatureModel(new []
      {
        aFeature,
        bFeature,
        cFeature
      }, Enumerable.Empty<PropertyRelation>());

      var microservices = new[]
      {
        new Microservice(new []
        {
          new FeatureInstance(aFeature, aFeature.Properties.Select(p => p.Id)) 
        }),
        new Microservice(new []
        {
          new FeatureInstance(bFeature, bFeature.Properties.Select(p => p.Id))
        }),
        new Microservice(new []
        {
          new FeatureInstance(cFeature, cFeature.Properties.Select(p => p.Id))
        }),
      };
      var deploymentModel = new DeploymentModel(featureModel, microservices);

      var sot = new DeploymentChromosomeFactory();
      var chromosome = sot.Create(deploymentModel);

      Assert.AreEqual(deploymentModel, chromosome.ToDeploymentModel());
    }


    [Test]
    public void Create_WithDependencies_Creates_CorrectChromosome()
    {
      var aFeature = new Feature(new FeatureIdentifier("a"), "a", new[]
        {
          new Property(new PropertyIdentifier("p1"), "p1"),
          new Property(new PropertyIdentifier("p2"), "p2")
        }
      );

      var bFeature = new Feature(new FeatureIdentifier("b"), "b", new[]
        {
          new Property(new PropertyIdentifier("p3"), "p3"),
          new Property(new PropertyIdentifier("p4"), "p4")
        }
      );

      var cFeature = new Feature(new FeatureIdentifier("c"), "c", new[]
        {
          new Property(new PropertyIdentifier("p5"), "p5"),
          new Property(new PropertyIdentifier("p6"), "p6"),
          new Property(new PropertyIdentifier("p7"), "p7")
        }
      );

      var featureModel = new FeatureModel(new[] { aFeature, bFeature, cFeature }, new[]
      {
        new PropertyRelation(new PropertyIdentifier("p6"), new PropertyIdentifier("p2")),
        new PropertyRelation(new PropertyIdentifier("p7"), new PropertyIdentifier("p4"))
      });

      var microservices = new[]
      {
        new Microservice(new[] { new FeatureInstance(aFeature, new[] { new PropertyIdentifier("p1"), new PropertyIdentifier("p2") }) }),
        new Microservice(new[] { new FeatureInstance(bFeature, new[] { new PropertyIdentifier("p3"), new PropertyIdentifier("p4") }) }),
        new Microservice(new[]
        {
          new FeatureInstance(cFeature, new[] { new PropertyIdentifier("p5"), new PropertyIdentifier("p6"), new PropertyIdentifier("p7") }),
          new FeatureInstance(bFeature, new[] { new PropertyIdentifier("p4") }, true),
          new FeatureInstance(aFeature, new[] { new PropertyIdentifier("p2") }, true)
        }),
      };

      var deploymentModel = new DeploymentModel(featureModel, microservices);

      var sot = new DeploymentChromosomeFactory();
      var chromosome = sot.Create(deploymentModel);

      Assert.AreEqual(deploymentModel, chromosome.ToDeploymentModel());
    }
  }
}