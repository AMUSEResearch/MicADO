using System.IO;
using MicADO.Models.Deployment;
using MicADO.IO.Readers.Json.v2;
using MicADO.IO.Readers.Json.v3;
using Newtonsoft.Json.Linq;

namespace MicADO.IO.Readers.Json
{
  public static class JsonParserFactory
  {
    public static IDeploymentModelReader GetDeploymentModelParser(string filePath)
    {
      JObject root = JObject.Parse(File.ReadAllText(filePath));
      var version = root["version"].Value<int>();
      switch(version)
      {
        case 2:
          return new LegacyJsonDeploymentModelReader(filePath);
        default:
          return new JsonDeploymentModelReader(filePath);
      }
    }

    public static int GetVersion(string filePath)
    {
      JObject root = JObject.Parse(File.ReadAllText(filePath));
      var version = root["version"].Value<int>();
      return version;
    }
  }
}