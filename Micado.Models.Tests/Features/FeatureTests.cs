using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MicADO.Models.Features;
using MicADO.Models.Misc;
using NUnit.Framework;

namespace MicADO.Models.Tests.Features
{
  [TestFixture]
  public class FeatureTests
  {
    [Test]
    public void Feature_WithArguments_AfterConstruction_ContainsFields()
    {
      var properties = new[] { new Property(new PropertyIdentifier("property"), "property") };
      var feature = new Feature(new FeatureIdentifier("id"), "Test", properties);

      Assert.AreEqual(new FeatureIdentifier("id"), feature.Id );
      Assert.AreEqual("Test", feature.Name);
      CollectionAssert.AreEqual(new[] { new Property(new PropertyIdentifier("property"), "property") }, feature.Properties);
    }

    [Test]
    public void Feature_ToString_DoesNotReturnEmptyString()
    {
      var properties = new[] { new Property(new PropertyIdentifier("property"), "property") };
      var feature = new Feature(new FeatureIdentifier("id"), "Test", properties);

      Assert.False(string.IsNullOrEmpty(feature.ToString()));
    }

    [Test]
    public void Feature_ToHashCode_IdenticalForItself()
    {
      var properties = new[] { new Property(new PropertyIdentifier("property"), "property") };
      var feature = new Feature(new FeatureIdentifier("id"), "Test", properties);

      Assert.AreEqual(feature.GetHashCode(), feature.GetHashCode());
    }

    [Test]
    public void Feature_ToHashCode_IdenticalForSameFeature()
    {
      var properties = new[] { new Property(new PropertyIdentifier("property"), "property") };
      var feature = new Feature(new FeatureIdentifier("id"), "Test", properties);

      var secondProperties = new[] { new Property(new PropertyIdentifier("property"), "property") };
      var secondFeature = new Feature(new FeatureIdentifier("id"), "Test", secondProperties);

      Assert.AreEqual(feature.GetHashCode(), secondFeature.GetHashCode());
    }

    [Test]
    public void Feature_ToHashCode_IdenticalForSetSameFeature()
    {
      var properties = new[] { new Property(new PropertyIdentifier("property"), "property") };
      var feature = new Feature(new FeatureIdentifier("id"), "Test", properties);

      var secondFeature = new Feature(new FeatureIdentifier("id"), "Test", properties.Reverse());

      Assert.AreEqual(feature.GetHashCode(), secondFeature.GetHashCode());
    }

    [Test]
    [TestCaseSource(typeof(EqualsTestCases))]
    public bool Equals_Evaluates_Correctly(Feature feature, object other)
    {
      return feature.Equals(other);
    }

    private class EqualsTestCases : IEnumerable<TestCaseData>
    {
      public IEnumerator<TestCaseData> GetEnumerator()
      {
        var properties = new[] { new Property(new PropertyIdentifier("property"), "property"), new Property(new PropertyIdentifier("p2"), "p2" ) };
        var feature = new Feature(new FeatureIdentifier("id"), "Test", properties);

        var sameProperties = new[] { new Property(new PropertyIdentifier("property"), "property"), new Property(new PropertyIdentifier("p2"), "p2") };
        var sameFeature = new Feature(new FeatureIdentifier("id"), "Test", properties);
        var reverseFeature = new Feature(new FeatureIdentifier("id"), "Test", properties.Reverse());

        var differentFeature = new Feature(new FeatureIdentifier("test"), "Test", properties);

        yield return new TestCaseData(feature, feature).Returns(true);
        yield return new TestCaseData(feature, sameFeature).Returns(true);
        yield return new TestCaseData(feature, differentFeature).Returns(false);
        yield return new TestCaseData(feature, reverseFeature).Returns(true);
        yield return new TestCaseData(feature, null).Returns(false);
        yield return new TestCaseData(feature, 3).Returns(false);
      }

      IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
  }
}