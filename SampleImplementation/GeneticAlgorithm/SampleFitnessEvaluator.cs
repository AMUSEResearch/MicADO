using System;
using System.Collections.Generic;
using System.Linq;
using MicADO.GeneticAlgorithm.Chromosome;
using MicADO.GeneticAlgorithm.FitnessEvaluators;
using MicADO.GeneticAlgorithm.FitnessEvaluators.Workload;
using SampleImplementation.GeneticAlgorithm.Workload;

namespace SampleImplementation.GeneticAlgorithm
{
  public class SampleFitnessEvaluator : IFitnessEvaluator<SampleWorkload>
  {
    public double Evaluate(IDeploymentChromosome deployment, SampleWorkload workload)
    {
      var deploymentModel = deployment.ToDeploymentModel();
      var microserviceQueueingTheoryInfo = new List<QueueingTheoryInfo>();
      foreach(var microservice in deploymentModel.Microservices)
      {
        // Get the queueing theory representation for every type of event that this microservice listens to
        var customerTypes = microservice.Select(workload.GetQueueingTheoryInfo);

        // Calculate the summation of this these customer classes
        microserviceQueueingTheoryInfo.Add(customerTypes.Aggregate((result, current) => result + current));
      }
      // In this case we want to minimize the mean sojourn time
      // For real usage you would want more complex calculations here
      return -microserviceQueueingTheoryInfo.Select(q => q.SojournTime).Average();
    }
  }
}