using MicADO.GeneticAlgorithm.State;
using NUnit.Framework;

namespace MicADO.GeneticAlgorithm.Tests.State
{
  [TestFixture]
  public class GenerationCountStateTests
  {
    [Test]
    public void UpdateOnGenerationRan_Updates_GenerationCount_Correctly()
    {
      var sot = new GenerationCountState();

      Assert.AreEqual(0, sot.GenerationCount);
      sot.UpdateOnGenerationRan(null);
      Assert.AreEqual(1, sot.GenerationCount);
    }
  }
}