using System;

namespace MicADO.Models.Misc
{
  public class MicroserviceIdentifier : IComparable<MicroserviceIdentifier>
  {
    /// <summary>
    ///   Initializes a new instance of the <see cref="MicroserviceIdentifier" /> class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    public MicroserviceIdentifier(string id)
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
      MicroserviceIdentifier microserviceIdentifier = obj as MicroserviceIdentifier;
      return microserviceIdentifier != null && Id.Equals(microserviceIdentifier.Id);
    }

    public override int GetHashCode()
    {
      return Id.GetHashCode();
    }

    public int CompareTo(MicroserviceIdentifier other)
    {
      return other == null ? 1 : Id.CompareTo(other.Id);
    }

  }
}