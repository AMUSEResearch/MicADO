using System.Collections;
using System.Collections.Generic;
using MicADO.Models.Features;
using MicADO.Models.Misc;
using NUnit.Framework;

namespace MicADO.Models.Tests.Features
{
  [TestFixture]
  public class PropertyTests
  {

    [Test]
    [TestCaseSource(typeof(EqualsTestCases))]
    public bool Equals_Returns_Correctly(Property property, object other)
    {
      return property.Equals(other);
    }

    [Test]
    public void Property_ToHashCode_IdenticalForSameFeature()
    {
      var property = new Property(new PropertyIdentifier("property"), "property");

      var secondProperty = new Property(new PropertyIdentifier("property"), "property");

      Assert.AreEqual(property.GetHashCode(), secondProperty.GetHashCode());
    }

    private class EqualsTestCases : IEnumerable<TestCaseData>
    {
      public IEnumerator<TestCaseData> GetEnumerator()
      {
        var property = new Property(new PropertyIdentifier("id"), "id", 1);
        yield return new TestCaseData(property, property).Returns(true);
        yield return new TestCaseData(property, new Property(new PropertyIdentifier("id"),"id", 1)).Returns(true);
        yield return new TestCaseData(property, new Property(new PropertyIdentifier("other"), "id", 1)).Returns(false);
        yield return new TestCaseData(property, new Property(new PropertyIdentifier("id"), "other", 1)).Returns(false);
        yield return new TestCaseData(property, new Property(new PropertyIdentifier("id"), "id", 2)).Returns(false);
        yield return new TestCaseData(property, 3).Returns(false);
        yield return new TestCaseData(property, null).Returns(false);
      }

      IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
  }
}