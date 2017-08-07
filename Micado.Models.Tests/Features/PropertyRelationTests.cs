using System;
using System.Collections;
using System.Collections.Generic;
using MicADO.Models.Features;
using MicADO.Models.Misc;
using NUnit.Framework;

namespace MicADO.Models.Tests.Features
{
  [TestFixture]
  public class PropertyRelationTests
  {
    [Test]
    public void Constructor_SetsFields_Correctly()
    {
      var from = new PropertyIdentifier("from");
      var to = new PropertyIdentifier("to");
      var propertyRelation = new PropertyRelation(from, to);
      Assert.AreEqual(from, propertyRelation.From);
      Assert.AreEqual(to, propertyRelation.To);
    }

    [Test]
    public void ToString_Returns_NotEmptyString()
    {
      var from = new PropertyIdentifier("from");
      var to = new PropertyIdentifier("to");
      var propertyRelation = new PropertyRelation(from, to);
      Assert.False(string.IsNullOrEmpty(propertyRelation.ToString()));
    }

    [Test]
    public void PropertyRelation_ToHashCode_IdenticalForSameFeature()
    {
      var relation = new PropertyRelation(new PropertyIdentifier("from"), new PropertyIdentifier("to"));

      var secondRelation = new PropertyRelation(new PropertyIdentifier("from"), new PropertyIdentifier("to"));

      Assert.AreEqual(relation.GetHashCode(), secondRelation.GetHashCode());
    }

    [Test]
    [TestCaseSource(typeof(EqualsTestCases))]
    public bool Equals_Returns_Correctly(PropertyRelation propertyRelation, object other)
    {
      return propertyRelation.Equals(other);
    }

    private class EqualsTestCases : IEnumerable<TestCaseData>
    {
      public IEnumerator<TestCaseData> GetEnumerator()
      {
        var from = new PropertyIdentifier("from");
        var to = new PropertyIdentifier("to");
        var propertyRelation = new PropertyRelation(from, to);

        yield return new TestCaseData(propertyRelation, propertyRelation).Returns(true);
        yield return new TestCaseData(propertyRelation, new PropertyRelation(new PropertyIdentifier("from"), new PropertyIdentifier("to"))).Returns(true);
        yield return new TestCaseData(propertyRelation, new PropertyRelation(new PropertyIdentifier("to"), new PropertyIdentifier("to"))).Returns(false);
        yield return new TestCaseData(propertyRelation, new PropertyRelation(new PropertyIdentifier("from"), new PropertyIdentifier("from"))).Returns(false);
        yield return new TestCaseData(propertyRelation, null).Returns(false);
        yield return new TestCaseData(propertyRelation, 3).Returns(false);
      }

      IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
  }
}