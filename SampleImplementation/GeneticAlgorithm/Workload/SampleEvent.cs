using System;

namespace SampleImplementation.GeneticAlgorithm.Workload
{
  public class SampleEvent
  {
    public int ArrivalTime { get; }

    public int ServiceTime { get; }

    public string Type { get; }

    public SampleEvent(int arrivalTime, int serviceTime, string type)
    {
      ArrivalTime = arrivalTime;
      ServiceTime = serviceTime;
      Type = type;
    }

    public override string ToString()
    {
      return $"{ArrivalTime}: {ServiceTime} ({Type})";
    }
  }
}