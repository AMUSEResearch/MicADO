using MicADO.GeneticAlgorithm.Populations;

namespace MicADO.GeneticAlgorithm.State
{
  public interface IGeneticAlgorithmState
  {
    void UpdateOnGenerationRan(IPopulation newPopulation);
  }
}