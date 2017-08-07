using CommandLine;

namespace MicADO.Visualizer
{
  public class Options
  {
    [Option('d', "deployment", HelpText = "The deploymentmodel that should be visualized", Required = true)]
    public string DeploymentModelFilePath { get; set; }

    [Option('o', "output", HelpText = "Filepath to the output svg file", Required = true)]
    public string OutputPath { get; set; }

    [Option('r', "render", HelpText = "Filepath to the dot.exe binary", Required = true)]
    public string DotExePath { get; set; }
  }
}