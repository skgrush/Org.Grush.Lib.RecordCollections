using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using FluentAssertions;
using Newtonsoft.Json;
using Org.Grush.Lib.RecordCollections.Newtonsoft;
using Org.Grush.Lib.RecordCollections.Tests.Utilities;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;
using NewtonsoftJsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace Org.Grush.Lib.RecordCollections.Tests;

[SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters")]
public static class ForSerializability
{
  [Theory]
  [ClassData(typeof(Serializers))]
  public static void WithMultipleLayers(string name,
    Func<TestRecord<RecordCollection<int>, RecordCollection<string>>, string> serializer)
  {
    var result = serializer(
      new(
        A: [1, 2, 3],
        B: ["1", "2", "3"]
      )
    );

    result
      .Should()
      .Be(
        """
        {"A":[1,2,3],"B":["1","2","3"]}
        """
      );
  }

  [Fact]
  public static void WithNonstandardSettings()
  {
    var result = JsonSerializer.Serialize(
      [
        "A",
        "B",
        "c",
      ],
      RecordCollectionOfStringNonStandardContext.Default.RecordCollectionString);

    result.Should()
      .Be("""
          [
            "A",
            "B",
            "c"
          ]
          """);
  }

  public static class ForcedEdgeCases
  {
    [Fact]
    public static void TryingToWriteWithNewtonsoftThrows()
    {
      // Assemble
      JsonWriter writer = new JsonTextWriter(new StringWriter());
      RecordCollection<int> data = [1, 2, 3];
      NewtonsoftJsonSerializer serializer = new();

      RecordCollectionNewtonsoftJsonConverter<int> converter = new();

      // Act/Assert
      var action = () => converter.WriteJson(writer, data, serializer);
      action.Should().ThrowExactly<NotSupportedException>();
    }

    [Fact]
    public static void SystemTextJson_UnhandledClosingArray()
    {
      var act = () => {
        // Assemble
        ReadOnlySpan<byte> bytes = "[ 1, "u8;

        JsonReaderState state = new();
        Utf8JsonReader reader = new Utf8JsonReader(bytes, false, state);
        JsonSerializerOptions options = new();

        RecordCollectionStrictJsonConverter<int> converter = new();

        // Act
        reader.Read();
        converter.Read(ref reader, typeof(RecordCollection<int>), options);
      };

      // Assert
      act.Should()
        .ThrowExactly<JsonException>()
        .Where(e => e.Message.Contains("Bad json end"));
    }
  }

  private class Serializers : TheoryData<string, Func<TestRecord<RecordCollection<int>, RecordCollection<string>>, string>>
  {
    public Serializers()
    {
      Add("System.Text.Json dynamic", obj => JsonSerializer.Serialize(obj, new JsonSerializerOptions
      {
        Converters = { new RecordCollectionJsonConverterFactory() }
      }));
      Add("System.Text.Json context",
        obj => JsonSerializer.Serialize(obj,
          TestRecordRecordCollectionIntStringContext.Default.TestRecordRecordCollectionInt32RecordCollectionString));
      Add("Newtonsoft factory", obj => JsonConvert.SerializeObject(obj, new JsonSerializerSettings
      {
        Converters = { new RecordCollectionNewtonsoftJsonConverterFactory() }
      }));
      Add("Newtonsoft explicit", obj => JsonConvert.SerializeObject(obj, new JsonSerializerSettings
      {
        Converters =
        {
          new RecordCollectionNewtonsoftJsonConverter<int>(),
          new RecordCollectionNewtonsoftJsonConverter<string>(),
        }
      }));
    }
  }
}
