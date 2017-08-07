using System;
using System.Collections.Generic;
using System.Linq;
using MicADO.GeneticAlgorithm.Chromosome.Factory;
using MicADO.GeneticAlgorithm.Chromosome.Gene;
using MicADO.Models.Deployment;
using MicADO.Models.Features;
using MicADO.Models.Misc;

namespace MicADO.GeneticAlgorithm.Chromosome
{
  public class DeploymentChromosome : IDeploymentChromosome
  {
    public FeatureModel FeatureModel { get; }

    public double? Fitness { get; set; }

    public IReadOnlyCollection<IDeploymentGene> Genes => _genes.Values;

    private Dictionary<FeatureIdentifier, IDeploymentGene> _genes;

    public DeploymentChromosome(FeatureModel featureModel, IEnumerable<IDeploymentGene> genes)
    {
      FeatureModel = featureModel;
      _genes = genes.ToDictionary(gene => gene.FeatureId, gene => gene);
    }

    public DeploymentModel ToDeploymentModel()
    {
      var microservices = new Dictionary<MicroserviceIdentifier, List<FeatureInstance>>();

      // Add all feature main instances to their microservices
      foreach(var gene in Genes)
      {
        if(!microservices.TryGetValue(gene.MicroserviceId, out List<FeatureInstance> microservice))
        {
          microservice = new List<FeatureInstance>();
          microservices.Add(gene.MicroserviceId, microservice);
        }
        var feature = FeatureModel.GetFeature(gene.FeatureId);
        var allProperties = feature.Properties.Select(p => p.Id);
        microservice.Add(new FeatureInstance(feature, allProperties));
      }

      // Use the relations of the Feature model to find which features need which internal feature instances
      // This needs to be done recursively, since a dependency might also have dependencies in turn
      var dependenciesPerMicroservice = microservices.Keys.ToDictionary(microserviceId => microserviceId, microserviceId => new HashSet<PropertyIdentifier>());

      // A mapping to quickly lookup dependencies of a PropertyIdentifier
      var dependenciesPerPropertyId = FeatureModel.Relations.GroupBy(r => r.From, r => r).ToDictionary(group => group.Key, group => group.Select(g => g.To).ToList());

      foreach(var microservice in microservices)
      {
        // Find all dependencies using BFS
        var initialPropertyIds = microservice.Value.Select(f => f.Properties).SelectMany(ps => ps.Select(p => p.Id));
        var propertyIdsinMicroservice = new Queue<PropertyIdentifier>(initialPropertyIds);
        while(propertyIdsinMicroservice.Count > 0)
        {
          var propertyId = propertyIdsinMicroservice.Dequeue();
          if(dependenciesPerPropertyId.TryGetValue(propertyId, out List<PropertyIdentifier> dependencies))
          {
            foreach(var dependency in dependencies)
            {
              // If this dependency is not present in the microservice it will be added and we will look for it's dependencies as well
              if(dependenciesPerMicroservice[microservice.Key].Add(dependency))
              {
                propertyIdsinMicroservice.Enqueue(dependency);
              }
            }
          }
        }
      }

      // Build property to feature index
      var propertyToFeatureIndex = new Dictionary<PropertyIdentifier, FeatureIdentifier>();
      foreach(var feature in FeatureModel.Features)
      {
        foreach(var property in feature.Properties)
        {
          propertyToFeatureIndex[property.Id] = feature.Id;
        }
      }

      // Construct the feature instances with the required properties
      foreach(var microservice in microservices)
      {
        // Get all featureIds that are required as internal features in this microservice, we can omit the features already present in this microservice
        var requiredFeatureIds = new HashSet<FeatureIdentifier>(dependenciesPerMicroservice[microservice.Key].Select(p => propertyToFeatureIndex[p]))
          .Except(microservice.Value.Select(f => f.FeatureId));
        var requiredFeatures = requiredFeatureIds.Select(FeatureModel.GetFeature);
        foreach(var feature in requiredFeatures)
        {
          var featurePropertyIds = feature.Properties.Select(p => p.Id);
          // Select only the properties of the feature that are dependencies of other features in this microservice
          var includedProperties = featurePropertyIds.Intersect(dependenciesPerMicroservice[microservice.Key]);
          microservice.Value.Add(new FeatureInstance(feature, includedProperties, true));
        }
      }
      return new DeploymentModel(FeatureModel, microservices.Select(kvp => new Microservice(kvp.Value)));
    }

    public IDeploymentChromosome UpdateGene(IDeploymentGene gene)
    {
      var clonedGenes = _genes.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
      clonedGenes[gene.FeatureId] = gene;
      var newGenes = UpdateMicroserviceIdentifiers(clonedGenes);
      return new DeploymentChromosome(FeatureModel, newGenes);
    }

    public IDeploymentChromosome UpdateGenes(IReadOnlyCollection<IDeploymentGene> genes)
    {
      var clonedGenes = _genes.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
      foreach(var gene in genes)
      {
        clonedGenes[gene.FeatureId] = gene;
      }
      var newGenes = UpdateMicroserviceIdentifiers(clonedGenes).ToArray();
      return new DeploymentChromosome(FeatureModel, newGenes);
    }

    public IDeploymentGene GetGene(FeatureIdentifier featureId)
    {
      return _genes[featureId];
    }

    private IEnumerable<IDeploymentGene> UpdateMicroserviceIdentifiers(Dictionary<FeatureIdentifier, IDeploymentGene> newGenes)
    {
      var featuresGroupedByMicroserviceId = newGenes.GroupBy(g => g.Value.MicroserviceId, g => g.Value.FeatureId);
      // Since the MicroserviceId is derived from the features inside the microservice, recalculate the microserviceId for every microservice
      var featuresPerMicroservice =  featuresGroupedByMicroserviceId.ToDictionary(gs => gs.GetMicroserviceIdentifier(), gs => gs.ToArray());
      // Return the new genes with the correct microserviceId's
      foreach(var featuresInMicroservice in featuresPerMicroservice)
      {
        foreach(var feature in featuresInMicroservice.Value)
        {
          yield return new DeploymentGene(feature, featuresInMicroservice.Key);
        }
      }
    }

    public override bool Equals(object obj)
    {
      DeploymentChromosome chromosome = obj as DeploymentChromosome;
      return chromosome != null && FeatureModel.Equals(chromosome.FeatureModel) && Genes.OrderBy(g => g.FeatureId).SequenceEqual(chromosome.Genes.OrderBy(g => g.FeatureId));
    }

    public override int GetHashCode()
    {
      unchecked
      {
        var hashCode = HashConstants.OffsetBasis;
        hashCode = (hashCode ^ (FeatureModel.GetHashCode())) * HashConstants.Prime;
        foreach(var gene in Genes.OrderBy(g => g.FeatureId))
        {
          hashCode = (hashCode ^ (gene.GetHashCode())) * HashConstants.Prime;
        }
        return hashCode;
      }
    }

    public override string ToString()
    {
      var groups = Genes.GroupBy(g => g.MicroserviceId, g => g);
      return "[" + string.Join(", ", groups.Select(gs => $"({gs.Key} : {string.Join(", ", gs.Select(g => g.FeatureId))} )")) + "]";
    }
  }
}