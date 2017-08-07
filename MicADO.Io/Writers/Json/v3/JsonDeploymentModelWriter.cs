using System;
using System.IO;
using System.Linq;
using MicADO.Models.Deployment;
using MicADO.Models.Features;
using Newtonsoft.Json.Linq;

namespace MicADO.IO.Writers.Json.v3
{
  internal class JsonDeploymentModelWriter : IDeploymentModelWriter
  {
    private readonly string _filePath;

    public JsonDeploymentModelWriter(string FilePath)
    {
      _filePath = FilePath;
    }

    public void Write(DeploymentModel deploymentModel)
    {
      var root = new JObject(
        new JProperty("version", new JValue(3)),
        new JProperty("features", new JArray(deploymentModel.FeatureModel.Features.Select(WriteFeature))),
        new JProperty("microservices", new JArray(deploymentModel.Microservices.Select(WriteMicroservice))),
        new JProperty("relations", new JArray(deploymentModel.FeatureModel.Relations.Select(WriteRelation)))
      );
      File.WriteAllText(_filePath, root.ToString());
    }

    private JObject WriteFeature(Feature feature)
    {
      return new JObject(
        new JProperty("id", new JValue(feature.Id.ToString())),
        new JProperty("name", new JValue(feature.Name)),
        new JProperty("properties", new JArray(feature.Properties.Select(WriteProperty)))
      );
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
        new JProperty("features", new JArray(microservice.Select(WriteFeatureInstance)))
      );
    }

    private JObject WriteFeatureInstance(FeatureInstance featureInstance)
    {
      return new JObject(
        new JProperty("type", new JValue(featureInstance.Feature.Id.ToString())),
        new JProperty("internal", new JValue(featureInstance.IsInternal)),
        new JProperty("properties", new JArray(featureInstance.Properties.Select(p => p.Id.ToString())))
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