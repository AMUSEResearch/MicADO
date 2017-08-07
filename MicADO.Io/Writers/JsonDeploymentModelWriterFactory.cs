using MicADO.IO.Writers.Json.v2;
using MicADO.IO.Writers.Json.v3;
using MicADO.Models.Deployment;

namespace MicADO.IO.Writers
{
  public static class JsonDeploymentModelWriterFactory
  {
    public static void Write(int version, DeploymentModel deploymentModel, string filePath)
    {
      IDeploymentModelWriter writer;
      switch(version)
      {
        case 2:
          writer = new LegacyDeploymentModelJsonWriter(filePath);
          break;
        default:
          writer = new JsonDeploymentModelWriter(filePath);
          break;
      }
      writer.Write(deploymentModel);
    }
  }
}