using System.Linq;
using FakeItEasy;
using MicADO.GeneticAlgorithm.Chromosome;
using MicADO.GeneticAlgorithm.Chromosome.Factory;
using MicADO.GeneticAlgorithm.Chromosome.Gene;
using MicADO.GeneticAlgorithm.Crossovers;
using MicADO.GeneticAlgorithm.FitnessEvaluators;
using MicADO.GeneticAlgorithm.Misc;
using MicADO.GeneticAlgorithm.Mutations;
using MicADO.GeneticAlgorithm.Populations.AdamPopulationCreater;
using MicADO.GeneticAlgorithm.Reinsertion;
using MicADO.GeneticAlgorithm.Selections;
using MicADO.GeneticAlgorithm.TerminationConditions;
using MicADO.GeneticAlgorithm.Tests.Misc;
using MicADO.Models.Deployment;
using MicADO.Models.Features;
using MicADO.Models.Misc;
using NUnit.Framework;

namespace MicADO.GeneticAlgorithm.Tests
{
  [TestFixture]
  public class GeneticAlgorithmTests
  {
    private IMutation _mutationOperator;
    private ICrossover _crossoverOperator;
    private IFitnessEvaluator<TestWorkload> _fitnessEvaluator;
    private IInitialPopulationCreator _initialPopulationCreator;
    private ITerminationCondition<TestState> _terminationCondition;
    private ISelectionStrategy _selectionStrategy;
    private IReinsertionStrategy _reinsertionStrategy;
    private IDeploymentChromosomeFactory _deploymentChromosomeFactory;
    private TestState _currentState;
    private TestGeneticAlgorithm _testGeneticAlgorithm;
    private TestWorkload _workload;
    private IRandomProvider _randomProvider;

    private double _mutationProbability;
    private double _crossoverProbability;
    private int _minPopulationSize;
    private int _maxPopulationSize;

    [SetUp]
    public void SetUp()
    {
      _mutationProbability = 1;
      _crossoverProbability = 1;
      _minPopulationSize = 2;
      _maxPopulationSize = 4;
      _mutationOperator = A.Fake<IMutation>();
      _crossoverOperator = A.Fake<ICrossover>();
      _fitnessEvaluator = A.Fake<IFitnessEvaluator<TestWorkload>>();
      _initialPopulationCreator = A.Fake<IInitialPopulationCreator>();
      _terminationCondition = A.Fake<ITerminationCondition<TestState>>();
      _selectionStrategy = A.Fake<ISelectionStrategy>();
      _reinsertionStrategy = A.Fake<IReinsertionStrategy>();
      _deploymentChromosomeFactory = A.Fake<IDeploymentChromosomeFactory>();
      _currentState = new TestState();
      _workload = new TestWorkload();
      _randomProvider = new DefaultRandomProvider();
      
    }

    
    private void ConstructGeneticAlgorithm()
    {
      _testGeneticAlgorithm = new TestGeneticAlgorithm(
        _mutationProbability,
        _crossoverProbability,
        _minPopulationSize,
        _maxPopulationSize,
        _mutationOperator,
        _crossoverOperator,
        _fitnessEvaluator,
        _initialPopulationCreator,
        _terminationCondition,
        _selectionStrategy,
        _reinsertionStrategy,
        _deploymentChromosomeFactory,
        _currentState,
        _workload,
        _randomProvider
      );
    }

    [Test]
    public void Constructor_Sets_Fields_Correctly()
    {
      ConstructGeneticAlgorithm();
      Assert.AreEqual(1, _testGeneticAlgorithm.MutationProbability);
      Assert.AreEqual(1, _testGeneticAlgorithm.CrossoverProbability);
      Assert.AreEqual(2, _testGeneticAlgorithm.MinPopulationSize);
      Assert.AreEqual(4, _testGeneticAlgorithm.MaxPopulationSize);
      Assert.AreEqual(_mutationOperator, _testGeneticAlgorithm.MutationOperator);
      Assert.AreEqual(_crossoverOperator, _testGeneticAlgorithm.CrossoverOperator);
      Assert.AreEqual(_fitnessEvaluator, _testGeneticAlgorithm.FitnessEvaluator);
      Assert.AreEqual(_initialPopulationCreator, _testGeneticAlgorithm.InitialPopulationCreator);
      Assert.AreEqual(_terminationCondition, _testGeneticAlgorithm.TerminationCondition);
      Assert.AreEqual(_selectionStrategy, _testGeneticAlgorithm.SelectionStrategyStrategy);
      Assert.AreEqual(_reinsertionStrategy, _testGeneticAlgorithm.ReinsertionStrategy);
      Assert.AreEqual(_deploymentChromosomeFactory, _testGeneticAlgorithm.ChromosomeFactory);
      Assert.AreEqual(_currentState, _testGeneticAlgorithm.CurrentState);
      Assert.AreEqual(_workload, _testGeneticAlgorithm.Workload);
      Assert.AreEqual(_randomProvider,_testGeneticAlgorithm.RandomProvider);
    }

    [Test]
    public void Run_WithChromsomeMatchingTerminationCriteria_CallsRightMethods()
    {
      var deploymentModel = CreateDeploymentModel();
      A.CallTo(() => _terminationCondition.HasReached(null, null)).WithAnyArguments().Returns(true);
      ConstructGeneticAlgorithm();

      var bestDeploymentModel =  _testGeneticAlgorithm.Run(deploymentModel);
      A.CallTo(() => _deploymentChromosomeFactory.Create(deploymentModel)).MustHaveHappened(Repeated.Exactly.Once);
      A.CallTo(() => _initialPopulationCreator.CreateInitialPopulation(null, 0, 0)).WithAnyArguments().MustHaveHappened(Repeated.Exactly.Once);

      A.CallTo(() => _mutationOperator.Mutate(null)).WithAnyArguments().MustNotHaveHappened();
      A.CallTo(() => _crossoverOperator.Cross(null)).WithAnyArguments().MustNotHaveHappened();
      A.CallTo(() => _selectionStrategy.SelectChromosomes(0, null)).WithAnyArguments().MustNotHaveHappened();
      A.CallTo(() => _reinsertionStrategy.Reinsert(null, null)).WithAnyArguments().MustNotHaveHappened();
      
      Assert.AreEqual(0, _currentState.Count);
    }

    private DeploymentModel CreateDeploymentModel()
    {
      var aFeature = new Feature(new FeatureIdentifier("a"), "a", new[]
        {
          new Property(new PropertyIdentifier("p1"), "p1"),
          new Property(new PropertyIdentifier("p2"), "p2")
        }
      );

      var bFeature = new Feature(new FeatureIdentifier("b"), "b", new[]
        {
          new Property(new PropertyIdentifier("p3"), "p3"),
          new Property(new PropertyIdentifier("p4"), "p4")
        }
      );

      var cFeature = new Feature(new FeatureIdentifier("c"), "c", new[]
        {
          new Property(new PropertyIdentifier("p5"), "p5"),
          new Property(new PropertyIdentifier("p6"), "p6"),
          new Property(new PropertyIdentifier("p7"), "p7")
        }
      );

      var featureModel = new FeatureModel(new[] { aFeature, bFeature, cFeature }, new[]
      {
        new PropertyRelation(new PropertyIdentifier("p6"), new PropertyIdentifier("p2")),
        new PropertyRelation(new PropertyIdentifier("p7"), new PropertyIdentifier("p4"))
      });

      return new DeploymentModel(featureModel, new[]
      {
        new Microservice(new[] { new FeatureInstance(aFeature, new[] { new PropertyIdentifier("p1"), new PropertyIdentifier("p2") }) }),
        new Microservice(new[] { new FeatureInstance(bFeature, new[] { new PropertyIdentifier("p3"), new PropertyIdentifier("p4") }) }),
        new Microservice(new[]
        {
          new FeatureInstance(cFeature, new[] { new PropertyIdentifier("p5"), new PropertyIdentifier("p6"), new PropertyIdentifier("p7") }),
          new FeatureInstance(bFeature, new[] { new PropertyIdentifier("p4") }, true),
          new FeatureInstance(aFeature, new[] { new PropertyIdentifier("p2") }, true)
        }),
      });
    }

    [Test]
    public void Run_Once_CallsRightMethods()
    {
      var deploymentModel = CreateDeploymentModel();
      A.CallTo(() => _terminationCondition.HasReached(null, null)).WithAnyArguments().ReturnsNextFromSequence(false, true);
      _deploymentChromosomeFactory = new DeploymentChromosomeFactory();
      _initialPopulationCreator = new CloneInitialPopulationCreator();
      _selectionStrategy = new EliteSelection();
      _crossoverOperator = new IdCrossover();
      _mutationOperator = new IdMutation();
      _reinsertionStrategy = new EliteReinsertion();
      ConstructGeneticAlgorithm();

      var bestDeploymentModel = _testGeneticAlgorithm.Run(deploymentModel);
      Assert.AreEqual(1, _currentState.Count);
    }

    [Test]
    public void Cross_WithUnevenPopulationSize_Returns_CorrectOffspring()
    {
      _crossoverOperator = new IdCrossover();
      _randomProvider = A.Fake<IRandomProvider>();
      ConstructGeneticAlgorithm();

      var deploymentModel = CreateDeploymentModel();

      var genes = new[]
      {
        new DeploymentGene(new FeatureIdentifier("a"), new MicroserviceIdentifier("a"))
      };

      var chromosomes = new[]
      {
        new DeploymentChromosome(deploymentModel.FeatureModel, genes),
      };

      var result = _testGeneticAlgorithm.Crossover(chromosomes);
      CollectionAssert.AreEqual(chromosomes, result);
    }

    [Test]
    public void Cross_WithNoCrossover_Returns_CorrectOffspring()
    {
      _crossoverOperator = new IdCrossover();
      _randomProvider = A.Fake<IRandomProvider>();
      _crossoverProbability = -1;
      ConstructGeneticAlgorithm();

      var deploymentModel = CreateDeploymentModel();

      var genes = new[]
      {
        new DeploymentGene(new FeatureIdentifier("a"), new MicroserviceIdentifier("a"))
      };

      var chromosomes = new[]
      {
        new DeploymentChromosome(deploymentModel.FeatureModel, genes),
        new DeploymentChromosome(deploymentModel.FeatureModel, genes),
      };

      var result = _testGeneticAlgorithm.Crossover(chromosomes);
      CollectionAssert.AreEqual(chromosomes, result);
    }

    [Test]
    public void Cross_WithNoMutation_Returns_CorrectOffspring()
    {
      _mutationOperator = A.Fake<IMutation>();
      _randomProvider = A.Fake<IRandomProvider>();
      A.CallTo(() => _randomProvider.GetRandom()).WithAnyArguments().Returns(1);
      _mutationProbability = 0;
      ConstructGeneticAlgorithm();

      var deploymentModel = CreateDeploymentModel();

      var genes = new[]
      {
        new DeploymentGene(new FeatureIdentifier("a"), new MicroserviceIdentifier("a"))
      };

      var chromosomes = new[]
      {
        new DeploymentChromosome(deploymentModel.FeatureModel, genes),
        new DeploymentChromosome(deploymentModel.FeatureModel, genes),
      };

      var result = _testGeneticAlgorithm.Mutation(chromosomes);
      A.CallTo(() => _mutationOperator.Mutate(null)).WithAnyArguments().MustNotHaveHappened();
      CollectionAssert.AreEqual(chromosomes, result);
    }
  }
}