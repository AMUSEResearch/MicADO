using System.IO;
using System.Linq;
using MicADO.Models.Deployment;
using MicADO.Models.Features;
using MicADO.Models.Misc;
using Newtonsoft.Json.Linq;

namespace MicADO.IO.Readers.Json.v3
{
  internal class JsonDeploymentModelReader : IDeploymentModelReader
  {
    private readonly string _filePath;
    private FeatureModel _featureModel;

    public JsonDeploymentModelReader(string FilePath)
    {
      _filePath = FilePath;
    }

    public DeploymentModel Read()
    {
      JObject root = JObject.Parse(File.ReadAllText(_filePath));
      _featureModel = ParseFeatureModel(root);
      var microservices = root["microservices"].Select(ParseMicroservice);
      return new DeploymentModel(_featureModel, microservices);
    }

    private Microservice ParseMicroservice(JToken microserviceJson)
    {
      var features = microserviceJson["features"].Select(ParseFeatureInstance);
      return new Microservice(features);
    }

    private FeatureInstance ParseFeatureInstance(JToken featureJson)
    {
      var featureId = new FeatureIdentifier(featureJson["type"].Value<string>());
      var feature = _featureModel.GetFeature(featureId);
      var propertyIdentifiers = featureJson["properties"].Select(p => new PropertyIdentifier(p.Value<string>()));
      var isInternal = featureJson["internal"].Value<bool>();
      return new FeatureInstance(feature, propertyIdentifiers, isInternal);
    }

    private FeatureModel ParseFeatureModel(JObject root)
    {
      var features = root["features"].Select(ParseFeature);
      var relations = root["relations"].Select(ParseRelation);
      return new FeatureModel(features, relations);
    }

    private Feature ParseFeature(JToken featureJson)
    {
      var featureIdentifier = new FeatureIdentifier(featureJson["id"].Value<string>());
      var name = featureJson["name"].Value<string>();
      var properties = featureJson["properties"].Select(ParseProperty);
      return new Feature(featureIdentifier, name, properties);
    }

    private Property ParseProperty(JToken propertyJson)
    {
      var propertyIdentifier = new PropertyIdentifier(propertyJson["id"].Value<string>());
      var name = propertyJson["name"].Value<string>();
      return new Property(propertyIdentifier, name);
    }

    private PropertyRelation ParseRelation(JToken relationJson)
    {
      PropertyIdentifier from = new PropertyIdentifier(relationJson["sourceId"].Value<string>());
      PropertyIdentifier to = new PropertyIdentifier(relationJson["targetId"].Value<string>());
      return new PropertyRelation(from, to);
    }
  }
}