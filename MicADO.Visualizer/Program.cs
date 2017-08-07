using System;
using System.Diagnostics;
using System.Linq;
using CommandLine;
using MicADO.IO.Readers.Json;

namespace MicADO.Visualizer
{
  class Program
  {
    static int Main(string[] args)
    {
      var result = Parser.Default.ParseArguments<Options>(args)
        .MapResult(
          options => RunVisualize(options),
          _ => 1);
      Console.WriteLine("Press any key to exit");
      Console.Read();
      return result;

    }

    private static int RunVisualize(Options options)
    {
      try
      {
        var deploymentModelParser = JsonParserFactory.GetDeploymentModelParser(options.DeploymentModelFilePath);
        var deploymentModel = deploymentModelParser.Read();
        var startInfo = new ProcessStartInfo
        {
          FileName = options.DotExePath,
          Arguments = $"-Tsvg -o {options.OutputPath}",
          UseShellExecute = false,
          CreateNoWindow = true,
          RedirectStandardInput = true,
          RedirectStandardOutput = true,
          RedirectStandardError = true
        };

        using(var process = Process.Start(startInfo))
        {
          using(var standardInput = process.StandardInput)
          {
            standardInput.WriteLine("digraph G {");
            standardInput.WriteLine("forcelabels=true;");
            foreach(var microservice in deploymentModel.Microservices)
            {
              standardInput.WriteLine($"\tsubgraph cluster_{microservice.Id} {{");
              standardInput.WriteLine($"\t\t label = \"{microservice.Id}\";");
              foreach(var featureInstance in microservice)
              {
                var fillcolor = featureInstance.IsInternal ? "white" : "#b7c4d5";
                standardInput.WriteLine($"\t\t{microservice.Id}_{featureInstance.Feature.Id}[shape=plaintext,label= <");
                standardInput.WriteLine($"<table bgcolor=\"{fillcolor}\" BORDER=\"0\" CELLBORDER=\"1\" CELLSPACING=\"0\">");
                standardInput.WriteLine($"<tr><td ALIGN=\"CENTER\" COLSPAN=\"{featureInstance.Properties.Count()}\">{featureInstance.Name}</td></tr>");
                standardInput.WriteLine("<tr>");
                foreach(var property in featureInstance.Properties)
                {
                  standardInput.WriteLine($"<td>{property.Name}</td>");
                }
                standardInput.WriteLine("</tr>");
                standardInput.WriteLine("</table>>]");
              }
              standardInput.WriteLine($"\t}}");
            }
            standardInput.WriteLine("}");
            standardInput.Close();
          }
          process.WaitForExit();
          var exitCode = process.ExitCode;
          if(exitCode != 0)
          {
            var output = process.StandardError.ReadToEnd();
            Console.WriteLine("An error occurred during visualization");
            Console.WriteLine();
            Console.WriteLine(output);
            return 1;
          }
          Console.WriteLine($"Deploymentmodel has been visualized in {options.OutputPath}");
          return 0;
        }
      }
      catch(Exception e)
      {
        Console.WriteLine("An error occured");
        Console.WriteLine();
        Console.WriteLine(e.Message);
        return 1;
      }
    }
  }
}
