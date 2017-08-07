using System.Diagnostics.CodeAnalysis;

namespace MicADO.Models.Misc
{
  /// <summary>
  ///   Static class to define the hash constants for the FNV-1a hashing algorithm we use
  ///   for reasoning see:
  ///   http://www.isthe.com/chongo/tech/comp/fnv/
  ///   http://stackoverflow.com/questions/13974443/c-sharp-implementation-of-fnv-hash
  ///   http://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode/263416#263416
  /// </summary>
  [ExcludeFromCodeCoverage]
  public static class HashConstants
  {
    /// <summary>The offset basis</summary>
    public static readonly int OffsetBasis = -2128831035;
    /// <summary>The prime</summary>
    public static readonly int Prime = 16777619;
  }
}