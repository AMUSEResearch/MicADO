using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MicADO.Models.Deployment;
using MicADO.Models.Features;
using MicADO.Models.Misc;
using NUnit.Framework;

namespace MicADO.Models.Tests.Deployment
{
  [TestFixture]
  public class DeploymentModelTests
  {
    private FeatureModel CreateFeatureModel()
    {
      var features = new[]
      {
        new Feature(new FeatureIdentifier("f1"), "f2", new[]
        {
          new Property(new PropertyIdentifier("p1"), "p1")
        }),
        new Feature(new FeatureIdentifier("f2"), "f2", new[]
        {
          new Property(new PropertyIdentifier("p2"), "p2")
        })
      };
      var relations = Enumerable.Empty<PropertyRelation>();
      return new FeatureModel(features, relations);
    }

    private IEnumerable<Microservice> CreateMicroservices(FeatureModel featureModel)
    {
      var f1 = featureModel.Features.First();
      var f2 = featureModel.Features.Skip(1).First();

      yield return new Microservice(new []
      {
        new FeatureInstance(f1, f1.Properties.Select(p => p.Id))
      });

      yield return new Microservice(new []
      {
        new FeatureInstance(f2, f2.Properties.Select(p => p.Id)) 
      });
    }

    [Test]
    public void Constructor_Sets_Fields_Correctly()
    {
      var featureModel = CreateFeatureModel();
      var microservices = CreateMicroservices(featureModel);
      var deploymentModel = new DeploymentModel(featureModel, microservices);

      Assert.AreEqual(deploymentModel.FeatureModel, featureModel);
      CollectionAssert.AreEquivalent(microservices, deploymentModel.Microservices);
    }

    [Test]
    public void GetHashCode_Returns_Same_ForIdenticalDeploymentModel()
    {
      var featureModel = CreateFeatureModel();
      var microservices = CreateMicroservices(featureModel);
      var deploymentModel = new DeploymentModel(featureModel, microservices);

      var featureModel2 = CreateFeatureModel();
      var microservices2 = CreateMicroservices(featureModel);
      var deploymentModel2 = new DeploymentModel(featureModel2, microservices2);

      Assert.AreEqual(deploymentModel.GetHashCode(), deploymentModel2.GetHashCode());
    }

    [Test]
    public void GetHashCode_Returns_Same_ForReversedDeploymentModel()
    {
      var featureModel = CreateFeatureModel();
      var microservices = CreateMicroservices(featureModel);
      var deploymentModel = new DeploymentModel(featureModel, microservices);

      Assert.AreEqual(deploymentModel, new DeploymentModel(featureModel, microservices.Reverse()));
    }

    [Test]
    [TestCaseSource(typeof(EqualsTestCases))]
    public bool Equals_Returns_Correctly(DeploymentModel deploymentModel, object other)
    {
      return deploymentModel.Equals(other);
    }

    private class EqualsTestCases : IEnumerable<TestCaseData>
    {
      public IEnumerator<TestCaseData> GetEnumerator()
      {
        var featureModel = CreateFeatureModel();
        var microservices = CreateMicroservices(featureModel);
        var deploymentModel = new DeploymentModel(featureModel, microservices);

        var sameDeploymentModel = new DeploymentModel(featureModel, microservices);
        var reversedDeploymentModel = new DeploymentModel(featureModel, microservices.Reverse());

        var extendedMicroservices = CreateExtendedMicroservice(featureModel);
        var extendedDeploymentModel = new DeploymentModel(featureModel, extendedMicroservices);

        yield return new TestCaseData(deploymentModel, deploymentModel).Returns(true);
        yield return new TestCaseData(deploymentModel, sameDeploymentModel).Returns(true);
        yield return new TestCaseData(deploymentModel, reversedDeploymentModel).Returns(true);
        yield return new TestCaseData(deploymentModel, extendedDeploymentModel).Returns(false);
        yield return new TestCaseData(deploymentModel, null).Returns(false);
        yield return new TestCaseData(deploymentModel, 3).Returns(false);
      }

      IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
      private FeatureModel CreateFeatureModel()
      {
        var features = new[]
        {
          new Feature(new FeatureIdentifier("f1"), "f2", new[]
          {
            new Property(new PropertyIdentifier("p1"), "p1")
          }),
          new Feature(new FeatureIdentifier("f2"), "f2", new[]
          {
            new Property(new PropertyIdentifier("p2"), "p2")
          })
        };
        var relations = Enumerable.Empty<PropertyRelation>();
        return new FeatureModel(features, relations);
      }

      private IEnumerable<Microservice> CreateMicroservices(FeatureModel featureModel)
      {
        var f1 = featureModel.Features.First();
        var f2 = featureModel.Features.Skip(1).First();

        yield return new Microservice(new[]
        {
          new FeatureInstance(f1, f1.Properties.Select(p => p.Id))
        });

        yield return new Microservice(new[]
        {
          new FeatureInstance(f2, f2.Properties.Select(p => p.Id))
        });
      }

      private IEnumerable<Microservice> CreateExtendedMicroservice(FeatureModel featureModel)
      {
        var f1 = featureModel.Features.First();
        var f2 = featureModel.Features.Skip(1).First();

        yield return new Microservice(new[]
        {
          new FeatureInstance(f1, f1.Properties.Select(p => p.Id)),
          new FeatureInstance(f2, f2.Properties.Select(p => p.Id).Take(1), true)
        });

        yield return new Microservice(new[]
        {
          new FeatureInstance(f2, f2.Properties.Select(p => p.Id))
        });
      }
    }
  }
}