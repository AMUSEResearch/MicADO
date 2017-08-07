using System.IO;
using System.Linq;
using MicADO.Models.Deployment;
using MicADO.Models.Features;
using Newtonsoft.Json.Linq;

namespace MicADO.IO.Writers.Json.v2
{
  internal class LegacyDeploymentModelJsonWriter : IDeploymentModelWriter
  {
    private readonly string _filePath;

    public LegacyDeploymentModelJsonWriter(string FilePath)
    {
      _filePath = FilePath;
    }

    public void Write(DeploymentModel deploymentModel)
    {
      var root = new JObject(
        new JProperty("version", new JValue(2)),
        new JProperty("microservices", new JArray(deploymentModel.Microservices.Select(WriteMicroservice))),
        new JProperty("relations", new JArray(deploymentModel.FeatureModel.Relations.Select(WriteRelation)))
      );
      File.WriteAllText(_filePath, root.ToString());
    }

    private JObject WriteRelation(PropertyRelation relation)
    {
      return new JObject(
        new JProperty("sourceId", new JValue(relation.From.ToString())),
        new JProperty("targetId", new JValue(relation.To.ToString()))
      );
    }

    private JObject WriteMicroservice(Microservice microservice)
    {
      return new JObject(
        new JProperty("id", new JValue(microservice.Id.ToString())),
        new JProperty("name", new JValue(microservice.Id.ToString())),
        new JProperty("features", new JArray(microservice.Select(WriteFeature)))
      );
    }

    private JObject WriteFeature(FeatureInstance feature)
    {
      return new JObject(
        new JProperty("id", new JValue(feature.Feature.Id.ToString())),
        new JProperty("name", new JValue(feature.Name)),
        new JProperty("internal", new JValue(feature.IsInternal)),
        new JProperty("properties", new JArray(feature.Properties.Select(WriteProperty)))
      );
    }

    private JObject WriteProperty(Property property)
    {
      return new JObject(
        new JProperty("id", new JValue(property.Id.ToString())),
        new JProperty("name", new JValue(property.Name)),
        new JProperty("weight", new JValue(property.Weight))
      );
    }
  }
}