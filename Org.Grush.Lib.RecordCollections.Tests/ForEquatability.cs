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
    var areEqEq = collectionA == collectionB;
    var arentEqEq = collectionA != collectionB;

    // Assert
    areEqual.Should().BeTrue();
    areEqEq.Should().BeTrue();
    arentEqEq.Should().BeFalse();
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

  [Fact]
  public static void EqualsFailsForWrongType()
  {
    RecordCollection<int> collectionA = [1, 2, 3];
    RecordCollection<long> collectionB = [1, 2, 3];

    // Act
    // ReSharper disable once SuspiciousTypeConversion.Global
    var areEqual = collectionA.Equals(collectionB);

    // Assert
    areEqual.Should().BeFalse();
  }

  private class RoundingComparer : IEqualityComparer<double>
  {
    public bool Equals(double x, double y)
      => Math.Round(x) == Math.Round(y);

    public int GetHashCode(double obj)
      => (int)Math.Round(obj);
  }

  [Fact]
  public static void EqualsWithComparer()
  {
    RecordCollection<double> collectionA = [1.1, 2.2, 3.3];
    RecordCollection<double> collectionB = [0.9, 1.9, 2.9];

    var areEqual = collectionA.Equals(collectionB, new RoundingComparer());

    areEqual.Should().BeTrue();
  }

  public static class ViaStaticMethod
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
    public void WhenToStructuralEquatable_ButNotEnumerable()
    {
      RecordCollection<uint> collectionA = [1, 2, 3];

      var comparer = MakeComparerOf<uint>(null, null);
      comparer.Setup(c => c.GetHashCode(It.IsIn(1u, 2u, 3u))).Returns((uint i) => (int)i);

      int hashOfCollectionA = ((IStructuralEquatable)collectionA).GetHashCode((IEqualityComparer)comparer.Object);
      comparer.Reset();
      comparer.Setup(c => c.GetHashCode(It.IsIn(1u, 2u, 3u))).Returns((uint i) => (int)i);

      var otherMock = new Mock<IStructuralEquatable>(MockBehavior.Strict);
      otherMock.Setup(m => m.GetHashCode((IEqualityComparer)comparer.Object)).Returns(hashOfCollectionA);

      var areEqual = ((IStructuralEquatable)collectionA).Equals(otherMock.Object, (IEqualityComparer)comparer.Object);

      areEqual.Should().BeTrue();

      comparer.Verify(c => c.GetHashCode(It.IsAny<uint>()), Times.Exactly(3));
      otherMock.VerifyAll();
    }

    [Fact]
    public void WhenComparerIsOfT_ButOtherIsNotEnumerableOfT_ShouldFallbackToEnumerable_AndFailLazily()
    {
      RecordCollection<uint> collectionA = [1, 2, 3];
      RecordCollection<int> collectionB = [1, 22, 3];
      var comparerMock = MakeComparer(null);
      comparerMock.Setup(c => c.Equals(1u, 1)).Returns(true);
      comparerMock.Setup(c => c.Equals(2u, 22)).Returns(false);
      // lazily doesn't evaluate third item

      var areEqual = ((IStructuralEquatable)collectionA).Equals(collectionB, comparerMock.Object);

      areEqual.Should().BeFalse();

      comparerMock.Verify(c => c.Equals(It.IsAny<object>(), It.IsAny<object>()), Times.Exactly(2));
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

      var maxCollectionMoq = new Mock<ICollection>(MockBehavior.Strict);
      maxCollectionMoq.SetupGet(x => x.Count).Returns(int.MaxValue);
      maxCollectionMoq.Setup(x => x.GetEnumerator()).Throws(() => FailException.ForFailure("called GetEnumerator"));

      var comparerMock = MakeAlwaysTrueComparer();

      var areEqual = ((IStructuralEquatable)collectionA).Equals(maxCollectionMoq.Object, comparerMock.Object);

      areEqual.Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_ForComparerNotOfT_ForValueType()
    {
      RecordCollection<int?> collectionA = [1, 2, 3, null];

      var expectedHash = HashCode.Combine(1, 2, 3, (int?)null);

      var comparerMock = MakeComparer();
      comparerMock.Setup(c => c.GetHashCode(It.IsIn(1, 2, 3))).Returns((int v) => v.GetHashCode());
      comparerMock.Setup(c => c.GetHashCode(null!)).Returns(0);

      var actualHashCode = ((IStructuralEquatable)collectionA).GetHashCode(comparerMock.Object);

      actualHashCode.Should().Be(expectedHash);
      comparerMock.VerifyAll();
    }

    [Fact]
    public static void GetHashCode_ForComparerNotOfT_ForReferenceType()
    {
      RecordCollection<string?> collectionA = ["a", "b", "c", null];

      var expectedHash = HashCode.Combine("a", "b", "c", 0);

      var comparerMock = MakeComparer();
      comparerMock.Setup(c => c.GetHashCode(It.IsIn("a", "b", "c"))).Returns((string v) => v.GetHashCode());

      var actualHashCode = ((IStructuralEquatable)collectionA).GetHashCode(comparerMock.Object);

      actualHashCode.Should().Be(expectedHash);
      comparerMock.VerifyAll();
    }

    private static Mock<IEqualityComparer> MakeComparer()
    {
      var mock = new Mock<IEqualityComparer>(MockBehavior.Strict);

      return mock;
    }

    private static Mock<IEqualityComparer<T>> MakeComparerOf<T>()
    {
      var mock = MakeComparer();
      var comparerOfT = mock.As<IEqualityComparer<T>>();

      return comparerOfT;
    }

    private static Mock<IEqualityComparer> MakeAlwaysTrueComparer()
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
