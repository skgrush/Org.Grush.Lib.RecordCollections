using System.Text.Json;
using System.Text.Json.Serialization;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Org.Grush.Lib.RecordCollections.Benchmark.Tests;

using NumberData = MyRecord<int, double, string>;

public record MyRecord<T0, T1, T2>(T0 A, T1 B, T2 C);



[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.NativeAot80)]
public class JsonDeserializationObject
{
  [Params(10, 1000, 10_000)]
  public int N { get; set; }

  private string _numberData; // MyRecord<int, double, string>

  public void GlobalSetup()
  {
    Random random = new();

    _numberData =
      '[' +
      string.Join(',', Enumerable.Range(0, N).Select(_ =>
      {
        var n = random.Next(0, 1_000_000_000);
        return $$"""{ "A": {{n}}, "B": {{n}}.00, "C": "{{n}}"  }""";
      })) +
      ']';
  }

  [Benchmark]
  public List<NumberData> Deserialization_NumberObject_List()
  {
    List<NumberData> list = JsonSerializer.Deserialize(_numberData, NumberDataJsonContext.Default.ListMyRecordInt32DoubleString)!;
    return list;
  }

  [Benchmark]
  public RecordCollection<NumberData> Deserialization_NumberObject_RecordCollection()
  {
    RecordCollection<NumberData> recordCollection = JsonSerializer.Deserialize(_numberData, NumberDataJsonContext.Default.RecordCollectionMyRecordInt32DoubleString)!;
    return recordCollection;
  }

  [Benchmark]
  public NumberData[] Deserialization_NumberObject_Array()
  {
    NumberData[] array = JsonSerializer.Deserialize(_numberData, NumberDataJsonContext.Default.MyRecordInt32DoubleStringArray)!;
    return array;
  }
}

[JsonSourceGenerationOptions(Converters = [typeof(RecordCollectionStrictJsonConverter<NumberData>)])]
[JsonSerializable(typeof(RecordCollection<NumberData>))]
[JsonSerializable(typeof(List<NumberData>))]
[JsonSerializable(typeof(NumberData[]))]
public partial class NumberDataJsonContext : JsonSerializerContext;
