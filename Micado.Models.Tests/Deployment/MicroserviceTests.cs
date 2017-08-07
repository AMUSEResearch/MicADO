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
  public class MicroserviceTests
  {
    [Test]
    public void Constructor_Sets_Fields_Correctly()
    {
      var aProperties = new []{ new Property(new PropertyIdentifier("p1"), "p1")};
      var bProperties = new[] { new Property(new PropertyIdentifier("p2"), "p2") };
      var featureInstances = new[]
      {
        new FeatureInstance(new Feature(new FeatureIdentifier("a"), "a", aProperties), aProperties.Select(p => p.Id)),
        new FeatureInstance(new Feature(new FeatureIdentifier("b"), "b",  bProperties), bProperties.Select(p => p.Id)) 
      };
      var microservice = new Microservice(featureInstances);
      CollectionAssert.AreEqual(featureInstances, microservice.AsEnumerable());
      Assert.AreEqual(new MicroserviceIdentifier("a"), microservice.Id);
    }

    [Test]
    public void Constructor_WithEmptyFeatureInstances_ThrowsException()
    {
      Assert.Throws<ArgumentException>(() =>
      {
        var sot = new Microservice(Enumerable.Empty<FeatureInstance>());
      });
    }

    [Test]
    public void ToString_Returns_NonEmptyString()
    {
      var aProperties = new[] { new Property(new PropertyIdentifier("p1"), "p1") };
      var bProperties = new[] { new Property(new PropertyIdentifier("p2"), "p2") };
      var featureInstances = new[]
      {
        new FeatureInstance(new Feature(new FeatureIdentifier("a"), "a", aProperties), aProperties.Select(p => p.Id)),
        new FeatureInstance(new Feature(new FeatureIdentifier("b"), "b",  bProperties), bProperties.Select(p => p.Id))
      };
      var microservice = new Microservice(featureInstances);
      Assert.False(string.IsNullOrEmpty(microservice.ToString()));
    }

    [Test]
    [TestCaseSource(typeof(EqualsTestCases))]
    public bool Equals_Returns_Correctly(Microservice microservice, object other)
    {
      return microservice.Equals(other);
    }

    [Test]
    public void GetHashCode_IsEqual_ForIdenticalMicroservice()
    {
      var aProperties = new[] { new Property(new PropertyIdentifier("p1"), "p1") };
      var bProperties = new[] { new Property(new PropertyIdentifier("p2"), "p2") };
      var featureInstances = new[]
      {
        new FeatureInstance(new Feature(new FeatureIdentifier("a"), "a", aProperties), aProperties.Select(p => p.Id)),
        new FeatureInstance(new Feature(new FeatureIdentifier("b"), "b",  bProperties), bProperties.Select(p => p.Id))
      };
      var microservice = new Microservice(featureInstances);

      var microserviceTwo = new Microservice(featureInstances.ToArray());

      Assert.AreEqual(microservice.GetHashCode(), microserviceTwo.GetHashCode());
    }

    [Test]
    public void GetHashCode_IsEqual_ForReversedMicroservice()
    {
      var aProperties = new[] { new Property(new PropertyIdentifier("p1"), "p1") };
      var bProperties = new[] { new Property(new PropertyIdentifier("p2"), "p2") };
      var featureInstances = new[]
      {
        new FeatureInstance(new Feature(new FeatureIdentifier("a"), "a", aProperties), aProperties.Select(p => p.Id)),
        new FeatureInstance(new Feature(new FeatureIdentifier("b"), "b",  bProperties), bProperties.Select(p => p.Id))
      };
      var microservice = new Microservice(featureInstances);

      var microserviceTwo = new Microservice(featureInstances.ToArray().Reverse());

      Assert.AreEqual(microservice.GetHashCode(), microserviceTwo.GetHashCode());
    }

    private class EqualsTestCases : IEnumerable<TestCaseData>
    {
      public IEnumerator<TestCaseData> GetEnumerator()
      {
        var aProperties = new[] { new Property(new PropertyIdentifier("p1"), "p1") };
        var bProperties = new[] { new Property(new PropertyIdentifier("p2"), "p2") };
        var featureInstances = new[]
        {
          new FeatureInstance(new Feature(new FeatureIdentifier("a"), "a", aProperties), aProperties.Select(p => p.Id)),
          new FeatureInstance(new Feature(new FeatureIdentifier("b"), "b",  bProperties), bProperties.Select(p => p.Id))
        };
        var microservice = new Microservice(featureInstances);
        var identicalMicroservice = new Microservice(featureInstances.ToArray());
        var sameMicroservice = new Microservice(featureInstances.ToArray().Reverse());

        yield return new TestCaseData(microservice, microservice).Returns(true);
        yield return new TestCaseData(microservice, identicalMicroservice).Returns(true);
        yield return new TestCaseData(microservice, sameMicroservice).Returns(true);
        yield return new TestCaseData(microservice, null).Returns(false);
        yield return new TestCaseData(microservice, 3).Returns(false);
      }

      IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
  }
}