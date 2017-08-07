using System;
using System.Collections.Generic;
using System.Linq;
using MicADO.Models.Features;
using MicADO.Models.Misc;

namespace MicADO.Models.Deployment
{
  /// <summary>
  ///   Model of an instance of a Feature
  /// </summary>
  /// <seealso cref="Property" />
  public class FeatureInstance
  {
    private Dictionary<PropertyIdentifier, Property> _properties;

    /// <summary>
    ///   Initializes a new instance of the <see cref="FeatureInstance" /> class.
    /// </summary>
    /// <param name="feature">The feature.</param>
    /// <param name="includedProperties">The included properties.</param>
    public FeatureInstance(Feature feature, IEnumerable<PropertyIdentifier> includedProperties, bool isInternal = false)
    {
      Feature = feature;
      _properties = feature.Properties.Where(p => includedProperties.Contains(p.Id)).ToDictionary(p => p.Id, p => p);
      if(!_properties.Any())
      {
        throw new ArgumentException("A featureInstance should have at least one property.");
      }
      if(isInternal == false && !feature.Properties.Select(p => p.Id).SequenceEqual(includedProperties))
      {
        throw new ArgumentException("A public feature instance should always contain all properties of a feature");
      }
      IsInternal = isInternal;
    }

    public Feature Feature { get; }

    /// <summary>
    ///   Gets the type.
    /// </summary>
    /// <value>
    ///   The type.
    /// </value>
    public FeatureIdentifier FeatureId => Feature.Id;

    public bool IsInternal { get; }

    /// <summary>
    ///   Gets the name.
    /// </summary>
    /// <value>
    ///   The name.
    /// </value>
    public string Name => Feature.Name;

    public IEnumerable<Property> Properties => _properties.Values.AsEnumerable();

    public override bool Equals(object obj)
    {
      FeatureInstance featureInstance = obj as FeatureInstance;
      return featureInstance != null && Feature.Equals(featureInstance.Feature) && IsInternal.Equals(featureInstance.IsInternal) &&
             Properties.OrderBy(p => p.Id).SequenceEqual(featureInstance.Properties.OrderBy(p => p.Id));
    }

    public override int GetHashCode()
    {
      unchecked
      {
        var hashCode = HashConstants.OffsetBasis;
        hashCode = (hashCode ^ (Feature.GetHashCode())) * HashConstants.Prime;
        hashCode = (hashCode ^ (IsInternal.GetHashCode())) * HashConstants.Prime;
        foreach(var property in Properties.OrderBy(p => p.Id))
        {
          hashCode = (hashCode ^ (property.GetHashCode())) * HashConstants.Prime;
        }
        return hashCode;
      }
    }

    public override string ToString()
    {
      return $"{(IsInternal ? "(internal)" : "")} {Feature.Id} ({string.Join(", ", _properties.Values)})";
    }
  }
}