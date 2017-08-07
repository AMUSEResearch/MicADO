using System.Collections;
using System.Collections.Generic;
using MicADO.Models.Misc;
using NUnit.Framework;

namespace Micado.Models.Tests.Misc
{
  [TestFixture]
  public class PropertyIdentifierTests
  {
    [Test]
    public void PropertyIdentifier_WithArgument_AfterConstructor_CorrectlySetsId()
    {
      var sot = new PropertyIdentifier("test");
      Assert.AreEqual("test", sot.Id);
    }

    [Test]
    public void ToString_Returns_Id()
    {
      var sot = new PropertyIdentifier("test2");
      Assert.AreEqual("test2", sot.ToString());
    }

    [Test]
    [TestCaseSource(typeof(EqualsTestCases))]
    public bool Equals_Returns_Correctly(PropertyIdentifier sot, object other)
    {
      return sot.Equals(other);
    }

    [Test]
    public void GetHashCode_ReturnValue_Equals_Id_Hashcode()
    {
      var sot = new PropertyIdentifier("sot");
      Assert.AreEqual(sot.Id.GetHashCode(), sot.GetHashCode());
    }

    [Test]
    [TestCaseSource(typeof(CompareToTestCases))]
    public int CompareTo_Returns_Correctly(PropertyIdentifier propertyIdentifier, PropertyIdentifier other)
    {
      return propertyIdentifier.CompareTo(other);
    }

    private class EqualsTestCases : IEnumerable<TestCaseData>
    {
      public IEnumerator<TestCaseData> GetEnumerator()
      {
        var id = new PropertyIdentifier("id");
        yield return new TestCaseData(id, id).Returns(true);
        yield return new TestCaseData(id, new PropertyIdentifier("id")).Returns(true);
        yield return new TestCaseData(id, new PropertyIdentifier("test")).Returns(false);
        yield return new TestCaseData(id, null).Returns(false);
        yield return new TestCaseData(id, 3).Returns(false);
      }

      IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
    private class CompareToTestCases : IEnumerable<TestCaseData>
    {
      public IEnumerator<TestCaseData> GetEnumerator()
      {
        var id = new PropertyIdentifier("b");
        yield return new TestCaseData(id, id).Returns(0);
        yield return new TestCaseData(id, new PropertyIdentifier("b")).Returns(0);
        yield return new TestCaseData(id, new PropertyIdentifier("c")).Returns(-1);
        yield return new TestCaseData(id, new PropertyIdentifier("a")).Returns(1);
        yield return new TestCaseData(id, null).Returns(1);
      }

      IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
  }
}