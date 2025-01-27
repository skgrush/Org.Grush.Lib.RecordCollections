using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using FluentAssertions;
using Newtonsoft.Json;
using Org.Grush.Lib.RecordCollections.Newtonsoft;
using Org.Grush.Lib.RecordCollections.Tests.Utilities;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Org.Grush.Lib.RecordCollections.Tests;

[SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters")]
public class ForSerializability
{
  [Theory]
  [ClassData(typeof(Serializers))]
  public void WithMultipleLayers(string name,
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
  public void WithNonstandardSettings()
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

  private class
    Serializers : TheoryData<string, Func<TestRecord<RecordCollection<int>, RecordCollection<string>>, string>>
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
      Add("Newtonsoft", obj => JsonConvert.SerializeObject(obj, new JsonSerializerSettings
      {
        Converters = { new RecordCollectionNewtonsoftJsonConverterFactory() }
      }));
    }
  }
}
