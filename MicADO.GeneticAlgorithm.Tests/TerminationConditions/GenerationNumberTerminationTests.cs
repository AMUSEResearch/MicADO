using System;
using FakeItEasy;
using MicADO.GeneticAlgorithm.State;
using MicADO.GeneticAlgorithm.TerminationConditions;
using NUnit.Framework;

namespace MicADO.GeneticAlgorithm.Tests.TerminationConditions
{
  [TestFixture]
  public class GenerationNumberTerminationTests
  {
    [Test]
    public void Constructor_Sets_Fields_Correctly()
    {
      var sot = new GenerationNumberTermination(10);

      Assert.AreEqual(10, sot.MaximalNumberOfGenerations);
    }

    [Test]
    public void HasReached_Returns_True_WhenGenerationCountIsLargerThanMaximalNumberOfGenerations()
    {
      var sot = new GenerationNumberTermination(10);
      var state = new GenerationCountState();
      for(var i = 0; i < 10; i++)
      {
        Assert.False(sot.HasReached(null, state));
        state.UpdateOnGenerationRan(null);
      }
      Assert.True(sot.HasReached(null, state));
    }
  }
}