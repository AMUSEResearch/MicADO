using System;
using System.Collections;
using System.Collections.Generic;
using MicADO.Models.Misc;
using NUnit.Framework;

namespace Micado.Models.Tests.Misc
{
  [TestFixture]
  public class FeatureIdentifierTests
  {
    [Test]
    public void FeatureIdentifier_WithArgument_AfterConstructor_CorrectlySetsId()
    {
      var sot = new FeatureIdentifier("test");
      Assert.AreEqual("test", sot.Id);
    }

    [Test]
    public void ToString_Returns_Id()
    {
      var sot = new FeatureIdentifier("test2");
      Assert.AreEqual("test2", sot.ToString());
    }

    [Test]
    [TestCaseSource(typeof(EqualsTestCases))]
    public bool Equals_Returns_Correctly(FeatureIdentifier sot, object other)
    {
      return sot.Equals(other);
    }

    [Test]
    public void GetHashCode_ReturnValue_Equals_Id_Hashcode()
    {
      var sot = new FeatureIdentifier("sot");
      Assert.AreEqual(sot.Id.GetHashCode(), sot.GetHashCode());
    }

    [Test]
    [TestCaseSource(typeof(CompareToTestCases))]
    public int CompareTo_Returns_Correctly(FeatureIdentifier sot, FeatureIdentifier other)
    {
      return sot.CompareTo(other);
    }

    private class CompareToTestCases : IEnumerable<TestCaseData>
    {
      public IEnumerator<TestCaseData> GetEnumerator()
      {
        var id = new FeatureIdentifier("b");
        yield return new TestCaseData(id, id).Returns(0);
        yield return new TestCaseData(id, new FeatureIdentifier("b")).Returns(0);
        yield return new TestCaseData(id, new FeatureIdentifier("c")).Returns(-1);
        yield return new TestCaseData(id, null).Returns(1);
        yield return new TestCaseData(id, new FeatureIdentifier("a")).Returns(1);
      }

      IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    private class EqualsTestCases : IEnumerable<TestCaseData>
    {
      public IEnumerator<TestCaseData> GetEnumerator()
      {
        var id = new FeatureIdentifier("id");
        yield return new TestCaseData(id, id).Returns(true);
        yield return new TestCaseData(id, new FeatureIdentifier("id")).Returns(true);
        yield return new TestCaseData(id, new FeatureIdentifier("test")).Returns(false);
        yield return new TestCaseData(id, null).Returns(false);
        yield return new TestCaseData(id, 3).Returns(false);
      }

      IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
  }
}