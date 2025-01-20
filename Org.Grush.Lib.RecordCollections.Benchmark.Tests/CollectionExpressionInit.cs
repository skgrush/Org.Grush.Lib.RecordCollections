using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Org.Grush.Lib.RecordCollections.Benchmark.Tests;

[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.NativeAot80)]
public class CollectionExpressionInit
{

  [Params(32)]
  public int N { get; set; }

  [Benchmark]
  public List<int> ExpressionInit_IntList()
  {
    List<int> list = [1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32];
    return list;
  }

  [Benchmark]
  public RecordCollection<int> ExpressionInit_IntRecordCollection()
  {
    RecordCollection<int> recordCollection = [1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32];
    return recordCollection;
  }

  [Benchmark]
  public int[] ExpressionInit_IntArray()
  {
    int[] array = [1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32];
    return array;
  }
}
