using MicADO.Models.Misc;

namespace MicADO.Models.Features
{
  public class Property
  {
    public Property(PropertyIdentifier id, string name, int weight = 1)
    {
      Id = id;
      Name = name;
      Weight = weight;
    }

    public PropertyIdentifier Id { get; }

    public string Name { get; }

    public int Weight { get; }

    public override bool Equals(object obj)
    {
      Property propertyObj = obj as Property;
      return propertyObj != null && Id.Equals(propertyObj.Id) && Name.Equals(propertyObj.Name) && Weight.Equals(propertyObj.Weight);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        var hashCode = HashConstants.OffsetBasis;
        hashCode = (hashCode ^ (Id.GetHashCode())) * HashConstants.Prime;
        hashCode = (hashCode ^ (Name.GetHashCode())) * HashConstants.Prime;
        hashCode = (hashCode ^ (Weight.GetHashCode())) * HashConstants.Prime;
        return hashCode;
      }
    }

    public override string ToString()
    {
      return Id.ToString();
    }
  }
}