using System.Collections.Generic;
using System.Linq;
using MicADO.Models.Misc;

namespace MicADO.Models.Features
{
  public class Feature
  {
    private Dictionary<PropertyIdentifier, Property> _properties;

    public Feature(FeatureIdentifier id, string name, IEnumerable<Property> properties)
    {
      Id = id;
      Name = name;
      _properties = properties.ToDictionary(p => p.Id, p => p);
    }

    public FeatureIdentifier Id { get; }

    public string Name { get; }

    public IEnumerable<Property> Properties => _properties.Values.AsEnumerable();

    public override string ToString()
    {
      return $"{Id} ({string.Join(", ", Properties)})";
    }

    public override bool Equals(object obj)
    {
      Feature featureObj = obj as Feature;
      return featureObj != null && Id.Equals(featureObj.Id) && Name.Equals(Name) && Properties.OrderBy(p => p.Id).SequenceEqual(featureObj.Properties.OrderBy(p => p.Id));
    }

    public override int GetHashCode()
    {
      unchecked
      {
        var hashCode = HashConstants.OffsetBasis;
        hashCode = (hashCode ^ (Id.GetHashCode())) * HashConstants.Prime;
        hashCode = (hashCode ^ (Name.GetHashCode())) * HashConstants.Prime;
        foreach(var property in Properties.OrderBy(p => p.Id))
        {
          hashCode = (hashCode ^ (property.GetHashCode())) * HashConstants.Prime;
        }
        return hashCode;
      }
    }
  }
}