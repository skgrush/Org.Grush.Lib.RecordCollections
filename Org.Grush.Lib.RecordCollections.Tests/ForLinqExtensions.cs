using FluentAssertions;

namespace Org.Grush.Lib.RecordCollections.Tests;

public static class ForLinqExtensions
{
  private record class Super(int A, int B);
  private record class Sub(int A, int B, int C, int D) : Super(A, B);

  private class SuperEqualityComparer : IEqualityComparer<Super>
  {
    public bool Equals(Super? x, Super? y)
    {
      return
        x?.A == y?.A &&
        x?.B == y?.B;

    }

    public int GetHashCode(Super obj) => obj.GetHashCode();
  }

  [Fact]
  public static void SequenceEqual_CompareBaseToDerived_WithComparer()
  {
    RecordCollection<Super> super = [new(1, 10), new(2, 20)];
    RecordCollection<Sub> sub = [new(1, 10, 100, 1000), new(2, 20, 200, 2000)];

    var eq = super.SequenceEqual(sub, new SuperEqualityComparer());

    eq.Should().BeTrue();
  }

  [Fact]
  public static void SequenceEqual_CompareBaseToDerivedEnumerable_WithComparer()
  {
    RecordCollection<Super> super = [new(1, 10), new(2, 20)];
    IEnumerable<Sub> sub = Enumerable.Range(1, 2).Select(i => new Sub(i, i * 10, i* 100, i* 1000));

    var eq = super.SequenceEqual(sub, new SuperEqualityComparer());

    eq.Should().BeTrue();
  }

  [Fact]
  public static void SequenceEqual_CompareBaseToDerived_WithFunction()
  {
    RecordCollection<Super> super = [new(1, 10), new(2, 20)];
    RecordCollection<Sub> sub = [new(1, 10, 100, 1000), new(2, 20, 200, 2000)];

    var eq = super.SequenceEqual(sub, (x, y) => x.A == y.A && x.B == y.B);

    eq.Should().BeTrue();
  }

  [Fact]
  public static void Select_ConfirmingLazy()
  {
    RecordCollection<int> ints = [1, 2, 3, 4];

    int iterations = 0;
    var enumerable = ints.Select(x =>
    {
      ++iterations;
      return (long)x;
    });

    iterations.Should().Be(0);

    enumerable
      .Should()
      .BeEquivalentTo([1L, 2L, 3L, 4L]);
  }

  [Fact]
  public static void Where_ConfirmingLazy()
  {
    RecordCollection<int> ints = [1, 2, 3, 4];

    int iterations = 0;
    var enumerable = ints.Where(x =>
    {
      ++iterations;
      return (x % 2) == 0;
    });

    iterations.Should().Be(0);

    enumerable
      .Should()
      .BeEquivalentTo([2, 4]);
  }

  [Fact]
  public static void ToDictionary_NoElementSelector()
  {
    var records = RecordCollection.Create([
      new { key = 1 },
      new { key = 2 },
      new { key = 3 },
    ]);

    var dict = records.ToDictionary(obj => obj.key);

    dict.Keys.Should().BeEquivalentTo([1, 2, 3]);
  }

  [Fact]
  public static void ToDictionary()
  {
    var records = RecordCollection.Create([
      new { key = 1, V = "A" },
      new { key = 2, V = "B" },
      new { key = 3, V = "C" },
    ]);

    var dict = records.ToDictionary(obj => obj.key, obj => obj.V);

    dict
      .Should()
      .BeEquivalentTo(
        new Dictionary<int, string>
        {
          { 1, "A" },
          { 2, "B" },
          { 3, "C" },
        }
      );
  }

  [Fact]
  public static void ToArray()
  {
    RecordCollection<int> ints = [1, 2, 3, 4];

    var array = ints.ToArray();

    array
      .Should()
      .BeOfType<int[]>()
      .And
      .BeEquivalentTo(ints);
  }
}
