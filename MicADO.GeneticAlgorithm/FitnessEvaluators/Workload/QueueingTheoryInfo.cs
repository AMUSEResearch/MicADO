using System;
using System.Collections.Generic;
using System.Linq;
using MicADO.Models.Misc;

namespace MicADO.GeneticAlgorithm.FitnessEvaluators.Workload
{
  public class QueueingTheoryInfo
  {
    public double ChanceOfOccurance { get; }

    public IEnumerable<FeatureIdentifier> Types { get; }

    public double MeanArrivalRate { get; }

    public double MeanInterArrivalTime => 1d / MeanArrivalRate;

    public double MeanServiceRate { get; }

    public double MeanServiceTime => 1d / MeanServiceRate;

    public double MeanWaitingTime { get; }

    public double Utilization => MeanArrivalRate / MeanServiceRate;

    public double SojournTime => MeanWaitingTime + MeanServiceTime; 

    public QueueingTheoryInfo(double meanInterArrivalTime, double meanServiceTime, double chanceOfOccurance, IEnumerable<string> types)
    {
      ChanceOfOccurance = chanceOfOccurance;
      Types = types.Select(t => new FeatureIdentifier(t));
      MeanArrivalRate = 1d / meanInterArrivalTime;
      MeanServiceRate = 1d / meanServiceTime;
      if(Utilization >= 1)
      {
        throw new ArgumentException("This distribution cannot be modelled by a M/M/1 queue");
      }
      var meanQueueLength = (Utilization * Utilization) / (1 - Utilization);
      var meanQueueWaitingTime = meanQueueLength / MeanArrivalRate;
      MeanWaitingTime = meanQueueWaitingTime + (1d / MeanServiceRate);
    }

    public static QueueingTheoryInfo operator + (QueueingTheoryInfo firstClass, QueueingTheoryInfo secondClass)
    {
      var InterArrivalTime = (firstClass.ChanceOfOccurance * firstClass.MeanInterArrivalTime) + (secondClass.ChanceOfOccurance * secondClass.MeanInterArrivalTime) / (firstClass.ChanceOfOccurance + secondClass.ChanceOfOccurance);
      var meanServiceTime = (firstClass.ChanceOfOccurance * firstClass.MeanServiceTime) + (secondClass.ChanceOfOccurance * secondClass.MeanServiceTime) / (firstClass.ChanceOfOccurance + secondClass.ChanceOfOccurance);
      var chanceofOccurance = firstClass.ChanceOfOccurance + secondClass.ChanceOfOccurance;
      var types = firstClass.Types.Select(t => t.Id).Union(secondClass.Types.Select(t => t.Id));
      return new QueueingTheoryInfo(InterArrivalTime, meanServiceTime, chanceofOccurance, types);
    }

  }
}