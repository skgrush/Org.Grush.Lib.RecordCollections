using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace Org.Grush.Lib.RecordCollections.Benchmark.Tests;

public class ForEachIteration
{
  [Params(10, 1000, 10_000)]
  public int Length { get; set; }

  private List<int> _initialList;
  private RecordCollection<int> _initialRecordCollection;
  private int[] _initialArray;

  [GlobalSetup]
  public void GlobalSetup()
  {
    var random = new Random();
    _initialList = Enumerable.Range(0, Length).Select(_ => random.Next(0, Length)).ToList();
    _initialRecordCollection = _initialList.ToRecordCollection();
    _initialArray = _initialList.ToArray();
  }

  [Benchmark]
  public void ForEach_List()
  {
    foreach (var x in _initialList)
    {
      Noop(x);
    }
  }

  [Benchmark]
  public void ForEach_RecordCollection()
  {
    foreach (var x in _initialRecordCollection)
    {
      Noop(x);
    }
  }

  [Benchmark]
  public void ForEach_Array()
  {
    foreach (var x in _initialArray)
    {
      Noop(x);
    }
  }

  [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
  private static void Noop(int arg)
  {
  }
}
