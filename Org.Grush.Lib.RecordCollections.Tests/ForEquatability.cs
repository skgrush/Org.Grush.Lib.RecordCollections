using FluentAssertions;
using FluentAssertions.Execution;

namespace Org.Grush.Lib.RecordCollections.Tests;

public class ForEquatability
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
    HashSet<RecordCollection<double>> set =
    [
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
