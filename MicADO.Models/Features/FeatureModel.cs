using System;
using System.Collections.Generic;
using System.Linq;
using MicADO.Models.Misc;

namespace MicADO.Models.Features
{
  public class FeatureModel
  {
    private Dictionary<FeatureIdentifier, Feature> _features;

    private Dictionary<PropertyIdentifier, List<PropertyIdentifier>> _propertyDependencies;

    public FeatureModel(IEnumerable<Feature> features, IEnumerable<PropertyRelation> relations)
    {
      _features = features.ToDictionary(f => f.Id, f => f);
      var properties = new HashSet<PropertyIdentifier>(features.SelectMany(f => f.Properties).Select(p => p.Id));
      var invalidReferences = relations.Where(r => !properties.Contains(r.From) || !properties.Contains(r.To));
      if(invalidReferences.Any())
      {
        throw new ArgumentException($"Relations contains references to a non existing feature {string.Join(", ", invalidReferences)}");
      }
      _propertyDependencies = relations.GroupBy(r => r.From, r => r.To).ToDictionary(kvp => kvp.Key, kvp => kvp.ToList());
    }

    public IEnumerable<Feature> Features => _features.Values.AsEnumerable();

    public IEnumerable<PropertyRelation> Relations => _propertyDependencies.SelectMany(kvp => kvp.Value.Select(to => new PropertyRelation(kvp.Key, to))).AsEnumerable();

    public Feature GetFeature(FeatureIdentifier featureId)
    {
      return _features[featureId];
    }

    public IEnumerable<PropertyRelation> GetRelations(FeatureIdentifier featureId)
    {
      var feature = GetFeature(featureId);
      var relations = new List<PropertyRelation>();
      foreach(var property in feature.Properties)
      {
        if (_propertyDependencies.TryGetValue(property.Id, out List<PropertyIdentifier> dependencies))
        {
          relations.AddRange(dependencies.Select(d => new PropertyRelation(property.Id, d)));
        }
      }
      return relations;
    }

    public override bool Equals(object obj)
    {
      FeatureModel featureModel = obj as FeatureModel;
      return featureModel != null 
        && Features.OrderBy(f => f.Id).SequenceEqual(featureModel.Features.OrderBy(f => f.Id)) 
        && Relations.OrderBy(r => r.From).ThenBy(r => r.To).SequenceEqual(featureModel.Relations.OrderBy(r => r.From).ThenBy(r => r.To));
    }

    public override int GetHashCode()
    {
      unchecked
      {
        var hashCode = HashConstants.OffsetBasis;
        foreach(var feature in Features.OrderBy(r => r.Id))
        {
          hashCode = (hashCode ^ (feature.GetHashCode())) * HashConstants.Prime;
        }
        foreach(var relation in Relations.OrderBy(r => r.From).ThenBy(r => r.To))
        {
          hashCode = (hashCode ^ (relation.GetHashCode())) * HashConstants.Prime;
        }
        return hashCode;
      }
    }
  }
}