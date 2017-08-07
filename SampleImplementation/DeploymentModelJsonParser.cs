using System;
using System.IO;
using MicADO.Models.Deployment;

namespace SampleImplementation
{
  public class DeploymentModelJsonParser : IDeploymentModelParser
  {
    private readonly string _filePath;

    public DeploymentModelJsonParser(string FilePath)
    {
      _filePath = FilePath;
    }

    public DeploymentModel Parse()
    {
      throw new NotImplementedException();
    }
  }
}