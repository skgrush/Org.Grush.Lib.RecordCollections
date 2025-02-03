using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FluentAssertions;
using FluentAssertions.Execution;

namespace Org.Grush.Lib.RecordCollections.Tests;

[SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters")]
public class ForImmutability
{
  [Fact]
  public void AndAlwaysReadOnly()
  {
    using var _ = new AssertionScope();

    var collection = RecordCollection.Create([1, 2]);

    (collection as ICollection<int>)
      .IsReadOnly
      .Should()
      .BeTrue();
  }

  [Fact]
  public void ItemRefIsReadOnly()
  {
    var collection = RecordCollection.Create([
      (Key: 1, Prop: "A"),
      (Key: 2, Prop: "B"),
    ]);

    collection[0].Should().NotBeSameAs(collection[0]);

    unsafe
    {
      fixed ((int Key, string Prop)* pa = &collection.ItemRef(0), pb = &collection.ItemRef(0))
      {
        (pa == pb).Should().BeTrue();
      }
    }
  }

  [Fact]
  public void AsSpanAndAsMemoryAreSame()
  {
    RecordCollection<string> collection = ["a", "b"];

    var span = collection.AsSpan();
    var mem = collection.AsMemory();

    (mem.Span == span).Should().BeTrue();
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
    [Obsolete("Allow testing obsolete methods")]
    public UnsupportedMethods()
    {
      RecordCollection<long> collection = [];

      var functions = new MulticastDelegate[]
      {
        ((ICollection<long>)collection).Add,
        ((ICollection<long>)collection).Clear,
        ((ICollection<long>)collection).Remove,
        ((IList<long>)collection).Insert,
        ((IList<long>)collection).RemoveAt,
        ((IList)collection).Clear,
        ((IList)collection).RemoveAt,
        ((IList)collection).Add,
        ((ICollection)collection).CopyTo,
        ((IList)collection).Insert,
        ((IList)collection).Remove,
      };

      foreach (var fn in functions)
      {
        Add(
          fn.Method.Name,
          GetDefaultCallOfMethod(collection, fn.Method)
        );
      }

      Add("RecordCollection<T>[0] =", (() => collection[0] = 0));
      Add("((IList)RecordCollection<T>)[0] =", () => ((IList)collection)[0] = 0);
    }
  }

  private static Action GetDefaultCallOfMethod<T>(T instance, MethodInfo method)
  {
    var theParams = method.GetParameters().Select(p =>
      p.ParameterType.IsValueType
        ? Activator.CreateInstance(p.ParameterType)
        : null
    ).ToArray();

    return () => { method.Invoke(instance, BindingFlags.Public | BindingFlags.Instance, null, theParams, null); };
  }
}
