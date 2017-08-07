using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicADO.IO.Readers.Json;
using MicADO.IO.Writers;
using MicADO.Models.Deployment;
using MicADO.Models.Features;
using MicADO.Models.Misc;
using NUnit.Framework;

namespace MicADO.IO.Tests
{
  [TestFixture]
  class ReaderWriterTests
  {
    [Test]
    public void ReadV2_Written_DeploymentModel_Returns_SameDeploymentModel()
    {
        var tmpFile = Path.GetTempFileName();
        var deploymentModel = CreateDeploymentModel();
        JsonDeploymentModelWriterFactory.Write(2, deploymentModel, tmpFile);
        var reader = JsonParserFactory.GetDeploymentModelParser(tmpFile);
        var version = JsonParserFactory.GetVersion(tmpFile);
        var readModel = reader.Read();
        Assert.AreEqual(2, version);
        Assert.AreEqual(deploymentModel, readModel);
    }

    [Test]
    public void ReadV3_Written_DeploymentModel_Returns_SameDeploymentModel()
    {
      var tmpFile = Path.GetTempFileName();
      var deploymentModel = CreateDeploymentModel();
      JsonDeploymentModelWriterFactory.Write(3, deploymentModel, tmpFile);
      var reader = JsonParserFactory.GetDeploymentModelParser(tmpFile);
      var version = JsonParserFactory.GetVersion(tmpFile);
      var readModel = reader.Read();
      Assert.AreEqual(3, version);
      Assert.AreEqual(deploymentModel, readModel);
    }

    private DeploymentModel CreateDeploymentModel()
    {
      var aFeature = new Feature(new FeatureIdentifier("a"), "a", new[]
        {
          new Property(new PropertyIdentifier("p1"), "p1"),
          new Property(new PropertyIdentifier("p2"), "p2")
        }
      );

      var bFeature = new Feature(new FeatureIdentifier("b"), "b", new[]
        {
          new Property(new PropertyIdentifier("p3"), "p3"),
          new Property(new PropertyIdentifier("p4"), "p4")
        }
      );

      var cFeature = new Feature(new FeatureIdentifier("c"), "c", new[]
        {
          new Property(new PropertyIdentifier("p5"), "p5"),
          new Property(new PropertyIdentifier("p6"), "p6"),
          new Property(new PropertyIdentifier("p7"), "p7")
        }
      );

      var featureModel = new FeatureModel(new[] { aFeature, bFeature, cFeature }, new[]
      {
        new PropertyRelation(new PropertyIdentifier("p6"), new PropertyIdentifier("p2")),
        new PropertyRelation(new PropertyIdentifier("p7"), new PropertyIdentifier("p4"))
      });


      return new DeploymentModel(featureModel, new[]
      {
        new Microservice(new[] { new FeatureInstance(aFeature, new[] { new PropertyIdentifier("p1"), new PropertyIdentifier("p2") }) }),
        new Microservice(new[] { new FeatureInstance(bFeature, new[] { new PropertyIdentifier("p3"), new PropertyIdentifier("p4") }) }),
        new Microservice(new[]
        {
          new FeatureInstance(cFeature, new[] { new PropertyIdentifier("p5"), new PropertyIdentifier("p6"), new PropertyIdentifier("p7") }),
          new FeatureInstance(bFeature, new[] { new PropertyIdentifier("p4") }, true),
          new FeatureInstance(aFeature, new[] { new PropertyIdentifier("p2") }, true)
        }),
      });
    }
  }
}
