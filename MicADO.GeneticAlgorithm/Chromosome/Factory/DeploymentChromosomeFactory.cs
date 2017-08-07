using System;
using System.Collections.Generic;
using System.Linq;
using MicADO.GeneticAlgorithm.Chromosome.Gene;
using MicADO.Models.Deployment;
using MicADO.Models.Misc;

namespace MicADO.GeneticAlgorithm.Chromosome.Factory
{
  public class DeploymentChromosomeFactory : IDeploymentChromosomeFactory
  {
    public IDeploymentChromosome Create(DeploymentModel deploymentModel)
    {
      var featureModel = deploymentModel.FeatureModel;
      var genes = new List<IDeploymentGene>();
      foreach(var microservice in deploymentModel.Microservices)
      {
        foreach(var feature in microservice.Where(f => !f.IsInternal))
        {
          genes.Add(new DeploymentGene(feature.FeatureId, microservice.Id));
        }
      }
      return new DeploymentChromosome(featureModel, genes);
    }
  }
}