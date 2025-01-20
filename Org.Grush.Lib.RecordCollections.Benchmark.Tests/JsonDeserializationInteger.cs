using System.Text.Json;
using System.Text.Json.Serialization;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Org.Grush.Lib.RecordCollections.Benchmark.Tests;

[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.NativeAot80)]
public class JsonDeserializationInt
{
  [Params(10, 1000, 10_000)]
  public int Length { get; set; }

  private string _intData;

  [GlobalSetup]
  public void Setup()
  {
    Random random = new();

    _intData =
      '[' +
      string.Join(',', Enumerable.Range(0, Length).Select(_ => random.Next(0, 1_000_000_000))) +
      ']';
  }

  [Benchmark]
  public List<int> Deserialization_IntList()
  {
    List<int> list = JsonSerializer.Deserialize(_intData, JsonIntegerContext.Default.ListInt32)!;
    return list;
  }

  [Benchmark]
  public RecordCollection<int> Deserialization_IntRecordCollection()
  {
    RecordCollection<int> recordCollection = JsonSerializer.Deserialize(_intData, JsonIntegerContext.Default.RecordCollectionInt32)!;
    return recordCollection;
  }

  [Benchmark]
  public int[] Deserialization_IntArray()
  {
    int[] array = JsonSerializer.Deserialize(_intData, JsonIntegerContext.Default.Int32Array)!;
    return array;
  }
}

[JsonSourceGenerationOptions(Converters = [typeof(RecordCollectionStrictJsonConverter<int>)])]
[JsonSerializable(typeof(RecordCollection<int>))]
[JsonSerializable(typeof(List<int>))]
[JsonSerializable(typeof(int[]))]
public partial class JsonIntegerContext : JsonSerializerContext;
