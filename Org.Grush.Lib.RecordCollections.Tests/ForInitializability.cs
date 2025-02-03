using FluentAssertions;

namespace Org.Grush.Lib.RecordCollections.Tests;

public static class ForInitializability
{
  [Fact]
  public static void UsingEmptyCreate()
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
  public static void UsingStaticCreateMethod()
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
  public static void UsingCollectionExpression()
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
  public static void UsingEnumerableExtension()
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
  public static void UsingEnumerableExtension_ForReadOnlySpan()
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
  public static void UsingCreateRange()
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
