using FluentAssertions;

namespace Org.Grush.Lib.RecordCollections.Tests;

using TestRecordOfInts = TestRecord<int, int>;
using TestRecordOfCollections = TestRecord<RecordCollection<int>, RecordCollection<int>>;
public record TestRecord<TA, TB>(TA A, TB B);

public class RecordCollectionShould
{
  public class BeEquatable
  {
    [Fact]
    public void ForValueTypes()
    {
      // Assemble
      var collectionA = RecordCollection.Create([1, 2, 3]);
      var collectionB = RecordCollection.Create([1, 2, 3]);

      // Act
      var areEqual = collectionA.Equals(collectionB);

      // Assert
      areEqual.Should().BeTrue();
    }

    [Fact]
    public void ForReferenceTypes()
    {
      // Assemble
      var collectionA = RecordCollection.Create([new TestRecordOfInts(1, 2), new TestRecordOfInts(3, 4)]);
      var collectionB = RecordCollection.Create([new TestRecordOfInts(1, 2), new TestRecordOfInts(3, 4)]);

      // Act
      var areEqual = collectionA.Equals(collectionB);

      // Assert
      areEqual.Should().BeTrue();
    }

    [Fact]
    public void AsMembersOfRecords()
    {
      // Assemble
      var recordA = new TestRecordOfCollections([1, 2], [3, 4]);
      var recordB = new TestRecordOfCollections([1, 2], [3, 4]);

      // Act
      var areEqual = recordA.Equals(recordB);

      // Assert
      areEqual.Should().BeTrue();
    }
  }

  public class BeSerializable
  {
    [Theory]
    [ClassData(typeof(Serializers))]
    public void WithMultipleLayers(string name, Func<TestRecord<RecordCollection<int>, RecordCollection<string>>, string> serializer)
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

    public class Serializers : TheoryData<string, Func<object, string>>
    {
      public Serializers()
      {
        Add("System.Text.Json", obj => System.Text.Json.JsonSerializer.Serialize(obj));
        Add("Newtonsoft", obj => Newtonsoft.Json.JsonConvert.SerializeObject(obj));
      }
    }
  }

  public class BeDeserializable
  {
    [Theory]
    [ClassData(typeof(Deserializers<TestRecord<RecordCollection<int>, RecordCollection<string>>>))]
    public void WithMultipleLayers(string name,
      Func<string, TestRecord<RecordCollection<int>, RecordCollection<string>>> deserializer)
    {
      var json =
        """
        {"A":[1,2,3],"B":["1","2","3"]}
        """;

      deserializer(json)
        .Should()
        .BeEquivalentTo(new TestRecord<RecordCollection<int>, RecordCollection<string>>(
          A: [1,2,3],
          B: ["1", "2", "3"]
        ));
    }


    public class Deserializers<T> : TheoryData<string, Func<string, T>>
    {
      public Deserializers()
      {
        Add("System.Text.Json", str => System.Text.Json.JsonSerializer.Deserialize<T>(str)!);
        Add("Newtonsoft", str => Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str)!);
      }
    }
  }
}
