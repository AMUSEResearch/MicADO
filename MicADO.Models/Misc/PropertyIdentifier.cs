using System;

namespace MicADO.Models.Misc
{
  /// <summary>
  ///   Identifier of a property
  /// </summary>
  public class PropertyIdentifier : IComparable<PropertyIdentifier>
  {
    public PropertyIdentifier(string id)
    {
      Id = id;
    }

    /// <summary>
    ///   Gets the identifier.
    /// </summary>
    /// <value>
    ///   The identifier.
    /// </value>
    public string Id { get; }

    /// <summary>
    ///   Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>
    ///   A <see cref="System.String" /> that represents this instance.
    /// </returns>
    public override string ToString()
    {
      return Id;
    }

    public override bool Equals(object obj)
    {
      PropertyIdentifier otherIdentifier = obj as PropertyIdentifier;
      return otherIdentifier != null && Id.Equals(otherIdentifier.Id);
    }

    public override int GetHashCode()
    {
      return Id.GetHashCode();
    }

    public int CompareTo(PropertyIdentifier other)
    {
      return other == null ? 1 : Id.CompareTo(other.Id);
    }
  }
}