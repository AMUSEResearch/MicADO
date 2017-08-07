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
  public class FeatureModelTests
  {
    private Feature[] _features;
    private PropertyRelation[] _relations;
    private FeatureModel _featureModel;

    [SetUp]
    public void SetUp()
    {
      _features = new[]
      {
        new Feature(new FeatureIdentifier("a"), "a", new[] { new Property(new PropertyIdentifier("p1"), "p1"), }),
        new Feature(new FeatureIdentifier("b"), "b", new[] { new Property(new PropertyIdentifier("p2"), "p2"), })
      };

      _relations = new[]
      {
        new PropertyRelation(new PropertyIdentifier("p1"), new PropertyIdentifier("p2")),
      };
      _featureModel = new FeatureModel(_features, _relations);
    }

    [Test]
    public void Constructor_WithInvalidRelations_ThrowsException()
    {
      Assert.Throws<ArgumentException>(() =>
      {
        var featureModel = new FeatureModel(_features, new []{ new PropertyRelation(new PropertyIdentifier("b"), new PropertyIdentifier("c")), });
      });
    }

    [Test]
    public void GetRelations_Returns_Correctly()
    {
      CollectionAssert.AreEqual(_relations, _featureModel.GetRelations(new FeatureIdentifier("a")));
      CollectionAssert.AreEqual(Enumerable.Empty<PropertyRelation>(), _featureModel.GetRelations(new FeatureIdentifier("b")));
    }

    [Test]
    [TestCaseSource(typeof(EqualsTestCases))]
    public bool Equals_Returns_Correctly(FeatureModel featureModel, object other)
    {
      return featureModel.Equals(other);
    }

    [Test]
    public void GetHashCode_ReturnsSame_ForIdenticalFeatureModel()
    {
      var identicalFeatureModel = new FeatureModel(_features.ToArray(), _relations.ToArray());
      Assert.AreEqual(_featureModel.GetHashCode(), identicalFeatureModel.GetHashCode());
    }

    [Test]
    public void GetHashCode_ReturnsSame_ForReversedFeaturesFeatureModel()
    {
      var identicalFeatureModel = new FeatureModel(_features.Reverse().ToArray(), _relations.Reverse().ToArray());
      Assert.AreEqual(_featureModel.GetHashCode(), identicalFeatureModel.GetHashCode());
    }

    [Test]
    public void Constructor_Sets_Fields_Correctly()
    {
      
      CollectionAssert.AreEqual(_features, _featureModel.Features);
      CollectionAssert.AreEqual(_relations ,_featureModel.Relations);
    }

    private class EqualsTestCases : IEnumerable<TestCaseData>
    {
      public IEnumerator<TestCaseData> GetEnumerator()
      {
        var features = new[]
        {
          new Feature(new FeatureIdentifier("a"), "a", new[] { new Property(new PropertyIdentifier("p1"), "p1"), }),
          new Feature(new FeatureIdentifier("b"), "b", new[] { new Property(new PropertyIdentifier("p2"), "p2"), })
        };

        var relations = new[]
        {
          new PropertyRelation(new PropertyIdentifier("p1"), new PropertyIdentifier("p2")),
        };
        var featureModel = new FeatureModel(features, relations);
        var identicalFeatureModel = new FeatureModel(features.ToArray(), relations.ToArray());
        var otherRelations = new FeatureModel(features, Enumerable.Empty<PropertyRelation>());
        var moreFeatureList = features.ToList();
        moreFeatureList.Add(new Feature(new FeatureIdentifier("c"), "c", new []{ new Property(new PropertyIdentifier("p3"), "p3" ) } ));
        var moreFeatures = new FeatureModel(moreFeatureList, relations);

        yield return new TestCaseData(featureModel, featureModel).Returns(true);
        yield return new TestCaseData(featureModel, identicalFeatureModel).Returns(true);
        yield return new TestCaseData(featureModel, new FeatureModel(features.Reverse(), relations.Reverse())).Returns(true);
        yield return new TestCaseData(featureModel, otherRelations).Returns(false);
        yield return new TestCaseData(featureModel, moreFeatures).Returns(false);
        yield return new TestCaseData(featureModel, null).Returns(false);
        yield return new TestCaseData(featureModel, 3).Returns(false);
      }

      IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
  }
}