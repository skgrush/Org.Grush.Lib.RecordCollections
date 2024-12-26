using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using Newtonsoft.Json;
using Org.Grush.Lib.RecordCollections.Newtonsoft;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Org.Grush.Lib.RecordCollections.Tests;

using TestRecordOfInts = TestRecord<int, int>;
using TestRecordOfCollections = TestRecord<RecordCollection<int>, RecordCollection<int>>;
public record TestRecord<TA, TB>(TA A, TB B);

public class RecordCollectionShould
{
  public class BeInitializable
  {
    [Fact]
    public void UsingStaticCreateMethod()
    {
      // Act
      var collection = RecordCollection.Create(["A", "B"]);

      // Assert
      collection
        .Should()
        .SatisfyRespectively(
          a => a.Should().Be("A"),
          b => b.Should().Be("B")
        );
    }

    [Fact]
    public void UsingCollectionExpression()
    {
      // Act
      RecordCollection<double> collection = [3.14159, double.NaN, double.PositiveInfinity];

      // Assert
      collection
        .Should()
        .SatisfyRespectively(
          a => a.Should().Be(3.14159),
          b => b.Should().Be(double.NaN),
          c => c.Should().Be(double.PositiveInfinity)
        );
    }

    [Fact]
    public void UsingTheEnumerableExtension()
    {
      // Assemble
      List<int> oldList = [1, 2, 3];

      // Act
      var coll = oldList.ToRecordCollection();

      // Assert
      coll
        .Should()
        .BeOfType<RecordCollection<int>>()
        .And
        .BeEquivalentTo(oldList);
    }
  }

  public class BeEquatable
  {
    [Fact]
    public void WhenContainingValueTypes()
    {
      // Assemble
      RecordCollection<int> collectionA = [1, 2, 3];
      RecordCollection<int> collectionB = [1, 2, 3];

      // Act
      var areEqual = Equals(collectionA, collectionB);

      // Assert
      areEqual.Should().BeTrue();
    }

    [Fact]
    public void WhenContainingReferenceTypes()
    {
      // Assemble
      var collectionA = RecordCollection.Create([new TestRecordOfInts(1, 2), new TestRecordOfInts(3, 4)]);
      var collectionB = RecordCollection.Create([new TestRecordOfInts(1, 2), new TestRecordOfInts(3, 4)]);

      // Act
      var areEqual = Equals(collectionA, collectionB);

      // Assert
      areEqual.Should().BeTrue();
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void WithNullValues(bool oneEmpty, bool reversed)
    {
      // Assemble
      RecordCollection<int>? collectionA = oneEmpty ? [] : [1, 2, 3];
      RecordCollection<int>? collectionB = null;

      if (reversed)
        (collectionA, collectionB) = (collectionB, collectionA);

      // Act
      var areEqual = Equals(collectionA, collectionB);

      // Assert
      areEqual
        .Should()
        .BeFalse();
    }

    [Fact]
    public void ForUseInDictionaries()
    {
      // Assemble
      RecordCollection<string> collectionA = ["a", "b"];
      RecordCollection<string> collectionB = ["a", "b"];
      RecordCollection<string> unrelatedCollection = ["b", "a"];

      Dictionary<RecordCollection<string>, object> testDict = [];

      // Act
      var uniqueValue = new { Anonymous = true };

      testDict[collectionA] = uniqueValue;

      var retrievedValue = testDict[collectionB];

      var containsUnrelatedCollection = testDict.ContainsKey(unrelatedCollection);

      // Assert
      retrievedValue.Should().BeSameAs(uniqueValue);
      containsUnrelatedCollection.Should().BeFalse();
    }

    [Fact]
    public void ForUseInHashSets()
    {
      // Assemble
      RecordCollection<double> collectionA = [3.14159, double.PositiveInfinity];
      RecordCollection<double> collectionB = [3.14159, double.PositiveInfinity];
      RecordCollection<double> unrelatedCollection = [double.PositiveInfinity, 3.14159];

      HashSet<RecordCollection<double>> set = [];

      // Act
      set.Add(collectionA);

      var containCollectionB = set.Contains(collectionB);

      var containsUnrelatedCollection = set.Contains(unrelatedCollection);

      // Assert
      containCollectionB.Should().BeTrue();
      containsUnrelatedCollection.Should().BeFalse();
    }

    [Fact]
    public void AsMembersOfRecords()
    {
      // Assemble
      var recordA = new TestRecordOfCollections([1, 2], [3, 4]);
      var recordB = new TestRecordOfCollections([1, 2], [3, 4]);

      // Act
      var areEqual = Equals(recordA, recordB);

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
        Add("System.Text.Json", obj => JsonSerializer.Serialize(obj, new JsonSerializerOptions
        {
          Converters = { new RecordCollectionJsonConverterFactory() }
        }));
        Add("Newtonsoft", obj => JsonConvert.SerializeObject(obj, new JsonSerializerSettings
        {
          Converters = { new RecordCollectionNewtonsoftJsonConverterFactory() }
        }));
      }
    }
  }

  public class BeDeserializable
  {
    [Theory]
    [ClassData(typeof(RuntimeDeserializers<RecordCollection<string?>>))]
    public void AtRuntime_WithSingleLayer(string name, Func<string, RecordCollection<string?>> deserializer)
    {
      var json =
        """
        ["a", "b", null]
        """;

      deserializer(json)
        .Should()
        .BeEquivalentTo(RecordCollection.Create(["a", "b", null]));
    }

    [Theory]
    [ClassData(typeof(RuntimeDeserializers<RecordCollection<TestRecord<RecordCollection<int>, RecordCollection<string>>>>))]
    public void AtRuntime_WithMultipleLayers(string name,
      Func<string, RecordCollection<TestRecord<RecordCollection<int>, RecordCollection<string>>>> deserializer)
    {
      var json =
        """
        [{"A":[1,2,3],"B":["1","2","3"]}]
        """;

      deserializer(json)
        .Should()
        .BeEquivalentTo(RecordCollection.Create([new TestRecord<RecordCollection<int>, RecordCollection<string>>(
          A: [1,2,3],
          B: ["1", "2", "3"]
        )]));
    }

    [Fact]
    public void WithSourceGeneratedDeserializer()
    {
      var json =
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

    public class RuntimeDeserializers<T> : TheoryData<string, Func<string, T>>
    {
      public RuntimeDeserializers()
      {
        Add("System.Text.Json", str => System.Text.Json.JsonSerializer.Deserialize<T>(str)!);
        Add("Newtonsoft", str => JsonConvert.DeserializeObject<T>(value: str, settings: new JsonSerializerSettings
        {
          Converters = { new RecordCollectionNewtonsoftJsonConverterFactory() }
        })!);
      }
    }
  }
}

[JsonSourceGenerationOptions(WriteIndented = true, Converters = [typeof(RecordCollectionJsonConverter<string>)])]
[JsonSerializable(typeof(RecordCollection<string>))]
[JsonSerializable(typeof(ImmutableList<string>))]
internal partial class RecordCollectionOfStringContext : JsonSerializerContext;
