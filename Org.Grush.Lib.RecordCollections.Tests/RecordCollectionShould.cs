using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using FluentAssertions.Execution;
using Newtonsoft.Json;
using Org.Grush.Lib.RecordCollections.Newtonsoft;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;
// ReSharper disable NotAccessedPositionalProperty.Global

namespace Org.Grush.Lib.RecordCollections.Tests;

using TestRecordOfInts = TestRecord<int, int>;
using TestRecordOfCollections = TestRecord<RecordCollection<int>, RecordCollection<int>>;
public record TestRecord<TA, TB>(TA A, TB B);

[SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters")]
public class RecordCollectionShould
{
  public class BeInitializable
  {
    [Fact]
    public void ForEmpty()
    {
      // Act
      var coll = RecordCollection.Create<double>();

      // Assert
      coll
        .IsEmpty
        .Should()
        .BeTrue();
    }

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
    public void UsingEnumerableExtension()
    {
      // Assemble
      List<int> oldList = [1, 2, 3];

      // Act
      var coll1 = oldList.ToRecordCollection();
      var coll2 = coll1.ToRecordCollection();

      // Assert
      coll1
        .Should()
        .BeOfType<RecordCollection<int>>()
        .And
        .BeEquivalentTo(oldList);

      coll1
        .Should()
        .Equal(coll2);
    }

    [Fact]
    public void UsingEnumerableExtension_ForReadOnlySpan()
    {
      // Assemble
      ReadOnlySpan<string> span = ["a", "b", "c"];

      // Act
      var coll1 = span.ToRecordCollection();

      // Assert
      coll1
        .Should()
        .BeOfType<RecordCollection<string>>()
        .And
        .BeEquivalentTo("a", "b", "c");
    }

    [Fact]
    public void UsingCreateRange()
    {
      // Assemble
      List<string> input = ["a", "b"];

      // Act
      var collection = RecordCollection.CreateRange(input);

      collection
        .Should()
        .BeEquivalentTo(["a", "b"]);
    }
  }

  public class BeImmutable
  {
    [Fact]
    public void AndAlwaysReadOnly()
    {
      using var _ = new AssertionScope();

      var collection = RecordCollection.Create([1, 2]);

      collection
        .IsReadOnly
        .Should()
        .BeTrue();

      collection.GetType()
        .GetMember(nameof(RecordCollection<object>.IsReadOnly))
        .Should()
        .SatisfyRespectively(
          member => member
            .As<PropertyInfo>()
            .Should()
            .NotBeWritable()
        );
    }

    [Theory]
    [ClassData(typeof(UnsupportedMethods))]
    public void AndThrowForUnsupportedMethods(string name, Action action)
    {
      action
        .Should()
        .Throw<Exception>()
        .And
        .Should()
        .Match((Exception e) =>
          (e.InnerException ?? e) is NotSupportedException
        );
    }

    private class UnsupportedMethods : TheoryData<string, Action>
    {
      public UnsupportedMethods()
      {
        RecordCollection<long> collection = [];

        var functions = new MulticastDelegate[] {
          ((ICollection<long>)collection).Add,
          ((ICollection<long>)collection).Clear,
          ((ICollection<long>)collection).Remove,
          ((IList<long>)collection).Insert,
          ((IList<long>)collection).RemoveAt,
        };

        foreach (var fn in functions)
        {
          Add(
            fn.Method.Name,
            GetDefaultCallOfMethod(collection, fn.Method)
          );
        }

        Add("RecordCollection<T>[0] =", (() => collection[0] = 0));
      }
    }

    private static Action GetDefaultCallOfMethod<T>(T instance, MethodInfo method)
    {
      var theParams = method.GetParameters().Select(p =>
        p.ParameterType.IsValueType
          ? Activator.CreateInstance(p.ParameterType)
          : null
      ).ToArray();

      return () =>
      {
        method.Invoke(instance, BindingFlags.Public | BindingFlags.Instance, null, theParams, null);
      };
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
    public void AndEqualWithNullValues(bool oneEmpty, bool reversed)
    {
      // Assemble
      RecordCollection<int>? collectionA = oneEmpty ? [] : [1, 2, 3];
      RecordCollection<int>? collectionB = null;

      if (reversed)
        (collectionA, collectionB) = (collectionB, collectionA);

      // Act/Assert
      using var _ = new AssertionScope();

      (collectionA == collectionB)
        .Should()
        .BeFalse();

      (collectionA != collectionB)
        .Should()
        .BeTrue();
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

      // Act
      HashSet<RecordCollection<double>> set = [
        collectionA,
      ];

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

    public class ViaStaticMethod
    {
      [Theory]
      [InlineData(true)]
      [InlineData(false)]
      public void WhenNeitherNull(bool shouldBeEqual)
      {
        // Assemble
        RecordCollection<int>? collB = [1, 2, 3];
        RecordCollection<int>? collA = shouldBeEqual ? [1, 2, 3] : null;

        // Act
        bool areEqual = RecordCollection<int>.Equals(collA, collB);

        // Assert
        areEqual.Should().Be(shouldBeEqual);
      }

      [Fact]
      public void WhenBothNull()
      {
        RecordCollection<string>.Equals(null, null)
          .Should()
          .BeTrue();
      }

      [Theory]
      [InlineData(false, true)]
      [InlineData(true, false)]
      public void WhenMismatchedNullable(bool firstIsNull, bool secondIsNull)
      {
        // Assemble
        RecordCollection<int>? firstColl = firstIsNull ? null : [1, 2, 3, 4];
        RecordCollection<int>? secondColl = secondIsNull ? null : [1, 2, 3, 4];

        // Act
        var areEqual = RecordCollection<int>.Equals(firstColl, secondColl);

        // Assert
        areEqual.Should().BeFalse();
      }
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

    private class Serializers : TheoryData<string, Func<TestRecord<RecordCollection<int>, RecordCollection<string>>, string>>
    {
      public Serializers()
      {
        Add("System.Text.Json dynamic", obj => JsonSerializer.Serialize(obj, new JsonSerializerOptions
        {
          Converters = { new RecordCollectionJsonConverterFactory() }
        }));
        Add("System.Text.Json context", obj => JsonSerializer.Serialize(obj, TestRecordRecordCollectionIntStringContext.Default.TestRecordRecordCollectionInt32RecordCollectionString));
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
      const string json =
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
      const string json =
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

    public class WithSourceGeneratedDeserializer
    {
      [Fact]
      public void WhenEmptyArray()
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
      public void InSimpleCase()
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
      public void WithNonStandardSettings()
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
      public void ThrowJsonExceptionWhen(string _, string json)
      {
        // Assemble
        Action act = () => JsonSerializer.Deserialize(json, RecordCollectionOfStringContext.Default.RecordCollectionString);

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
}

[JsonSourceGenerationOptions(Converters = [typeof(RecordCollectionStrictJsonConverter<string>)])]
[JsonSerializable(typeof(RecordCollection<string>))]
[JsonSerializable(typeof(string))]
internal partial class RecordCollectionOfStringContext : JsonSerializerContext;

[JsonSourceGenerationOptions(WriteIndented = true, AllowTrailingCommas = true, ReadCommentHandling = JsonCommentHandling.Skip, Converters = [typeof(RecordCollectionStrictJsonConverter<string>)])]
[JsonSerializable(typeof(RecordCollection<string>))]
[JsonSerializable(typeof(string))]
internal partial class RecordCollectionOfStringNonStandardContext : JsonSerializerContext;

[JsonSourceGenerationOptions(Converters = [typeof(RecordCollectionStrictJsonConverter<int>), typeof(RecordCollectionStrictJsonConverter<string>)])]
[JsonSerializable(typeof(TestRecord<RecordCollection<int>, RecordCollection<string>>))]
[JsonSerializable(typeof(RecordCollection<string>))]
[JsonSerializable(typeof(RecordCollection<int>))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(int))]
internal partial class TestRecordRecordCollectionIntStringContext : JsonSerializerContext;
