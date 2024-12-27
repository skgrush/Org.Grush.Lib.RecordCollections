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

public record DataList<T>(
  ImmutableList<T> Underlying
) : IData<T, ImmutableList<T>>;

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
  public void StringDataListTest()
  {
    var deserialized = JsonSerializer.Deserialize(Raw, StringDataListContext.Default.DataListString);

    Asserter(deserialized);

    var reSerialized = JsonSerializer.Serialize(deserialized!, StringDataListContext.Default.DataListString);

    Assert.AreEqual(Raw, reSerialized);
  }


  [TestMethod]
  public void NameStructDataListTest()
  {
    var deserialized = JsonSerializer.Deserialize(NameStructRaw, NameStructDataListContext.Default.DataListNameStruct);

    NameStructAsserter(deserialized);

    var reSerialized = JsonSerializer.Serialize(deserialized!, NameStructDataListContext.Default.DataListNameStruct);

    Assert.AreEqual(NameStructRaw,reSerialized);
  }

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

  private void Asserter(IData<string, IReadOnlyList<string>>? obj)
  {
    Assert.IsNotNull(obj);

    var underlying = obj.Underlying;

    Assert.AreEqual(3, underlying.Count);
    Assert.AreEqual("c", underlying[2]);
  }

  private void NameStructAsserter(IData<NameStruct, IReadOnlyList<NameStruct>>? obj)
  {
    Assert.IsNotNull(obj);

    var underlying = obj.Underlying;

    Assert.AreEqual(2, underlying.Count);
    Assert.AreEqual("Echo", underlying[1].Name);
  }
}

[JsonSourceGenerationOptions(WriteIndented = true, Converters = [typeof(RecordCollectionJsonConverter<string>)])]
[JsonSerializable(typeof(DataRecordCollection<string>))]
[JsonSerializable(typeof(ImmutableList<string>))]
internal partial class StringDataRecordCollectionContext : JsonSerializerContext;

[JsonSourceGenerationOptions(Converters = [typeof(RecordCollectionJsonConverter<NameStruct>)])]
[JsonSerializable(typeof(DataRecordCollection<NameStruct>))]
[JsonSerializable(typeof(ImmutableList<NameStruct>))]
internal partial class NameStructDataRecordCollectionContext : JsonSerializerContext;



[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(DataList<string>))]
internal partial class StringDataListContext : JsonSerializerContext;


[JsonSourceGenerationOptions()]
[JsonSerializable(typeof(DataList<NameStruct>))]
internal partial class NameStructDataListContext : JsonSerializerContext;
