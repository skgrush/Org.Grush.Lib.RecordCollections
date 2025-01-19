using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Org.Grush.Lib.RecordCollections.AotTests;

public readonly record struct NameStruct(string Name);

public interface IData<out T, out TList> where TList : IReadOnlyList<T>
{
  public TList Underlying { get; }
}

public record DataRecordCollection<T>(
  RecordCollection<T> Underlying
) : IData<T, RecordCollection<T>>;


[TestClass]
public class AotSerializationExperiments
{
  [StringSyntax("json")]
  private const string Raw =
    """
    {
      "Underlying": [
        "a",
        "b",
        "c"
      ]
    }
    """;

  [StringSyntax("json")]
  private const string NameStructRaw =
    """
    {"Underlying":[{"Name":"Sam"},{"Name":"Echo"}]}
    """;

  [TestMethod]
  public void StringDataRecordCollectionTest()
  {
    var deserialized = JsonSerializer.Deserialize(Raw, StringDataRecordCollectionContext.Default.DataRecordCollectionString);

    Asserter(deserialized);
  }

  [TestMethod]
  public void NameStructDataRecordCollectionTest()
  {
    var deserialized = JsonSerializer.Deserialize(NameStructRaw, NameStructDataRecordCollectionContext.Default.DataRecordCollectionNameStruct);

    NameStructAsserter(deserialized);

    var reSerialized = JsonSerializer.Serialize(deserialized!, NameStructDataRecordCollectionContext.Default.DataRecordCollectionNameStruct);

    Assert.AreEqual(NameStructRaw,reSerialized);
  }

  private void Asserter<TStringList>(IData<string, TStringList>? obj) where TStringList : IReadOnlyList<string>
  {
    Assert.IsNotNull(obj);

    var underlying = obj.Underlying;

    Assert.AreEqual(3, underlying.Count);
    Assert.AreEqual("c", underlying[2]);
  }

  private void NameStructAsserter<TNSList>(IData<NameStruct, TNSList>? obj) where TNSList : IReadOnlyList<NameStruct>
  {
    Assert.IsNotNull(obj);

    var underlying = obj.Underlying;

    Assert.AreEqual(2, underlying.Count);
    Assert.AreEqual("Echo", underlying[1].Name);
  }
}

[JsonSourceGenerationOptions(WriteIndented = true, Converters = [typeof(RecordCollectionStrictJsonConverter<string>)])]
[JsonSerializable(typeof(DataRecordCollection<string>))]
[JsonSerializable(typeof(ImmutableArray<string>))]
internal partial class StringDataRecordCollectionContext : JsonSerializerContext;

[JsonSourceGenerationOptions(Converters = [typeof(RecordCollectionStrictJsonConverter<NameStruct>)])]
[JsonSerializable(typeof(DataRecordCollection<NameStruct>))]
[JsonSerializable(typeof(ImmutableArray<NameStruct>))]
internal partial class NameStructDataRecordCollectionContext : JsonSerializerContext;
