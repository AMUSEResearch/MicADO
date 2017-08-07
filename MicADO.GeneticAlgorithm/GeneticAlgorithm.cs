using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using MicADO.GeneticAlgorithm.Chromosome;
using MicADO.GeneticAlgorithm.Chromosome.Factory;
using MicADO.GeneticAlgorithm.Crossovers;
using MicADO.GeneticAlgorithm.FitnessEvaluators;
using MicADO.GeneticAlgorithm.Misc;
using MicADO.GeneticAlgorithm.Mutations;
using MicADO.GeneticAlgorithm.Populations.AdamPopulationCreater;
using MicADO.GeneticAlgorithm.Reinsertion;
using MicADO.GeneticAlgorithm.Selections;
using MicADO.GeneticAlgorithm.State;
using MicADO.GeneticAlgorithm.TerminationConditions;
using MicADO.Models.Deployment;

namespace MicADO.GeneticAlgorithm
{
  public abstract class GeneticAlgorithm<TWorkload, TState> : IGeneticAlgorithm<TWorkload, TState> 
    where TState : IGeneticAlgorithmState
  {
    public GeneticAlgorithm(
      double mutationProbability,
      double crossoverPropability,
      int minPopulationSize,
      int maxPopulationSize,
      IMutation mutationOperator,
      ICrossover crossoverOperator,
      IFitnessEvaluator<TWorkload> fitnessEvaluator,
      IInitialPopulationCreator initialPopulationCreator,
      ITerminationCondition<TState> terminationCondition,
      ISelectionStrategy selectionStrategy,
      IReinsertionStrategy reinsertionStrategy,
      IDeploymentChromosomeFactory chromosomeFactory,
      TState currentState,
      TWorkload workload,
      IRandomProvider randomProvider)
    {
      MutationProbability = mutationProbability;
      CrossoverProbability = crossoverPropability;
      MinPopulationSize = minPopulationSize;
      MaxPopulationSize = maxPopulationSize;
      MutationOperator = mutationOperator;
      CrossoverOperator = crossoverOperator;
      FitnessEvaluator = fitnessEvaluator;
      InitialPopulationCreator = initialPopulationCreator;
      TerminationCondition = terminationCondition;
      SelectionStrategyStrategy = selectionStrategy;
      ReinsertionStrategy = reinsertionStrategy;
      ChromosomeFactory = chromosomeFactory;
      CurrentState = currentState;
      Workload = workload;
      RandomProvider = randomProvider;
    }

    public double MutationProbability { get;}

    public double CrossoverProbability { get; }

    public int MinPopulationSize { get; }

    public int MaxPopulationSize { get; }

    public IMutation MutationOperator { get; }

    public ICrossover CrossoverOperator { get; }
    
    public IFitnessEvaluator<TWorkload> FitnessEvaluator { get; }

    public IInitialPopulationCreator InitialPopulationCreator { get; }

    public ITerminationCondition<TState> TerminationCondition { get; }

    public ISelectionStrategy SelectionStrategyStrategy { get; }

    public IReinsertionStrategy ReinsertionStrategy { get; }

    public IDeploymentChromosomeFactory ChromosomeFactory { get; }

    public TState CurrentState { get; }

    public IRandomProvider RandomProvider { get; }

    public TWorkload Workload { get; }

    public DeploymentModel Run(DeploymentModel initialDeploymentModel)
    {
      var initialChromosome = ChromosomeFactory.Create(initialDeploymentModel);
      var population = InitialPopulationCreator.CreateInitialPopulation(initialChromosome, MinPopulationSize, MaxPopulationSize);
      while(!TerminationCondition.HasReached(population, CurrentState))
      {
        var parents = SelectionStrategyStrategy.SelectChromosomes(MinPopulationSize, population).ToArray();
        var offspring = Cross(parents).ToArray();
        offspring = Mutate(offspring).ToArray();
        population = ReinsertionStrategy.Reinsert(offspring, population);
        foreach(var deployment in population.Deployments)
        {
          deployment.Fitness = FitnessEvaluator.Evaluate(deployment, Workload);
        }
        CurrentState.UpdateOnGenerationRan(population);
      }
      return population.BestDeployment.ToDeploymentModel();
    }

    protected virtual IEnumerable<IDeploymentChromosome> Cross(IEnumerable<IDeploymentChromosome> parents)
    {
      var crossoverParents = new IDeploymentChromosome[CrossoverOperator.ParentsNumber];
      var i = 0;
      List<IDeploymentChromosome> offspring = new List<IDeploymentChromosome>();
      foreach(var parent in parents)
      {
        crossoverParents[i] = parent;
        i++;
        if(i == CrossoverOperator.ParentsNumber)
        {
          if(RandomProvider.GetRandom() <= CrossoverProbability)
          {
            offspring.AddRange(CrossoverOperator.Cross(crossoverParents));
          }
          else
          {
            offspring.AddRange(crossoverParents);
          }
          i = 0;
        }
      }
      if(i != 0)
      {
        offspring.AddRange(crossoverParents.Take(i));
      }
      return offspring;
    }

    protected virtual IEnumerable<IDeploymentChromosome> Mutate(IEnumerable<IDeploymentChromosome> offspring)
    {
      foreach(var child in offspring)
      {
        if(RandomProvider.GetRandom() <= MutationProbability)
        {
          yield return MutationOperator.Mutate(child);
        }
        else
        {
          yield return child;
        }
      }
    }
  }
}