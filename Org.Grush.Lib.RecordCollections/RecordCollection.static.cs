using System.Collections.Immutable;

namespace Org.Grush.Lib.RecordCollections;

public static class RecordCollection
{
  /// <summary>Alternative to <see cref="RecordCollection{T}.Empty"/>.</summary>
  public static RecordCollection<T> Create<T>()
    => RecordCollection<T>.Empty;

  /// <summary>
  /// Public <see cref="RecordCollection{T}"/> initializer from <see cref="ReadOnlySpan{T}"/>,
  /// used for collection expression syntax.
  /// </summary>
  public static RecordCollection<T> Create<T>(ReadOnlySpan<T> items)
    => ImmutableArray.Create(items);

  /// <inheritdoc cref="ToRecordCollection{T}(System.Collections.Generic.IEnumerable{T})"/>
  public static RecordCollection<T> ToRecordCollection<T>(this ReadOnlySpan<T> items)
    => [..items];

  /// <summary>Produces a record collection from the specified elements, as a LINQ-like extensions.</summary>
  public static RecordCollection<T> ToRecordCollection<T>(this IEnumerable<T> enumerable)
    => CreateRange(enumerable);


  /// <summary>Creates a new <see cref="RecordCollection{T}"/> from the specified elements.</summary>
  public static RecordCollection<T> CreateRange<T>(IEnumerable<T> enumerable)
  {
    if (enumerable is RecordCollection<T> c)
      return c;

    return ImmutableArray.CreateRange(enumerable);
  }

  public static bool Equals<T>(RecordCollection<T>? lhs, RecordCollection<T>? rhs) =>
    (lhs, rhs) switch
    {
      (null, null) => true,
      (not null, not null) => lhs.Equals(rhs),
      _ => false,
    };
}
