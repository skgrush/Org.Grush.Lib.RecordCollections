using System.Collections;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using Xunit.Sdk;

namespace Org.Grush.Lib.RecordCollections.Tests;

public class ForEquatability
{
  [Fact]
  public void WhenContainingValueTypes()
  {
    // Assemble
    RecordCollection<int> collectionA = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
    RecordCollection<int> collectionB = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

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

  public class Structural
  {
    [Fact]
    public void ShouldReturnFalseAgainstNull()
    {
      RecordCollection<int> collectionA = [1, 2, 3];
      RecordCollection<int>? collectionB = null;

      var comparerMock = MakeAlwaysTrueComparer();

      var areEqual = ((IStructuralEquatable)collectionA).Equals(collectionB, comparerMock.Object);

      areEqual.Should().BeFalse();
      comparerMock.Verify(m => m.Equals(It.IsAny<int>(), It.IsAny<long>()), Times.Never);
      comparerMock.Verify(m => m.GetHashCode(It.IsAny<object>()), Times.Never);
    }

    [Fact]
    public void WhenComparerIsOfT_AndOtherIsRecordCollectionOfT_ShouldUseTComparer()
    {
      RecordCollection<byte> collectionA = [1, 2, 3];
      RecordCollection<byte>? collectionB = [1, 2, 3];

      var comparerMock = MakeComparerOf<byte>(null, null);
      comparerMock.Setup(c => c.Equals(It.IsAny<byte>(), It.IsAny<byte>())).Returns(true);

      var areEqual = ((IStructuralEquatable)collectionA).Equals(collectionB, (IEqualityComparer)comparerMock.Object);

      areEqual.Should().BeTrue();

      comparerMock.Verify(c => c.Equals(It.IsAny<byte>(), It.IsAny<byte>()), Times.Exactly(3));
    }

    [Fact]
    public void WhenComparerIsOfT_AndOtherIsIEnumerableOfT_ShouldUseTComparer()
    {
      RecordCollection<byte> collectionA = [1, 2, 3];
      IEnumerable<byte> enumerableB = Enumerable.Range(1, 3).Select(x => (byte)x);
      var comparer = MakeComparerOf<byte>(null, null);
      comparer.Setup(c => c.Equals(It.IsAny<byte>(), It.IsAny<byte>())).Returns((byte a, byte b) => a == b);

      var areEqual = ((IStructuralEquatable)collectionA).Equals(enumerableB, (IEqualityComparer)comparer.Object);

      areEqual.Should().BeTrue();

      comparer.Verify(c => c.Equals(It.IsAny<byte>(), It.IsAny<byte>()), Times.Exactly(3));
    }

    [Fact]
    public void WhenComparerIsOfT_ButOtherIsNotEnumerableOfT_ShouldFallbackToStructuralEquatable()
    {
      RecordCollection<uint> collectionA = [1, 2, 3];
      RecordCollection<int> collectionB = [1, 2, 3];
      var comparer = MakeComparerOf<uint>(null, null);
      comparer.Setup(c => c.GetHashCode(It.IsAny<uint>())).Returns((uint i) => (int)i);
      comparer.As<IEqualityComparer>().Setup(c => c.GetHashCode(It.IsIn(1, 2, 3))).Returns((int i) => i);

      var areEqual = ((IStructuralEquatable)collectionA).Equals(collectionB, (IEqualityComparer)comparer.Object);

      areEqual.Should().BeTrue();

      comparer.Verify(c => c.GetHashCode(It.IsAny<uint>()), Times.Exactly(3));
      comparer.As<IEqualityComparer>().Verify(c => c.GetHashCode(It.IsAny<object>()), Times.Exactly(3));
    }

    [Fact]
    public void WhenComparerIsOfT_ButOtherIsNotEnumerableOfT_ShouldFallbackToStructuralEquatable_AndFailForMismatch()
    {
      RecordCollection<uint> collectionA = [1, 2, 3];
      RecordCollection<int> collectionB = [1, 22, 3];
      var comparer = MakeComparerOf<uint>(null, null);
      comparer.Setup(c => c.GetHashCode(It.IsAny<uint>())).Returns((uint i) => (int)i);
      comparer.As<IEqualityComparer>().Setup(c => c.GetHashCode(It.IsIn(1, 22, 3))).Returns((int i) => i);

      var areEqual = ((IStructuralEquatable)collectionA).Equals(collectionB, (IEqualityComparer)comparer.Object);

      areEqual.Should().BeFalse();

      comparer.Verify(c => c.GetHashCode(It.IsAny<uint>()), Times.Exactly(3));
      comparer.As<IEqualityComparer>().Verify(c => c.GetHashCode(It.IsAny<object>()), Times.Exactly(3));
    }

    [Fact]
    public void WhenOtherIsEnumerableOfOther_UseComparerEquals()
    {
      RecordCollection<uint> collectionA = [1, 2, 3];
      IEnumerable<double> enumerableB = Enumerable.Range(1, 3).Select(x => x + 0.4);
      IntegralComparer comparer = new();

      var areEqual = ((IStructuralEquatable)collectionA).Equals(enumerableB, comparer);

      areEqual.Should().BeTrue();
    }

    [Theory]
    [InlineData(3, 4)]
    [InlineData(4, 3)]
    public void WhenOtherIsEnumerableOfOther_UseComparerEquals_AndFailForMismatchedLength(int len1, int len2)
    {
      RecordCollection<int> collectionA = Enumerable.Range(1, len1).ToRecordCollection();
      IEnumerable<long> enumerableB = Enumerable.Range(1, len2).Select(x => (long)x);
      var comparerMock = MakeAlwaysTrueComparer();

      var areEqual = ((IStructuralEquatable)collectionA).Equals(enumerableB, comparerMock.Object);

      areEqual.Should().BeFalse();
      comparerMock.Verify(m => m.Equals(It.IsAny<int>(), It.IsAny<long>()), Times.Exactly(3));
      comparerMock.Verify(m => m.GetHashCode(It.IsAny<object>()), Times.Never);
    }

    [Fact]
    public void WhenOtherIsEnumerableOfOther_UseComparerEquals_AndFailForMismatchedValues()
    {
      RecordCollection<int> collectionA = [5, 6, 7];
      IEnumerable<long> enumerableB = Enumerable.Range(1, 3).Select(x => (long)x);
      IntegralComparer comparer = new();

      var areEqual = ((IStructuralEquatable)collectionA).Equals(enumerableB, comparer);

      areEqual.Should().BeFalse();
    }

    [Fact]
    public void FailForNonEnumerable()
    {
      RecordCollection<int> collectionA = [1, 2, 3];
      object otherThing = new { unique = true };
      Mock<IEqualityComparer> comparer = new(MockBehavior.Strict);

      var areEqual = ((IStructuralEquatable)collectionA).Equals(otherThing, comparer.Object);

      areEqual.Should().BeFalse();
    }

    [Fact]
    public void ShortCircuitForDifferentSizedCollections()
    {
      RecordCollection<int> collectionA = [1, 2, 3];

      var maxCollectionMoq = new Mock<IReadOnlyCollection<int>>(MockBehavior.Strict);
      maxCollectionMoq.SetupGet(x => x.Count).Returns(int.MaxValue);
      maxCollectionMoq.Setup(x => x.GetEnumerator()).Throws(() => FailException.ForFailure("called GetEnumerator"));

      var comparerMock = MakeAlwaysTrueComparer();

      var areEqual = ((IStructuralEquatable)collectionA).Equals(maxCollectionMoq.Object, comparerMock.Object);

      areEqual.Should().BeFalse();
    }


    private Mock<IEqualityComparer> MakeComparer(IEqualityComparer? basedOn)
    {
      var mock = new Mock<IEqualityComparer>(MockBehavior.Strict);

      if (basedOn is not null)
      {
        mock
          .Setup(c => c.Equals(It.IsAny<object>(), It.IsAny<object>()))
          .Callback((object a, object b) => basedOn.Equals(a, b));

        mock
          .Setup(c => c.GetHashCode(It.IsAny<object>()))
          .Callback((object a) => basedOn.GetHashCode(a));
      }

      return mock;
    }

    private Mock<IEqualityComparer<T>> MakeComparerOf<T>(IEqualityComparer<T>? basedOnOfT, IEqualityComparer? basedOn)
    {
      var mock = MakeComparer(basedOn);
      var comparerOfT = mock.As<IEqualityComparer<T>>();

      if (basedOnOfT is not null)
      {
        comparerOfT
          .Setup(c => c.Equals(It.IsAny<T>(), It.IsAny<T>()))
          .Callback((T? a, T? b) => basedOnOfT.Equals(a, b));

        comparerOfT
          .Setup(c => c.GetHashCode(It.IsAny<T>()!))
          .Callback((T a) => basedOnOfT.GetHashCode(a));
      }

      return comparerOfT;
    }

    private Mock<IEqualityComparer> MakeAlwaysTrueComparer()
    {
      var mock = new Mock<IEqualityComparer>(MockBehavior.Strict);
      mock.Setup(x => x.Equals(It.IsAny<object>(), It.IsAny<object>())).Returns(true);
      mock.Setup(x => x.GetHashCode(It.IsAny<object>())).Returns(1);
      return mock;
    }

    private class IntegralComparer : IEqualityComparer
    {
      bool IEqualityComparer.Equals(object? x, object? y) =>
        (x, y) switch
        {
          (null, null) => true,
          (not null, not null) => Convert.ToInt64(x) == Convert.ToInt64(y),
          _ => false,
        };

      public int GetHashCode(object obj) => Convert.ToInt64(obj).GetHashCode();
    }
  }
}
