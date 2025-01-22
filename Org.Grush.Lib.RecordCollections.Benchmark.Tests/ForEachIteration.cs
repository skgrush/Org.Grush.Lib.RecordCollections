using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Org.Grush.Lib.RecordCollections.Benchmark.Tests;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80, id: "ForEach")]
[SimpleJob(RuntimeMoniker.NativeAot80, id: "ForEach")]
public class ForEachIteration
{
  public const string Title = "`foreach` loop enumeration";
  public const string Subtitle = "Iterate over a pre-built collection of `N` integers using `foreach`.";

  [Params(10, 1000, 10_000)]
  public int N { get; set; }

  private List<int> _initialList = null!;
  private RecordCollection<int> _initialRecordCollection;
  private int[] _initialArray = null!;

  [GlobalSetup]
  public void GlobalSetup()
  {
    var random = new Random();
    _initialList = Enumerable.Range(0, N).Select(_ => random.Next(0, N)).ToList();
    _initialRecordCollection = _initialList.ToRecordCollection();
    _initialArray = _initialList.ToArray();
  }

  [Benchmark]
  public void ListOfIntegers()
  {
    foreach (var x in _initialList)
    {
      Noop(x);
    }
  }

  [Benchmark]
  public void RecordCollectionOfIntegers()
  {
    foreach (var x in _initialRecordCollection)
    {
      Noop(x);
    }
  }

  [Benchmark]
  public void ArrayOfIntegers()
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
