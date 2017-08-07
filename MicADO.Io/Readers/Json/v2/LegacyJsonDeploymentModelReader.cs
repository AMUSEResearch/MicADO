using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MicADO.Models.Deployment;
using MicADO.Models.Features;
using MicADO.Models.Misc;
using Newtonsoft.Json.Linq;

namespace MicADO.IO.Readers.Json.v2
{
  internal class LegacyJsonDeploymentModelReader : IDeploymentModelReader
  {
    private string _filePath;
    private FeatureModel _featureModel;

    public LegacyJsonDeploymentModelReader(string FilePath)
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

    private FeatureInstance ParseFeatureInstance(JToken featureInstanceJson)
    {
      var isInternal = featureInstanceJson["internal"].Value<bool>();
      var id = featureInstanceJson["id"].Value<string>();
      var feature = _featureModel.GetFeature(new FeatureIdentifier(id));
      var includedProperties = featureInstanceJson["properties"].Select(ParsePropertyInstance);
      return new FeatureInstance(feature, includedProperties, isInternal);
    }

    private PropertyIdentifier ParsePropertyInstance(JToken propertyInstanceJson)
    {
      string id = propertyInstanceJson["id"].Value<string>();
      return new PropertyIdentifier(id);
    }

    private FeatureModel ParseFeatureModel(JObject root)
    {
      var features = root["microservices"].SelectMany(ParseFeaturesFromMicroservice);
      var relations = root["relations"].Select(ParseRelation);
      return new FeatureModel(features, relations);
    }

    private PropertyRelation ParseRelation(JToken relationJson)
    {
      PropertyIdentifier from = new PropertyIdentifier(relationJson["sourceId"].Value<string>());
      PropertyIdentifier to = new PropertyIdentifier(relationJson["targetId"].Value<string>());
      return new PropertyRelation(from, to);
    }

    private IEnumerable<Feature> ParseFeaturesFromMicroservice(JToken microserviceJson)
    {
      return microserviceJson["features"].Where(json => !json["internal"].Value<bool>()).Select(ParseFeature);
    }

    private Feature ParseFeature(JToken featureJson)
    {
      var id = featureJson["id"].Value<string>();
      var name = featureJson["name"].Value<string>();
      var properties = featureJson["properties"].Select(CreateProperty);
      return new Feature(new FeatureIdentifier(id), name, properties);
    }

    private Property CreateProperty(JToken propertyJson)
    {
      var id = propertyJson["id"].Value<string>();
      var name = propertyJson["name"].Value<string>();
      var weight = propertyJson["weight"].Value<int>();

      return new Property(new PropertyIdentifier(id), name, weight);
    }
  }
}