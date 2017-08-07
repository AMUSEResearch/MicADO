using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MicADO.GeneticAlgorithm.FitnessEvaluators.Workload;
using MicADO.Models.Deployment;
using MicADO.Models.Misc;
using Newtonsoft.Json;

namespace SampleImplementation.GeneticAlgorithm.Workload
{
  public class SampleWorkload
  {
    private Dictionary<FeatureIdentifier, QueueingTheoryInfo> _queueingTheoryInfos;

    public SampleWorkload()
    {
      // This sample file contains a poisson distributed workload
      var json = File.ReadAllText("workload.json");
      var workload = (SampleEvent[])JsonConvert.DeserializeObject(json, typeof(SampleEvent[]));
      // Determine individual customer classes
      var workloadPerTypes = workload.GroupBy(e => e.Type, e => e).ToDictionary(g => g.Key, g => g.ToArray());
      _queueingTheoryInfos = new Dictionary<FeatureIdentifier, QueueingTheoryInfo>();
      // Calculate the queueing theory representation for every customer class
      foreach(var workloadTypePair in workloadPerTypes)
      {
        var events = workloadTypePair.Value;
        var timeBetweenArrivals = new List<int>();
        var serviceTimes = new List<int>();
        for(var i = 1; i < events.Length; i++)
        {
          timeBetweenArrivals.Add(events[i].ArrivalTime - events[i-1].ArrivalTime);
          serviceTimes.Add(events[i].ServiceTime);
        }

        var meanInterArrivalTime = (int)Math.Round(timeBetweenArrivals.Average());
        var meanServiceTime = (int)Math.Round(serviceTimes.Average());

        var featureId = new FeatureIdentifier(workloadTypePair.Key);
        var chanceOfOccurance = (double)events.Length / workload.Length;
        _queueingTheoryInfos[featureId] = new QueueingTheoryInfo(meanInterArrivalTime, meanServiceTime, chanceOfOccurance, new []{ workloadTypePair.Key });
      }
    }

    public QueueingTheoryInfo GetQueueingTheoryInfo(FeatureInstance featureInstance)
    {
      var publicFeatureInfo = _queueingTheoryInfos[featureInstance.FeatureId];
      var fractionOfPropertiesIncluded = (double)featureInstance.Properties.Count() / featureInstance.Feature.Properties.Count();
      var serviceTime = publicFeatureInfo.MeanServiceTime * fractionOfPropertiesIncluded;
      return new QueueingTheoryInfo(publicFeatureInfo.MeanInterArrivalTime, serviceTime, publicFeatureInfo.ChanceOfOccurance, publicFeatureInfo.Types.Select(f => f.Id));
    }
  }
}