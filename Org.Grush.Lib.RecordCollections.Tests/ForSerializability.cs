using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using Org.Grush.Lib.RecordCollections.Newtonsoft;
using Org.Grush.Lib.RecordCollections.Tests.Utilities;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;
using NewtonsoftJsonConverter = Newtonsoft.Json.JsonConverter;
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

  [Theory]
  [InlineData(true)]
  [InlineData(false)]
  public static void NewtonsoftWithConverterCacheIsEnabled(bool enabled)
  {
    var genericCollectionOf = typeof(int);

    var mockType = new Mock<Type>(MockBehavior.Strict);
    mockType.Setup(m => m.GetGenericArguments()).Returns([genericCollectionOf]);
    mockType.Setup(m => m.GetHashCode()).Returns(1);
    mockType.Setup(m => m.ToString()).Returns("_.Mock.Type");
    mockType.Setup(m => m.Equals(It.IsAny<object>())).Returns((object v) => ReferenceEquals(v, mockType.Object));

    NewtonsoftJsonSerializer serializer = new();

    var factory = new RecordCollectionNewtonsoftJsonConverterFactory(useConverterCache: enabled);

    factory.ReadJson(CreateReaderMock().Object, mockType.Object, null, serializer);
    factory.ReadJson(CreateReaderMock().Object, mockType.Object, null, serializer);

    mockType.Verify(m => m.GetGenericArguments(), enabled ? Times.Once() : Times.Exactly(2));

    return;

    static Mock<JsonReader> CreateReaderMock()
    {
      var mockReader = new Mock<JsonReader>(MockBehavior.Strict);
      List <(JsonToken Token, string Path, int Depth)> sequence =
      [
        (JsonToken.StartArray, "$.Start", 0),
        (JsonToken.EndArray, "$.End", 1),
      ];
      using var enumerator = sequence.GetEnumerator();

      mockReader.Setup(r => r.Read()).Returns(() => enumerator.MoveNext());
      mockReader.SetupGet(r => r.Path).Returns(() => enumerator.Current.Path);
      mockReader.SetupGet(r => r.TokenType).Returns(() => enumerator.Current.Token);
      mockReader.SetupGet(r => r.Depth).Returns(() => enumerator.Current.Depth);
      mockReader.Setup(r => r.ReadAsInt32()).Returns(() =>
      {
        enumerator.MoveNext();
        return null;
      });

      return mockReader;
    }
  }


  public static class ForcedEdgeCases
  {
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public static void TryingToWriteWithNewtonsoftThrows(bool useFactory)
    {
      // Assemble
      JsonWriter writer = new JsonTextWriter(new StringWriter());
      RecordCollection<int> data = [1, 2, 3];
      NewtonsoftJsonSerializer serializer = new();

      NewtonsoftJsonConverter converter = useFactory
        ? new RecordCollectionNewtonsoftJsonConverterFactory()
        : new RecordCollectionNewtonsoftJsonConverter<int>();

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

    [Fact]
    public static void SystemTextJson_NonStrictSubConverter()
    {
      var act = () => {
        // Assemble
        ReadOnlySpan<byte> bytes = """[ { "key": 1 } ]"""u8;

        JsonReaderState state = new();
        Utf8JsonReader reader = new Utf8JsonReader(bytes, false, state);
        JsonSerializerOptions options = new();
        options.Converters.Add(new ImpreciseConverterFactory());

        RecordCollectionStrictJsonConverter<Record> converter = new();

        // Act
        reader.Read();
        converter.Read(ref reader, typeof(RecordCollection<Record>), options);
      };

      act.Should()
        .ThrowExactly<JsonException>()
        .Where(e => e.Message.Contains("SubConverter for "));
    }

    private record Record(int Key);

    private class ImpreciseConverterFactory : JsonConverterFactory
    {
      public override bool CanConvert(Type typeToConvert)
        => !typeToConvert.IsGenericType;

      public override System.Text.Json.Serialization.JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        => new ImpreciseConverter();
    }

    private class ImpreciseConverter : System.Text.Json.Serialization.JsonConverter<object>
    {
      public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => throw new NotImplementedException();

      public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        => throw new NotImplementedException();
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
