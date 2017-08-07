using System.Collections.Generic;
using System.Linq;
using MicADO.Models.Features;
using MicADO.Models.Misc;

namespace MicADO.Models.Deployment
{
  /// <summary>
  ///   Model of a Microservice Architecture
  /// </summary>
  public class DeploymentModel
  {
    public FeatureModel FeatureModel { get; }

    private Dictionary<MicroserviceIdentifier, Microservice> _microservices;

    /// <summary>
    ///   Initializes a new instance of the <see cref="DeploymentModel" /> class.
    /// </summary>
    public DeploymentModel(FeatureModel featureModel, IEnumerable<Microservice> microservices)
    {
      FeatureModel = featureModel;
      _microservices = microservices.ToDictionary(m => m.Id, m => m);
    }

    /// <summary>
    ///   Gets the microservices.
    /// </summary>
    /// <value>
    ///   The microservices.
    /// </value>
    public IEnumerable<Microservice> Microservices => _microservices.Values.AsEnumerable();

    public override bool Equals(object obj)
    {
      DeploymentModel deploymentModel = obj as DeploymentModel;
      return deploymentModel != null 
        && Microservices.OrderBy(m => m.Id).SequenceEqual(deploymentModel.Microservices.OrderBy(m => m.Id)) 
        && FeatureModel.Equals(deploymentModel.FeatureModel);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        var hashCode = HashConstants.OffsetBasis;
        hashCode = (hashCode ^ (FeatureModel.GetHashCode())) * HashConstants.Prime;
        foreach(var microservice in Microservices.OrderBy(m => m.Id))
        {
          hashCode = (hashCode ^ (microservice.GetHashCode())) * HashConstants.Prime;
        }
        return hashCode;
      }
    }
  }
}