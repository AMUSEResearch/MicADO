using System;
using System.IO;
using CommandLine;
using MicADO.IO.Readers.Json;
using MicADO.IO.Writers;
using SampleImplementation.GeneticAlgorithm;

namespace SampleImplementation
{
  class Program
  {
    static int Main(string[] args)
    {
      var result = Parser.Default.ParseArguments<Options>(args)
        .MapResult(
          options => RunOptimize(options),
          _ => 1);
      Console.WriteLine("Press any key to exit");
      Console.Read();
      return result;
    }

    private static int RunOptimize(Options options)
    {
      var geneticAlgorithm = new SampleGeneticAlgorithm();
      try
      {
        var deploymentModelParser = JsonParserFactory.GetDeploymentModelParser(options.DeploymentModelFilePath);
        var version = JsonParserFactory.GetVersion(options.DeploymentModelFilePath);
        var deploymentModel = deploymentModelParser.Read();
        var bestDeploymentModel = geneticAlgorithm.Run(deploymentModel);
        var outputPath = options.OutputDeploymentModelFilePath;
        if(string.IsNullOrEmpty(outputPath))
        {
          var dirInfo = Path.GetDirectoryName(Path.GetFullPath(options.DeploymentModelFilePath));
          var fileInfo = Path.GetFileName(options.DeploymentModelFilePath);
          outputPath = dirInfo + "/optimized_" + fileInfo;
        }
        Console.WriteLine("Outputting optimized deploymentmodel to " + outputPath);
        JsonDeploymentModelWriterFactory.Write(version, bestDeploymentModel, outputPath);
      }
      catch(Exception exception)
      {
        Console.WriteLine(exception.Message);
        return 1;
      }
      return 0;
    }
  }
}
