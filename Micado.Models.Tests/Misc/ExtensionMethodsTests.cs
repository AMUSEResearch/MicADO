using System;
using System.Linq;
using MicADO.Models.Deployment;
using MicADO.Models.Features;
using MicADO.Models.Misc;
using NUnit.Framework;

namespace Micado.Models.Tests.Misc
{
  [TestFixture]
  public class ExtensionMethodsTests
  {
    [Test]
    public void GetMicroserviceIdentifier_WithEmptyIEnumerable_OnCall_ThrowsException()
    {
      Assert.Throws<ArgumentOutOfRangeException>(() =>
      {
        var microserviceIdentifiers = Enumerable.Empty<FeatureIdentifier>();
        microserviceIdentifiers.GetMicroserviceIdentifier();
      });
    }

    [Test]
    public void GetMicroserviceIdentifier_WithSingleIEnumerable_OnCall_ReturnsElement()
    {
      var microserviceIdentifiers = new[] { new FeatureIdentifier("test") };
      Assert.AreEqual(new MicroserviceIdentifier("test"), microserviceIdentifiers.GetMicroserviceIdentifier());
    }

    [Test]
    public void GetMicroserviceIdentifier_WithMultipleElements_OnCall_ReturnsHighestFeatureIdentifier()
    {
      var microserviceIdentifiers = new[] { new FeatureIdentifier("c"), new FeatureIdentifier("aa"), new FeatureIdentifier("a"), new FeatureIdentifier("b"), };
      Assert.AreEqual(new MicroserviceIdentifier("a"), microserviceIdentifiers.GetMicroserviceIdentifier());
    }

    [Test]
    public void GetMicroserviceIdentifier_WithEmptyIEnumerableOfFeatureInstances_OnCall_ThrowsException()
    {
      Assert.Throws<ArgumentOutOfRangeException>(() =>
      {
        var featureInstances = Enumerable.Empty<FeatureInstance>();
        featureInstances.GetMicroserviceIdentifier();
      });
    }

    [Test]
    public void GetMicroserviceIdentifier_WithMultipleFeatureInstances_OnCall_ReturnsHighestFeatureIdentifier()
    {
      var aProperties = new[]
      {
        new Property(new PropertyIdentifier("p1"), "p1"),
      };

      var featureInstances = new[]
      {
        new FeatureInstance(new Feature(new FeatureIdentifier("a"), "a", aProperties), aProperties.Select(p => p.Id))
      };
      Assert.AreEqual(new MicroserviceIdentifier("a"), featureInstances.GetMicroserviceIdentifier());
    }

    [Test]
    public void GetMicroserviceIdentifier_WithInternalFeatureInstances_OnCall_ReturnsNoInternalFeatureInstance()
    {
      var aProperties = new[]
      {
        new Property(new PropertyIdentifier("p1"), "p1"),
      };

      var bProperties = new[]
      {
        new Property(new PropertyIdentifier("p2"), "p2"),
      };

      var featureInstances = new[]
      {
        new FeatureInstance(new Feature(new FeatureIdentifier("b"), "b", bProperties), bProperties.Select(p => p.Id)),
        new FeatureInstance(new Feature(new FeatureIdentifier("a"), "a", aProperties), aProperties.Select(p => p.Id), true)
      };
      Assert.AreEqual(new MicroserviceIdentifier("b"), featureInstances.GetMicroserviceIdentifier());
    }
  }
}