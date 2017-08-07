using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MicADO.GeneticAlgorithm.Chromosome;
using MicADO.GeneticAlgorithm.Chromosome.Gene;
using MicADO.Models.Deployment;
using MicADO.Models.Features;
using MicADO.Models.Misc;
using NUnit.Framework;

namespace MicADO.GeneticAlgorithm.Tests.Chromosomes
{
  [TestFixture]
  public class DeploymentChromosomeTests
  {
    private class EqualsTestCases : IEnumerable<TestCaseData>
    {
      public IEnumerator<TestCaseData> GetEnumerator()
      {
        var featureModel = new FeatureModel(new[]
        {
          new Feature(new FeatureIdentifier("a"), "a", new[]
          {
            new Property(new PropertyIdentifier("p1"), "p1")
          })
        }, Enumerable.Empty<PropertyRelation>());
        var gene1 = new DeploymentGene(new FeatureIdentifier("a"), new MicroserviceIdentifier("b"));
        var gene2 = new DeploymentGene(new FeatureIdentifier("b"), new MicroserviceIdentifier("b"));

        var chromosome = new DeploymentChromosome(featureModel, new[] { gene1, gene2 });

        var same = new DeploymentChromosome(featureModel, new[] { gene1, gene2 });

        yield return new TestCaseData(chromosome, chromosome).Returns(true);
        yield return new TestCaseData(chromosome, same).Returns(true);
        yield return new TestCaseData(chromosome, null).Returns(false);
        yield return new TestCaseData(chromosome, 3).Returns(false);
      }

      IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    [Test]
    public void Constructor_Sets_Fields_Correctly()
    {
      var featureModel = A.Fake<FeatureModel>();
      var genes = new[]
      {
        new DeploymentGene(new FeatureIdentifier("a"), new MicroserviceIdentifier("a")),
        new DeploymentGene(new FeatureIdentifier("b"), new MicroserviceIdentifier("b"))
      };
      var sot = new DeploymentChromosome(featureModel, genes);

      Assert.AreEqual(featureModel, sot.FeatureModel);
      Assert.AreEqual(genes, sot.Genes);
      Assert.AreEqual(null, sot.Fitness);
    }

    [Test]
    public void Fitness_Is_Settable()
    {
      var featureModel = A.Fake<FeatureModel>();
      var genes = new[]
      {
        new DeploymentGene(new FeatureIdentifier("a"), new MicroserviceIdentifier("a")),
        new DeploymentGene(new FeatureIdentifier("b"), new MicroserviceIdentifier("b"))
      };
      var sot = new DeploymentChromosome(featureModel, genes)
      {
        Fitness = 1
      };
      Assert.AreEqual(1, sot.Fitness);
    }

    [Test]
    public void GetGene_Returns_CorrectGene()
    {
      var featureModel = A.Fake<FeatureModel>();
      var genes = new[]
      {
        new DeploymentGene(new FeatureIdentifier("a"), new MicroserviceIdentifier("a")),
        new DeploymentGene(new FeatureIdentifier("b"), new MicroserviceIdentifier("b"))
      };
      var sot = new DeploymentChromosome(featureModel, genes);
      Assert.AreEqual(genes[0], sot.GetGene(new FeatureIdentifier("a")));
    }

    [Test]
    public void ToString_Returns_NonEmptyString()
    {
      var featureModel = A.Fake<FeatureModel>();
      var genes = new[]
      {
        new DeploymentGene(new FeatureIdentifier("a"), new MicroserviceIdentifier("a")),
        new DeploymentGene(new FeatureIdentifier("b"), new MicroserviceIdentifier("b"))
      };
      var sot = new DeploymentChromosome(featureModel, genes);
      Assert.False(string.IsNullOrEmpty(sot.ToString()));
    }

    [Test]
    [TestCaseSource(typeof(EqualsTestCases))]
    public bool Equals_Returns_Correctly(DeploymentChromosome chromosome, object other)
    {
      return chromosome.Equals(other);
    }

    [Test]
    public void UpdateGene_ToSameMicroservice_Returns_SameDeployment()
    {
      var featureModel = A.Fake<FeatureModel>();
      var genes = new[]
      {
        new DeploymentGene(new FeatureIdentifier("a"), new MicroserviceIdentifier("a")),
        new DeploymentGene(new FeatureIdentifier("b"), new MicroserviceIdentifier("b"))
      };
      var sot = new DeploymentChromosome(featureModel, genes);
      sot = (DeploymentChromosome)sot.UpdateGene(genes[0]);
      Assert.AreEqual(genes[0], sot.GetGene(new FeatureIdentifier("a")));
    }

    [Test]
    public void GetHashCode_Returns_Same_ForIdenticalDeployment()
    {
      var featureModel = A.Fake<FeatureModel>();
      var genes = new[]
      {
        new DeploymentGene(new FeatureIdentifier("a"), new MicroserviceIdentifier("a")),
        new DeploymentGene(new FeatureIdentifier("b"), new MicroserviceIdentifier("b"))
      };
      var sot = new DeploymentChromosome(featureModel, genes);

      var sameFeatureModel = A.Fake<FeatureModel>();
      var samegenes = new[]
      {
        new DeploymentGene(new FeatureIdentifier("a"), new MicroserviceIdentifier("a")),
        new DeploymentGene(new FeatureIdentifier("b"), new MicroserviceIdentifier("b"))
      };
      var same = new DeploymentChromosome(featureModel, genes);

      Assert.AreEqual(sot.GetHashCode(), same.GetHashCode());
    }

    [Test]
    public void UpdateGene_GeneToOtherMicroservice_Returns_Correct_Genes()
    {
      var featureModel = A.Fake<FeatureModel>();
      var genes = new[]
      {
        new DeploymentGene(new FeatureIdentifier("a"), new MicroserviceIdentifier("a")),
        new DeploymentGene(new FeatureIdentifier("b"), new MicroserviceIdentifier("b"))
      };
      var sot = new DeploymentChromosome(featureModel, genes);
      var gene = new DeploymentGene(new FeatureIdentifier("a"), new MicroserviceIdentifier("b"));
      sot = (DeploymentChromosome)sot.UpdateGene(gene);
      Assert.AreEqual(genes[0], sot.GetGene(new FeatureIdentifier("a")));
      Assert.AreEqual(new DeploymentGene(new FeatureIdentifier("b"), new MicroserviceIdentifier("a")), sot.GetGene(new FeatureIdentifier("b")));
    }

    [Test]
    public void ToDeploymentModel_WithDependencies_Returns_Correctly()
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

      var genes = new[]
      {
        new DeploymentGene(new FeatureIdentifier("a"), new MicroserviceIdentifier("a")),
        new DeploymentGene(new FeatureIdentifier("b"), new MicroserviceIdentifier("b")),
        new DeploymentGene(new FeatureIdentifier("c"), new MicroserviceIdentifier("c"))
      };

      var sot = new DeploymentChromosome(featureModel, genes);

      var deploymentModel = new DeploymentModel(featureModel, new[]
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

      Assert.AreEqual(deploymentModel, sot.ToDeploymentModel());
    }

    [Test]
    public void ToDeploymentModel_WithoutDependencies_Returns_Correctly()
    {
      var aFeature = new Feature(new FeatureIdentifier("a"), "a", new[]
        {
          new Property(new PropertyIdentifier("p1"), "p1")
        }
      );

      var bFeature = new Feature(new FeatureIdentifier("b"), "b", new[]
        {
          new Property(new PropertyIdentifier("p2"), "p2")
        }
      );

      var featureModel = new FeatureModel(new[] { aFeature, bFeature }, Enumerable.Empty<PropertyRelation>());

      var genes = new[]
      {
        new DeploymentGene(new FeatureIdentifier("a"), new MicroserviceIdentifier("a")),
        new DeploymentGene(new FeatureIdentifier("b"), new MicroserviceIdentifier("b"))
      };
      var sot = new DeploymentChromosome(featureModel, genes);

      var deploymentModel = new DeploymentModel(featureModel, new[]
      {
        new Microservice(new[] { new FeatureInstance(aFeature, new[] { new PropertyIdentifier("p1") }) }),
        new Microservice(new[] { new FeatureInstance(bFeature, new[] { new PropertyIdentifier("p2") }) }),
      });

      Assert.AreEqual(deploymentModel, sot.ToDeploymentModel());
    }

    [Test]
    public void ToDeploymentModel_WithDependenciesInSingleMicroservice_Returns_Correctly()
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

      var genes = new[]
      {
        new DeploymentGene(new FeatureIdentifier("a"), new MicroserviceIdentifier("a")),
        new DeploymentGene(new FeatureIdentifier("b"), new MicroserviceIdentifier("a")),
        new DeploymentGene(new FeatureIdentifier("c"), new MicroserviceIdentifier("a"))
      };

      var sot = new DeploymentChromosome(featureModel, genes);

      var deploymentModel = new DeploymentModel(featureModel, new[]
      {
        new Microservice(new[]
        {
          new FeatureInstance(aFeature, new[] { new PropertyIdentifier("p1"), new PropertyIdentifier("p2") }),
          new FeatureInstance(bFeature, new[] { new PropertyIdentifier("p3"), new PropertyIdentifier("p4") }),
          new FeatureInstance(cFeature, new[] { new PropertyIdentifier("p5"), new PropertyIdentifier("p6"), new PropertyIdentifier("p7") }),
        })
      });

      Assert.AreEqual(deploymentModel, sot.ToDeploymentModel());
    }

    [Test]
    public void ToDeploymentModel_WithRecursiveDependenciesInSingleMicroservice_Returns_Correctly()
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
        new PropertyRelation(new PropertyIdentifier("p6"), new PropertyIdentifier("p4")),
        new PropertyRelation(new PropertyIdentifier("p4"), new PropertyIdentifier("p2"))
      });

      var genes = new[]
      {
        new DeploymentGene(new FeatureIdentifier("a"), new MicroserviceIdentifier("a")),
        new DeploymentGene(new FeatureIdentifier("b"), new MicroserviceIdentifier("b")),
        new DeploymentGene(new FeatureIdentifier("c"), new MicroserviceIdentifier("c"))
      };

      var sot = new DeploymentChromosome(featureModel, genes);

      var deploymentModel = new DeploymentModel(featureModel, new[]
      {
        new Microservice(new[]
        {
          new FeatureInstance(aFeature, new[] { new PropertyIdentifier("p1"), new PropertyIdentifier("p2") }),
        }),
        new Microservice(new []
        {
          new FeatureInstance(aFeature, new[] { new PropertyIdentifier("p2") }, true),
          new FeatureInstance(bFeature, new[] { new PropertyIdentifier("p3"), new PropertyIdentifier("p4") })
        }),
        new Microservice(new []
        {
          new FeatureInstance(cFeature, new[] { new PropertyIdentifier("p5"), new PropertyIdentifier("p6"), new PropertyIdentifier("p7") }),
          new FeatureInstance(bFeature, new[] { new PropertyIdentifier("p4") }, true),
          new FeatureInstance(aFeature, new[] { new PropertyIdentifier("p2") }, true)
        }), 
      });

      Assert.AreEqual(deploymentModel, sot.ToDeploymentModel());
    }
  }
}