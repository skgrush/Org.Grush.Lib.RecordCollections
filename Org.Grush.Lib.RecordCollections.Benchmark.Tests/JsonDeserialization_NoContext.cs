using System.Text.Json;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Org.Grush.Lib.RecordCollections.Benchmark.Tests;

using DtoOfLists = SomeRecord<List<string>, List<int>>;
using DtoOfCollections = SomeRecord<RecordCollection<string>, RecordCollection<int>>;

public record SomeRecord<T1, T2>(
  int Id,
  T1 Strings,
  T2 Numbers
)
  where T1 : IEnumerable<string>
  where T2 : IEnumerable<int>
;

[SimpleJob(RuntimeMoniker.Net80)]
public class JsonDeserialization_NoContext
{
  [Params(10, 500)]
  public int N { get; set; }

  private string _listOfDtoOfList;

  [GlobalSetup]
  public void GlobalSetup()
  {
    Random random = new();

    List<DtoOfLists> listDtos = new(N);

    for (int i = 0; i < N; i++)
    {
      List<int> innerInt = Enumerable.Range(0, i).Select(_ => random.Next(0, N)).ToList();
      List<string> innerStr = innerInt.Select(v => v.ToString()).ToList();

      listDtos.Add(new(i, innerStr, innerInt));
    }

    _listOfDtoOfList = JsonSerializer.Serialize(listDtos);
  }

  [Benchmark]
  public List<DtoOfLists> DynamicDeserialization_List()
  {
    var v = JsonSerializer.Deserialize<List<DtoOfLists>>(_listOfDtoOfList);
    return v;
  }

  [Benchmark]
  public RecordCollection<DtoOfCollections> DynamicDeserialization_RecordCollection()
  {
    var v = JsonSerializer.Deserialize<RecordCollection<DtoOfCollections>>(_listOfDtoOfList);
    return v;
  }
}
