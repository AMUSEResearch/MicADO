using CommandLine;

namespace SampleImplementation
{
  public class Options
  {
    [Option('d', "deployment", HelpText = "Filepath to a deploymentmodel", Required = true)]
    public string DeploymentModelFilePath { get; set; }

    [Option('o', "output", HelpText = "Filepath to which the optimized deploymentmodel will be outputted")]
    public string OutputDeploymentModelFilePath { get; set; }

  }
}