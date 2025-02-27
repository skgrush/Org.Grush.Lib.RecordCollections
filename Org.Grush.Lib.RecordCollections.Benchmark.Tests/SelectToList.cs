using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Org.Grush.Lib.RecordCollections.Benchmark.Tests;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80)]
public class SelectToList
{
  public const string Title = "LINQ iteration";
  public const string Subtitle = "Iterate of a prebuilt collection with `.Select(x => x).ToList();`";

  [Params(10, 1000, 10_000)]
  public int Length { get; set; }

  private List<int> _initialList = null!;
  private RecordCollection<int> _initialRecordCollection;

  [GlobalSetup]
  public void SetupSelectToList()
  {
    var random = new Random();
    _initialList = Enumerable.Range(0, Length).Select(_ => random.Next(0, Length)).ToList();
    _initialRecordCollection = _initialList.ToRecordCollection();
  }

  [Benchmark(Baseline = true)]
  public List<int> ListSelectToList()
  {
    return _initialList.Select(x => x).ToList();
  }

  [Benchmark]
  public List<int> RecordCollectionSelectToList()
  {
    return _initialRecordCollection.Select(x => x).ToList();
  }

  [Benchmark]
  public RecordCollection<int> ListSelectToRecordCollection()
  {
    return _initialList.Select(x => x).ToRecordCollection();
  }
}
