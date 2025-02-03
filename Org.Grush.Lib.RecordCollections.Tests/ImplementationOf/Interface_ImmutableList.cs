using FluentAssertions;
using System.Collections.Immutable;

namespace Org.Grush.Lib.RecordCollections.Tests.ImplementationOf;

/// <summary>
/// Yes these methods are mostly actually testing the <see cref="ImmutableArray{T}"/>
/// struct but the intent is to make sure we're correctly interacting with that struct.
/// </summary>
public static class Interface_ImmutableList
{
  public static class ImplicitImplementations
  {
    private class StartsWithComparer : IEqualityComparer<string>
    {
      public bool Equals(string? ofStr, string? first)
      {
        if (first is null || ofStr is null)
          return false;

        return ofStr.StartsWith(first);
      }

      public int GetHashCode(string obj) => throw new NotImplementedException();
    }

    [Fact]
    public static void IndexOf()
    {
      RecordCollection<string> collection = ["a", "b", "c", "a", "b", "c"];

      var foundIndex = collection.IndexOf("c");

      foundIndex.Should().Be(2);
    }

    [Fact]
    public static void IndexOf_IndexCount()
    {
      RecordCollection<string> collection = ["a", "b", "c", "a", "b", "c"];

      var foundIndex = collection.IndexOf(item: "a", index: 2, count: 4);

      foundIndex.Should().Be(3);
    }

    [Fact]
    public static void IndexOf_WithComparer()
    {
      RecordCollection<string> collection = ["Ayo", "Bye", "Cool", "And", "Bots", "Craze"];

      var foundIndex = collection.IndexOf(item: "A", index: 2, count: 4, new StartsWithComparer());

      foundIndex.Should().Be(3);
    }

    [Fact]
    public static void IndexOf_Missing()
    {
      RecordCollection<string> collection = ["a", "b", "c", "a", "b", "c"];

      var foundIndex = collection.IndexOf(item: "a", index: 1, count: 2);

      foundIndex.Should().Be(-1);
    }

    [Fact]
    public static void LastIndexOf()
    {
      RecordCollection<string> collection = ["a", "b", "c", "a", "b", "c"];

      var foundIndex = collection.LastIndexOf("a");

      foundIndex.Should().Be(3);
    }

    [Fact]
    public static void LastIndexOf_IndexCount()
    {
      RecordCollection<string> collection = ["a", "b", "c", "a", "b", "c", "a", "b", "c"];

      var foundIndex = collection.LastIndexOf(item: "a", index: 5, count: 5);

      foundIndex.Should().Be(3);
    }

    [Fact]
    public static void LastIndexOf_WithComparer()
    {
      RecordCollection<string> collection = ["Ayo", "Bye", "Cool", "And", "Bots", "Craze", "Are", "Bits"];

      var foundIndex = collection.IndexOf(item: "A", index: 1, count: 5, new StartsWithComparer());

      foundIndex.Should().Be(3);
    }

    [Fact]
    public static void LastIndexOf_Missing()
    {
      RecordCollection<string> collection = ["a", "b", "c", "a", "b", "c"];

      var foundIndex = collection.IndexOf(item: "a", index: 1, count: 2);

      foundIndex.Should().Be(-1);
    }
  }

  public static class ExplicitImplementations
  {
    private const int InitialLength = 6;
    private static readonly RecordCollection<int> Initial = [1, 2, 3, 4, 5, 6];
    private static IImmutableList<int> InitialAsList => Initial;

    [Fact]
    public static void Add()
    {
      var initialAsList = InitialAsList;

      var resultAsList = initialAsList.Add(10);
      var result = Initial.Add(10);

      result.Should().Equal(resultAsList);

      initialAsList.Count.Should().Be(InitialLength);
      resultAsList
        .Should()
        .BeOfType<RecordCollection<int>>()
        .And
        .BeEquivalentTo([1, 2, 3, 4, 5, 6, 10]);
    }

    [Fact]
    public static void AddRange()
    {
      var initialAsList = InitialAsList;

      var resultAsList = initialAsList.AddRange([10, 11, 12]);
      var result = Initial.AddRange([10, 11, 12]);

      result.Should().Equal(resultAsList);

      initialAsList.Count.Should().Be(InitialLength);
      resultAsList
        .Should()
        .BeOfType<RecordCollection<int>>()
        .And
        .BeEquivalentTo([1, 2, 3, 4, 5, 6, 10, 11, 12]);
    }

    [Fact]
    public static void Clear()
    {
      var initialAsList = InitialAsList;

      var resultAsList = initialAsList.Clear();
      var result = Initial.Clear();

      result.Should().Equal(resultAsList);

      initialAsList.Count.Should().Be(InitialLength);
      resultAsList
        .Should()
        .BeOfType<RecordCollection<int>>()
        .And
        .BeEmpty();
    }

    [Fact]
    public static void Insert()
    {
      var initialAsList = InitialAsList;

      var resultAsList = initialAsList.Insert(3, 10);
      var result = Initial.Insert(3, 10);

      result.Should().Equal(resultAsList);

      initialAsList.Count.Should().Be(InitialLength);
      resultAsList
        .Should()
        .BeOfType<RecordCollection<int>>()
        .And
        .BeEquivalentTo([1, 2, 3, 10, 4, 5, 6]);
    }

    [Fact]
    public static void InsertRange()
    {
      var initialAsList = InitialAsList;

      var resultAsList = initialAsList.InsertRange(3, [10, 11, 12]);
      var result = Initial.InsertRange(3, [10, 11, 12]);

      result.Should().Equal(resultAsList);

      initialAsList.Count.Should().Be(InitialLength);
      resultAsList
        .Should()
        .BeOfType<RecordCollection<int>>()
        .And
        .BeEquivalentTo([1, 2, 3, 10, 11, 12, 4, 5, 6]);
    }

    /// <summary>"Matches" any time `x` parameter is even.</summary>
    private class EvenEqualityComparer : IEqualityComparer<int>
    {
      public bool Equals(int x, int y)
      {
        return x % 2 is 0;
      }

      public int GetHashCode(int obj)
        => throw new NotImplementedException();
    }

    [Fact]
    public static void Remove_WithComparer()
    {
      RecordCollection<int> duplicatingCollection = [1, 2, 3, 4, 5, 6];
      IImmutableList<int> duplicatingCollectionAsList = duplicatingCollection;

      var resultAsList = duplicatingCollectionAsList.Remove(999, new EvenEqualityComparer());

      duplicatingCollection.Count.Should().Be(6);
      resultAsList
        .Should()
        .BeOfType<RecordCollection<int>>()
        .And
        .BeEquivalentTo([1, /* 2 removed */3, 4, 5, 6]);
    }

    [Fact]
    public static void Remove_WithoutComparer()
    {
      RecordCollection<int> duplicatingCollection = [1, 2, 2, 2, 2, 3];
      IImmutableList<int> duplicatingCollectionAsList = duplicatingCollection;

      var resultAsList = duplicatingCollectionAsList.Remove(2, null);

      duplicatingCollection.Count.Should().Be(6);
      resultAsList
        .Should()
        .BeOfType<RecordCollection<int>>()
        .And
        .BeEquivalentTo([1, /* single removed */2, 2, 2, 3]);
    }

    [Fact]
    public static void RemoveAll()
    {
      var initialAsList = InitialAsList;

      var resultAsList = initialAsList.RemoveAll(i => i % 2 is 0);
      var result = Initial.RemoveAll(i => i % 2 is 0);

      result.Should().Equal(resultAsList);

      initialAsList.Count.Should().Be(InitialLength);
      resultAsList
        .Should()
        .BeOfType<RecordCollection<int>>()
        .And
        .BeEquivalentTo([1, 3, 5,]);
    }

    [Fact]
    public static void RemoveAt()
    {
      var initialAsList = InitialAsList;

      var resultAsList = initialAsList.RemoveAt(3);
      var result = Initial.RemoveAt(3);

      result.Should().Equal(resultAsList);

      initialAsList.Count.Should().Be(InitialLength);
      resultAsList
        .Should()
        .BeOfType<RecordCollection<int>>()
        .And
        .BeEquivalentTo([1, 2, 3, /* removed */ 5, 6]);
    }

    [Fact]
    public static void RemoveRange_WithoutComparer()
    {
      var initialAsList = InitialAsList;

      var resultAsList = initialAsList.RemoveRange([3, 4, 5], null);
      var result = Initial.RemoveRange([3, 4, 5]);

      result.Should().Equal(resultAsList);

      initialAsList.Count.Should().Be(InitialLength);
      resultAsList
        .Should()
        .BeOfType<RecordCollection<int>>()
        .And
        .BeEquivalentTo([1, 2, /* removed */6]);
    }

    [Fact]
    public static void RemoveRange_WithComparer()
    {
      var initialAsList = InitialAsList;

      var resultAsList = initialAsList.RemoveRange([999, 999], new EvenEqualityComparer());
      var result = Initial.RemoveRange([999, 999], new EvenEqualityComparer());

      result.Should().Equal(resultAsList);

      initialAsList.Count.Should().Be(InitialLength);
      resultAsList
        .Should()
        .BeOfType<RecordCollection<int>>()
        .And
        .BeEquivalentTo([1, /* removed */3, /* removed */5, 6]);
    }

    [Fact]
    public static void RemoveRange_IndexCount()
    {
      var initialAsList = InitialAsList;

      var resultAsList = initialAsList.RemoveRange(2, 3);
      var result = Initial.RemoveRange(2, 3);

      result.Should().Equal(resultAsList);

      initialAsList.Count.Should().Be(InitialLength);
      resultAsList
        .Should()
        .BeOfType<RecordCollection<int>>()
        .And
        .BeEquivalentTo([1, 2, /* removed */6]);
    }

    [Fact]
    public static void Replace_WithComparer()
    {
      var initialAsList = InitialAsList;

      var resultAsList = initialAsList.Replace(999, 10, new EvenEqualityComparer());
      var result = Initial.Replace(999, 10, new EvenEqualityComparer());

      result.Should().Equal(resultAsList);

      initialAsList.Count.Should().Be(InitialLength);
      resultAsList
        .Should()
        .BeOfType<RecordCollection<int>>()
        .And
        .BeEquivalentTo([1, 10, 3, 4, 5, 6]);
    }

    [Fact]
    public static void Replace_WithoutComparer()
    {
      var initialAsList = InitialAsList;

      var resultAsList = initialAsList.Replace(3, 10, null);
      var result = Initial.Replace(3, 10);

      result.Should().Equal(resultAsList);

      initialAsList.Count.Should().Be(InitialLength);
      resultAsList
        .Should()
        .BeOfType<RecordCollection<int>>()
        .And
        .BeEquivalentTo([1, 2, 10, 4, 5, 6]);
    }

    [Fact]
    public static void SetItem()
    {
      var initialAsList = InitialAsList;

      var resultAsList = initialAsList.SetItem(3, 10);
      var result = Initial.SetItem(3, 10);

      result.Should().Equal(resultAsList);

      initialAsList.Count.Should().Be(InitialLength);
      initialAsList.Count.Should().Be(InitialLength);
      resultAsList
        .Should()
        .BeOfType<RecordCollection<int>>()
        .And
        .BeEquivalentTo([1, 2, 3, 10, 5, 6]);
    }
  }
}
