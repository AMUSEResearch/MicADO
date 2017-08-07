using System;
using System.Linq;
using MicADO.GeneticAlgorithm.FitnessEvaluators.Workload;
using MicADO.Models.Misc;
using NUnit.Framework;

namespace MicADO.GeneticAlgorithm.Tests.FitnessEvaluators.Workload
{
  [TestFixture]
  public class QueueingTheoryInfoTests
  {
    [Test]
    public void Constructor_Sets_Fields_Correctly()
    {
      var types = new[] { "test" };
      var sot = new QueueingTheoryInfo(10, 5, 1, types);
      Assert.AreEqual(10, sot.MeanInterArrivalTime);
      Assert.AreEqual(5, sot.MeanServiceTime);
      Assert.AreEqual(1, sot.ChanceOfOccurance);
      Assert.AreEqual(0.1d, sot.MeanArrivalRate);
      Assert.AreEqual(0.2d, sot.MeanServiceRate);
      Assert.AreEqual(10, sot.MeanWaitingTime);
      Assert.AreEqual(15, sot.SojournTime);
      CollectionAssert.AreEqual(types.Select(t => new FeatureIdentifier(t)), sot.Types);
      
    }

    [Test]
    public void Constructor_WithUtilizationLargerThan1_ThrowsException()
    {
      Assert.Throws<ArgumentException>(() =>
      {
        var types = new[] { "test" };
        var sot = new QueueingTheoryInfo(5, 10, 1, types);
      });
    }

    [Test]
    public void PlusOperator_Returns_CorrectResult()
    {
      var firstTypes = new[] { "first" };
      var firstClass = new QueueingTheoryInfo(50, 5, 0.5, firstTypes);

      var secondTypes = new[] { "second" };
      var secondClass = new QueueingTheoryInfo(20, 10, 0.5, secondTypes);

      var result = firstClass + secondClass;
      Assert.AreEqual(35, result.MeanInterArrivalTime);
      Assert.AreEqual(7.5d, result.MeanServiceTime);
      var newTypes = new[] { new FeatureIdentifier("first"), new FeatureIdentifier("second") };
      CollectionAssert.AreEqual(newTypes, result.Types);
    }
  }
}