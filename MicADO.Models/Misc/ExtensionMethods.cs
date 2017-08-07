using System;
using System.Collections.Generic;
using System.Linq;
using MicADO.Models.Deployment;

namespace MicADO.Models.Misc
{
  public static class ExtensionMethods
  {
    public static MicroserviceIdentifier GetMicroserviceIdentifier(this IEnumerable<FeatureIdentifier> publicFeatureIdentifiers)
    {
      if(!publicFeatureIdentifiers.Any())
      {
        throw new ArgumentOutOfRangeException("Empty Microservices are not supported");
      }
      return new MicroserviceIdentifier(publicFeatureIdentifiers.OrderBy(f => f.Id).First().Id);
    }

    public static MicroserviceIdentifier GetMicroserviceIdentifier(this IEnumerable<FeatureInstance> featureInstances)
    {
      return featureInstances.Where(f => !f.IsInternal).Select(f => f.Feature).Select(f => f.Id).GetMicroserviceIdentifier();
    }
  }
}