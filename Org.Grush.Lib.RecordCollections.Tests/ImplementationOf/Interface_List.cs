using System.Collections;
using FluentAssertions;

namespace Org.Grush.Lib.RecordCollections.Tests.ImplementationOf;

public static class Interface_List
{
  public static class Generic
  {
    record SomeRecord(int Int, string String);

    [Fact]
    public static void IndexOf()
    {
      RecordCollection<SomeRecord> collection =
      [
        new(1, "A"),
        new(2, "B"),
        new(2, "B"),
        new(2, "b"),
      ];

      var foundIndex = collection.IndexOf(new SomeRecord(2, "B"));

      foundIndex.Should().Be(1);
    }

    [Fact]
    public static void Contains()
    {
      RecordCollection<SomeRecord> collection =
      [
        new(1, "A"),
        new(2, "B"),
        new(2, "B"),
        new(2, "b"),
      ];

      var foundIndex = collection.Contains(new SomeRecord(2, "B"));

      foundIndex.Should().BeTrue();
    }

    [Fact]
    public static void CopyTo_Span()
    {
      RecordCollection<string> collection = ["a", "b", "c"];
      var targetSpan = new Span<string>(new string[3]);

      collection.CopyTo(targetSpan);

      targetSpan[0].Should().Be("a");
      targetSpan[1].Should().Be("b");
      targetSpan[2].Should().Be("c");
      targetSpan.Length.Should().Be(3);
    }
  }

  public static class NonGeneric
  {
    static readonly RecordCollection<string> collection = ["a", "b", "c"];
    static IList CollectionAsIList => collection;

    [Fact]
    public static void GetByIndex()
    {
      object? item1 = CollectionAsIList[1];

      item1.Should().Be("b");
    }

    [Fact]
    public static void Contains()
    {
      var contains = CollectionAsIList.Contains("b");

      contains.Should().BeTrue();
    }

    [Fact]
    public static void Contains_FalseWhenMissing()
    {
      var contains = CollectionAsIList.Contains("Z");

      contains.Should().BeFalse();
    }

    [Fact]
    public static void Contains_FalseForWrongType()
    {
      var contains = CollectionAsIList.Contains(CollectionAsIList);

      contains.Should().BeFalse();
    }

    [Fact]
    public static void IndexOf()
    {
      int foundIndex = CollectionAsIList.IndexOf("b");

      foundIndex.Should().Be(1);
    }

    [Fact]
    public static void IndexOf_Negative1WhenMissing()
    {
      int foundIndex = CollectionAsIList.IndexOf("Z");

      foundIndex.Should().Be(-1);
    }

    [Fact]
    public static void IndexOf_Negative1WhenMissingNull()
    {
      int foundIndex = CollectionAsIList.IndexOf(null);

      foundIndex.Should().Be(-1);
    }

    [Fact]
    public static void IndexOf_Negative1ForWrongType()
    {
      int foundIndex = CollectionAsIList.IndexOf(CollectionAsIList);

      foundIndex.Should().Be(-1);
    }

    [Fact]
    public static void ConstantProperties()
    {
      CollectionAsIList.IsFixedSize.Should().BeTrue();
      CollectionAsIList.IsReadOnly.Should().BeTrue();
      CollectionAsIList.IsSynchronized.Should().BeTrue();

      var getSyncRoot = () => CollectionAsIList.SyncRoot;

      getSyncRoot.Should().ThrowExactly<NotSupportedException>();
    }
  }
}
