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
  public class FeatureInstanceTests
  {
    [Test]
    public void Constructors_Sets_Fields_Correctly()
    {
      var properties = new []
      {
        new Property(new PropertyIdentifier("p1"), "p1"), 
      };
      var feature = new Feature(new FeatureIdentifier("id"), "id", properties);
      var sot = new FeatureInstance(feature, new []{ new PropertyIdentifier("p1")}, true);

      Assert.True(sot.IsInternal);
      CollectionAssert.AreEqual(properties, sot.Properties);
      Assert.AreEqual(feature, sot.Feature);
      Assert.AreEqual(feature.Name, sot.Name);
      Assert.AreEqual(sot.FeatureId, feature.Id);
    }

    [Test]
    public void Constructor_WithoutProperties_ThrowsException()
    {
      Assert.Throws<ArgumentException>(() =>
      {
        var properties = new[]
        {
          new Property(new PropertyIdentifier("p1"), "p1"),
        };
        var feature = new Feature(new FeatureIdentifier("id"), "id", properties);
        var sot = new FeatureInstance(feature, Enumerable.Empty<PropertyIdentifier>());
      });
    }

    [Test]
    public void Constructor_ForPublicFeatureInstance_WithoutAllProperties_ThrowsException()
    {
      Assert.Throws<ArgumentException>(() =>
      {
        var properties = new[]
        {
          new Property(new PropertyIdentifier("p1"), "p1"),
          new Property(new PropertyIdentifier("p2"), "p2"), 
        };
        var feature = new Feature(new FeatureIdentifier("id"), "id", properties);
        var sot = new FeatureInstance(feature, properties.Take(1).Select(p => p.Id));
      });
    }

    [Test]
    public void ToString_Returns_NonEmptyString()
    {
      var properties = new[]
      {
        new Property(new PropertyIdentifier("p1"), "p1"),
      };
      var feature = new Feature(new FeatureIdentifier("id"), "id", properties);
      var sot = new FeatureInstance(feature, properties.Select(p => p.Id));

      Assert.False(string.IsNullOrEmpty(sot.ToString()));
    }

    [Test]
    public void ToString_OnInternalFeature_Returns_NonEmptyString()
    {
      var properties = new[]
      {
        new Property(new PropertyIdentifier("p1"), "p1"),
      };
      var feature = new Feature(new FeatureIdentifier("id"), "id", properties);
      var sot = new FeatureInstance(feature, properties.Select(p => p.Id), true);

      Assert.False(string.IsNullOrEmpty(sot.ToString()));
    }

    [Test]
    public void GetHashCode_OnIdenticalFeatureInstance_Returns_Same()
    {
      var properties = new[]
      {
        new Property(new PropertyIdentifier("p1"), "p1"),
      };
      var feature = new Feature(new FeatureIdentifier("id"), "id", properties);
      var sot = new FeatureInstance(feature, properties.Select(p => p.Id), true);

      var other = new FeatureInstance(feature, properties.Select(p => p.Id), true);

      Assert.AreEqual(sot.GetHashCode(), other.GetHashCode());
    }

    [Test]
    [TestCaseSource(typeof(EqualsTestCases))]
    public bool Equals_Returns_Correctly(FeatureInstance featureInstance, object other)
    {
      return featureInstance.Equals(other);
    }

    private class EqualsTestCases : IEnumerable<TestCaseData>
    {
      public IEnumerator<TestCaseData> GetEnumerator()
      {
        var properties = new[]
        {
          new Property(new PropertyIdentifier("p1"), "p1"),
        };
        var feature = new Feature(new FeatureIdentifier("id"), "id", properties);
        var sot = new FeatureInstance(feature, properties.Select(p => p.Id));

        var identicalFeatureInstance = new FeatureInstance(feature, properties.Select(p => p.Id));

        var internalFeatureInstance = new FeatureInstance(feature, properties.Select(p => p.Id), true);

        yield return new TestCaseData(sot, sot).Returns(true);
        yield return new TestCaseData(sot, identicalFeatureInstance).Returns(true);
        yield return new TestCaseData(sot, internalFeatureInstance).Returns(false);
        yield return new TestCaseData(sot, 3).Returns(false);
        yield return new TestCaseData(sot, null).Returns(false);
      }

      IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

  }
}