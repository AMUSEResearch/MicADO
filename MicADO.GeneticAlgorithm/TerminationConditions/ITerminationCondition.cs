using MicADO.GeneticAlgorithm.Populations;
using MicADO.GeneticAlgorithm.State;
using MicADO.Models.Deployment;

namespace MicADO.GeneticAlgorithm.TerminationConditions
{
  public interface ITerminationCondition<TState> where TState : IGeneticAlgorithmState
  {
    bool HasReached(IPopulation population, TState state);
  }
}