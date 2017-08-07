using System;
using System.Collections.Generic;
using MicADO.GeneticAlgorithm.Chromosome;
using MicADO.GeneticAlgorithm.Chromosome.Factory;
using MicADO.GeneticAlgorithm.Crossovers;
using MicADO.GeneticAlgorithm.FitnessEvaluators;
using MicADO.GeneticAlgorithm.Misc;
using MicADO.GeneticAlgorithm.Mutations;
using MicADO.GeneticAlgorithm.Populations.AdamPopulationCreater;
using MicADO.GeneticAlgorithm.Reinsertion;
using MicADO.GeneticAlgorithm.Selections;
using MicADO.GeneticAlgorithm.TerminationConditions;

namespace MicADO.GeneticAlgorithm.Tests.Misc
{
  public class TestGeneticAlgorithm : GeneticAlgorithm<TestWorkload, TestState>
  {
    public TestGeneticAlgorithm(
      double mutationProbability,
      double crossoverPropability,
      int minPopulationSize,
      int maxPopulationSize,
      IMutation mutationOperator,
      ICrossover crossoverOperator,
      IFitnessEvaluator<TestWorkload> fitnessEvaluator,
      IInitialPopulationCreator initialPopulationCreator,
      ITerminationCondition<TestState> terminationCondition,
      ISelectionStrategy selectionStrategy,
      IReinsertionStrategy reinsertionStrategy,
      IDeploymentChromosomeFactory chromosomeFactory,
      TestState currentState,
      TestWorkload workload,
      IRandomProvider randomProvider)
      : base(
        mutationProbability,
        crossoverPropability,
        minPopulationSize,
        maxPopulationSize,
        mutationOperator,
        crossoverOperator,
        fitnessEvaluator,
        initialPopulationCreator,
        terminationCondition,
        selectionStrategy,
        reinsertionStrategy,
        chromosomeFactory,
        currentState,
        workload,
        randomProvider)
    {
      
    }

    public IEnumerable<IDeploymentChromosome> Crossover(IEnumerable<IDeploymentChromosome> parents)
    {
      return Cross(parents);
    }

    public IEnumerable<IDeploymentChromosome> Mutation(IEnumerable<IDeploymentChromosome> offspring)
    {
      return Mutate(offspring);
    }
  }
}