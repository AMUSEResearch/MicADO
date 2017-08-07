using MicADO.Models.Misc;

namespace MicADO.Models.Features
{
  /// <summary>
  ///   Model of a relation (i.e. dependency) between two properties
  /// </summary>
  public class PropertyRelation
  {
    public PropertyRelation(PropertyIdentifier from, PropertyIdentifier to)
    {
      From = from;
      To = to;
    }

    public PropertyIdentifier From { get; }

    public PropertyIdentifier To { get; }

    public override string ToString()
    {
      return $"{From} -> {To}";
    }

    public override bool Equals(object obj)
    {
      PropertyRelation propertyRelation = obj as PropertyRelation;
      return propertyRelation != null && From.Equals(propertyRelation.From) && To.Equals(propertyRelation.To);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        var hashCode = HashConstants.OffsetBasis;
        hashCode = (hashCode ^ (From.GetHashCode())) * HashConstants.Prime;
        hashCode = (hashCode ^ (To.GetHashCode())) * HashConstants.Prime;
        return hashCode;
      }
    }
  }
}