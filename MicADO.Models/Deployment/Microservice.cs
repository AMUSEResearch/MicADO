using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MicADO.Models.Misc;

namespace MicADO.Models.Deployment
{
  /// <summary>
  ///   Model of a Microservice
  /// </summary>
  /// <seealso cref="System.Collections.Generic.IEnumerable{MicADO.Models.DeploymentModel.FeatureInstance}" />
  public class Microservice : IEnumerable<FeatureInstance>
  {
    private Dictionary<FeatureIdentifier, FeatureInstance> _features;

    public Microservice(IEnumerable<FeatureInstance> featureInstances)
    {
      if(!featureInstances.Any())
      {
        throw new ArgumentException("A microservice should contain at least one feature instance");
      }
      _features = featureInstances.ToDictionary(f => f.FeatureId, f => f);
    }

    public MicroserviceIdentifier Id => _features.Values.GetMicroserviceIdentifier();

    public IEnumerator<FeatureInstance> GetEnumerator() => _features.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public override bool Equals(object obj)
    {
      Microservice microservice = obj as Microservice;
      return microservice != null && Id.Equals(microservice.Id) && this.OrderBy(m => m.FeatureId).SequenceEqual(microservice.OrderBy(m => m.FeatureId));
    }

    public override int GetHashCode()
    {
      unchecked
      {
        var hashCode = HashConstants.OffsetBasis;
        foreach(var feature in this.OrderBy(f => f.FeatureId))
        {
          hashCode = (hashCode ^ (feature.GetHashCode())) * HashConstants.Prime;
        }
        hashCode = (hashCode ^ (Id.GetHashCode())) * HashConstants.Prime;
        return hashCode;
      }
    }

    public override string ToString() => string.Join(", ", this.AsEnumerable());
  }
}