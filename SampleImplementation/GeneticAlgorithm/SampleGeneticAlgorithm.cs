using MicADO.GeneticAlgorithm;
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
using SampleImplementation.GeneticAlgorithm.Workload;

namespace SampleImplementation.GeneticAlgorithm
{
  public class SampleGeneticAlgorithm : GeneticAlgorithm<SampleWorkload, GenerationCountState>

  {
    public SampleGeneticAlgorithm() : base(
      mutationProbability: 0.01d,
      crossoverPropability: 0.2d,
      minPopulationSize: 20,
      maxPopulationSize: 40,
      mutationOperator: new ScatterMicroserviceMutation(new DefaultRandomProvider()),
      crossoverOperator: new MergeMicroserviceCrossover(new DefaultRandomProvider()),
      fitnessEvaluator: new SampleFitnessEvaluator(),
      initialPopulationCreator: new CloneInitialPopulationCreator(),
      terminationCondition: new GenerationNumberTermination(1000),
      selectionStrategy: new EliteSelection(),
      reinsertionStrategy: new EliteReinsertion(),
      chromosomeFactory: new DeploymentChromosomeFactory(),
      currentState: new GenerationCountState(),
      workload: new SampleWorkload(),
      randomProvider: new DefaultRandomProvider()
    )
    {
    }
  }
}