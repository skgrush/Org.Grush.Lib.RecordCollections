using System.Collections.Immutable;

namespace Org.Grush.Lib.RecordCollections;

public static class RecordCollectionExtensions
{
  public static bool SequenceEqual<TDerived, TBase>(this RecordCollection<TBase> source, RecordCollection<TDerived> other, IEqualityComparer<TBase>? comparer = null)
    where TDerived : TBase
    => ((ImmutableArray<TBase>)source).SequenceEqual((ImmutableArray<TDerived>)other, comparer);

  public static bool SequenceEqual<TDerived, TBase>(this RecordCollection<TBase> source, IEnumerable<TDerived> other, IEqualityComparer<TBase>? comparer = null)
    where TDerived : TBase
    => ((ImmutableArray<TBase>)source).SequenceEqual(other, comparer);

  public static bool SequenceEqual<TDerived, TBase>(this RecordCollection<TBase> source, RecordCollection<TDerived> other, Func<TBase, TBase, bool> predicate)
    where TDerived : TBase
    => ((ImmutableArray<TBase>)source).SequenceEqual((ImmutableArray<TDerived>)other, predicate);

  public static IEnumerable<TResult> Select<T, TResult>(this RecordCollection<T> source, Func<T, TResult> selector)
    => ((ImmutableArray<T>)source).Select(selector);

  public static IEnumerable<T> Where<T>(this RecordCollection<T> source, Func<T, bool> predicate)
    => ((ImmutableArray<T>)source).Where(predicate);

  public static Dictionary<TKey, T> ToDictionary<TKey, T>(
    this RecordCollection<T> source,
    Func<T, TKey> keySelector,
    IEqualityComparer<TKey>? comparer = null
  ) where TKey : notnull
  {
    return ((ImmutableArray<T>)source).ToDictionary(keySelector, t => t, comparer);
  }

  public static Dictionary<TKey, TElement> ToDictionary<TKey, TElement, T>(
    this RecordCollection<T> source,
    Func<T, TKey> keySelector,
    Func<T, TElement> elementSelector,
    IEqualityComparer<TKey>? comparer = null
    ) where TKey : notnull
  {
    return ((ImmutableArray<T>)source).ToDictionary(keySelector, elementSelector, comparer);
  }

  public static T[] ToArray<T>(this RecordCollection<T> source)
    => ((ImmutableArray<T>)source).ToArray();
}
