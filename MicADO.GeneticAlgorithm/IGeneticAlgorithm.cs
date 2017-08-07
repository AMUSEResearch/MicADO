using MicADO.GeneticAlgorithm.Crossovers;
using MicADO.GeneticAlgorithm.FitnessEvaluators;
using MicADO.GeneticAlgorithm.Mutations;
using MicADO.GeneticAlgorithm.Populations.AdamPopulationCreater;
using MicADO.GeneticAlgorithm.State;
using MicADO.Models.Deployment;

namespace MicADO.GeneticAlgorithm
{
  public interface IGeneticAlgorithm<TWorkload, TState> 
    where TState : IGeneticAlgorithmState
  {
    IMutation MutationOperator { get; }

    ICrossover CrossoverOperator { get; }

    IFitnessEvaluator<TWorkload> FitnessEvaluator { get; }

    IInitialPopulationCreator InitialPopulationCreator { get; }

    TState CurrentState { get; }

    TWorkload Workload { get; }

    DeploymentModel Run(DeploymentModel initialDeployment);
  }
}