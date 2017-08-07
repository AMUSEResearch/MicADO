using System.Collections;
using System.Collections.Generic;
using MicADO.Models.Misc;
using NUnit.Framework;

namespace Micado.Models.Tests.Misc
{
  [TestFixture]
  public class MicroserviceIdentifierTests
  {
    [Test]
    public void MicroserviceIdentifier_WithArgument_AfterConstructor_CorrectlySetsId()
    {
      var sot = new MicroserviceIdentifier("test");
      Assert.AreEqual("test", sot.Id);
    }

    [Test]
    public void ToString_Returns_Id()
    {
      var sot = new MicroserviceIdentifier("test2");
      Assert.AreEqual("test2", sot.ToString());
    }

    [Test]
    [TestCaseSource(typeof(EqualsTestCases))]
    public bool Equals_Returns_Correctly(MicroserviceIdentifier sot, object other)
    {
      return sot.Equals(other);
    }

    [Test]
    public void GetHashCode_ReturnValue_Equals_Id_Hashcode()
    {
      var sot = new MicroserviceIdentifier("sot");
      Assert.AreEqual(sot.Id.GetHashCode(), sot.GetHashCode());
    }

    [Test]
    [TestCaseSource(typeof(CompareToTestCases))]
    public int CompareTo_Returns_Correctly(MicroserviceIdentifier sot, MicroserviceIdentifier other)
    {
      return sot.CompareTo(other);
    }

    private class CompareToTestCases : IEnumerable<TestCaseData>
    {
      public IEnumerator<TestCaseData> GetEnumerator()
      {
        var id = new MicroserviceIdentifier("b");
        yield return new TestCaseData(id, id).Returns(0);
        yield return new TestCaseData(id, new MicroserviceIdentifier("b")).Returns(0);
        yield return new TestCaseData(id, new MicroserviceIdentifier("c")).Returns(-1);
        yield return new TestCaseData(id, null).Returns(1);
        yield return new TestCaseData(id, new MicroserviceIdentifier("a")).Returns(1);
      }

      IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    private class EqualsTestCases : IEnumerable<TestCaseData>
    {
      public IEnumerator<TestCaseData> GetEnumerator()
      {
        var id = new MicroserviceIdentifier("id");
        yield return new TestCaseData(id, id).Returns(true);
        yield return new TestCaseData(id, new MicroserviceIdentifier("id")).Returns(true);
        yield return new TestCaseData(id, new MicroserviceIdentifier("test")).Returns(false);
        yield return new TestCaseData(id, null).Returns(false);
        yield return new TestCaseData(id, 3).Returns(false);
      }

      IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
  }
}