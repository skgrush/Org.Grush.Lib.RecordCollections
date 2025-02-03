using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Newtonsoft.Json;
using Org.Grush.Lib.RecordCollections.Newtonsoft;
using Org.Grush.Lib.RecordCollections.Tests.Utilities;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Org.Grush.Lib.RecordCollections.Tests;

[SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters")]
public static class ForDeserializability
{
  [Theory]
  [ClassData(typeof(RuntimeDeserializers<RecordCollection<string?>>))]
  public static void AtRuntime_WithSingleLayer(string name, Func<string, RecordCollection<string?>> deserializer)
  {
    const string json =
      """
      ["a", "b", null]
      """;

    deserializer(json)
      .Should()
      .BeEquivalentTo(RecordCollection.Create(["a", "b", null]));
  }

  [Theory]
  [ClassData(
    typeof(RuntimeDeserializers<RecordCollection<TestRecord<RecordCollection<int>, RecordCollection<string>>>>))]
  public static void AtRuntime_WithMultipleLayers(string name,
    Func<string, RecordCollection<TestRecord<RecordCollection<int>, RecordCollection<string>>>> deserializer)
  {
    const string json =
      """
      [{"A":[1,2,3],"B":["1","2","3"]}]
      """;

    deserializer(json)
      .Should()
      .BeEquivalentTo(RecordCollection.Create([
        new TestRecord<RecordCollection<int>, RecordCollection<string>>(
          A: [1, 2, 3],
          B: ["1", "2", "3"]
        )
      ]));
  }

  public static class WithSourceGeneratedDeserializer
  {
    [Fact]
    public static void WhenEmptyArray()
    {
      const string json = "[]";

      JsonSerializer.Deserialize(
          json: json,
          jsonTypeInfo: RecordCollectionOfStringContext.Default.RecordCollectionString
        )
        .Should()
        .BeEmpty();
    }

    [Fact]
    public static void InSimpleCase()
    {
      const string json =
        """
        ["a", "b", "c"]
        """;

      JsonSerializer.Deserialize(
          json: json,
          jsonTypeInfo: RecordCollectionOfStringContext.Default.RecordCollectionString
        )
        .Should()
        .BeEquivalentTo(RecordCollection.Create(["a", "b", "c"]));
    }

    [Fact]
    public static void WithNonStandardSettings()
    {
      const string json =
        """
        ["a",
          // Just had to interject here
          /*
            comments in JSON are cool but problematic
            */
        "b", "c"
        ,
        ]
        """;

      JsonSerializer.Deserialize(
          json: json,
          jsonTypeInfo: RecordCollectionOfStringNonStandardContext.Default.RecordCollectionString
        )
        .Should()
        .BeEquivalentTo(RecordCollection.Create(["a", "b", "c"]));
    }

    [Theory]
    [InlineData("is empty", "")]
    [InlineData("is null", "null")]
    [InlineData("no start", " 1, 2, 3 ]")]
    [InlineData("no end", "[ 1, 2, 3 ")]
    [InlineData("no end", "[ 1, 2, 3 , ")]
    [InlineData("no brackets", " 1, 2, 3")]
    public static void ThrowJsonExceptionWhen(string _, string json)
    {
      // Assemble
      Action act = () =>
        JsonSerializer.Deserialize(json, RecordCollectionOfStringContext.Default.RecordCollectionString);

      // Act/Assert
      act
        .Should()
        .ThrowExactly<JsonException>();
    }
  }

  private class RuntimeDeserializers<T> : TheoryData<string, Func<string, T>>
  {
    public RuntimeDeserializers()
    {
      Add("System.Text.Json dynamic", str => JsonSerializer.Deserialize<T>(str)!);
      Add("Newtonsoft", str => JsonConvert.DeserializeObject<T>(value: str, settings: new JsonSerializerSettings
      {
        Converters = { new RecordCollectionNewtonsoftJsonConverterFactory() }
      })!);
    }
  }
}
