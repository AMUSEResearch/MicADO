using System;

namespace MicADO.Models.Misc
{
  /// <summary>
  ///   Identifier for Features
  /// </summary>
  public class FeatureIdentifier : IComparable<FeatureIdentifier>
  {
    /// <summary>
    ///   Initializes a new instance of the <see cref="FeatureIdentifier" /> class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    public FeatureIdentifier(string id)
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

    /// <summary>
    ///   Determines whether the specified <see cref="System.Object" />, is equal to this instance.
    /// </summary>
    /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
    /// <returns>
    ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object obj)
    {
      FeatureIdentifier featureIdentifierObj = obj as FeatureIdentifier;
      return featureIdentifierObj != null && Id.Equals(featureIdentifierObj.Id);
    }

    public override int GetHashCode()
    {
      return Id.GetHashCode();
    }

    public int CompareTo(FeatureIdentifier other)
    {
      return other == null ? 1 : Id.CompareTo(other.Id);
    }
  }
}