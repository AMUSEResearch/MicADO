using MicADO.GeneticAlgorithm.Chromosome;

namespace MicADO.GeneticAlgorithm.FitnessEvaluators
{
  /// <summary>
  ///   Evaluates the Fitness of a deployment
  /// </summary>
  public interface IFitnessEvaluator<TWorkload>
  {
    /// <summary>
    /// Returns the fitness score, higher is better
    /// </summary>
    /// <param name="deployment"></param>
    /// <param name="workload"></param>
    /// <returns></returns>
    double Evaluate(IDeploymentChromosome deployment, TWorkload workload);
  }
}