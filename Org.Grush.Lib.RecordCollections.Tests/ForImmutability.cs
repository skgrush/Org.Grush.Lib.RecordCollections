using System.Reflection;
using FluentAssertions;
using FluentAssertions.Execution;

namespace Org.Grush.Lib.RecordCollections.Tests;

public class ForImmutability
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

      var functions = new MulticastDelegate[]
      {
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

#pragma warning disable CS0618 // Type or member is obsolete
      Add("RecordCollection<T>[0] =", (() => collection[0] = 0));
#pragma warning restore CS0618 // Type or member is obsolete
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
