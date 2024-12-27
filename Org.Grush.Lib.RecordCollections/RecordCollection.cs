using System.Collections;
using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace Org.Grush.Lib.RecordCollections {

public static class RecordCollection
{
  public static RecordCollection<T> Create<T>() => RecordCollection<T>.Empty;

  public static RecordCollection<T> Create<T>(ReadOnlySpan<T> items)
    => RecordCollection<T>.Empty.AddRange(items);

  public static RecordCollection<T> ToRecordCollection<T>(this IEnumerable<T> enumerable)
    => CreateRange(enumerable);

  public static RecordCollection<T> CreateRange<T>(IEnumerable<T> enumerable)
  {
    if (enumerable is RecordCollection<T> c)
      return c;

    return RecordCollection<T>.Empty.AddRange(enumerable);
  }
}

#if NETSTANDARD
internal static class IEnumerableExtensionsForNetStandard
{
  public static bool TryGetNonEnumeratedCount<T>(this IEnumerable<T> enumerable, out int count)
  {
    if (enumerable is ICollection<T> coll)
    {
      count = coll.Count;
      return true;
    }

    count = -1;
    return false;
  }
}
#endif

#if NET8_0_OR_GREATER
[System.Runtime.CompilerServices.CollectionBuilder(typeof (RecordCollection), "Create")]
#endif
[JsonConverter(typeof(RecordCollectionJsonConverterFactory))]
public sealed class RecordCollection<T> : IImmutableList<T>, IEquatable<IImmutableList<T>>
{
  public static readonly RecordCollection<T> Empty = new([]);

  private readonly T[] _list;

  internal RecordCollection(IEnumerable<T> list) => _list = list.ToArray();
  private RecordCollection(T[] array) => _list = array;

  public bool IsEmpty => _list.Length is 0;

  internal RecordCollection<T> AddRange(ReadOnlySpan<T> items)
  {
    if (items.IsEmpty)
      return this;

    var immutableItems = ImmutableList.Create(items);
    if (IsEmpty)
      return new RecordCollection<T>(immutableItems);
    return AddRange(immutableItems);
  }

  internal IReadOnlyCollection<T> _DangerouslyGetInternalArray() => Array.AsReadOnly(_list);

  #region IImmutableList implementation overrides

  public RecordCollection<T> Add(T value)
  {
    return new([.._list, value]);
  }

  public RecordCollection<T> AddRange(IEnumerable<T> items)
  {
    if (items.TryGetNonEnumeratedCount(out int count) && count is 0)
      return this;
    return new RecordCollection<T>([.._list, ..items]);
  }

  public RecordCollection<T> Clear()
    => Empty;

  public RecordCollection<T> Insert(int index, T element)
  {
    return InsertRange(index, [element]);
  }

  public RecordCollection<T> InsertRange(int index, IEnumerable<T> items)
  {
    if (items.TryGetNonEnumeratedCount(out int count) && count is 0)
      return this;
    return new RecordCollection<T>([.._list[..index], ..items, .._list[index..]]);
  }

  public RecordCollection<T> Remove(T value, IEqualityComparer<T>? equalityComparer)
  {
    equalityComparer ??= EqualityComparer<T>.Default;
    var index = IndexOf(value, 0, Count, equalityComparer);

    if (index is not -1)
      return RemoveAt(index);

    return this;
  }

  public RecordCollection<T> RemoveAll(Predicate<T> match)
  {
    var newArray = _list.Where(x => !match(x)).ToArray();
    if (_list.Length == newArray.Length)
      return this;
    return new RecordCollection<T>(newArray);
  }

  public RecordCollection<T> RemoveAt(int index)
  {
    if (index >= Count)
      return this;
    return new([.._list[..index], .._list[(index + 1)..]]);
  }

  public RecordCollection<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer)
  {
    var set =
      items is HashSet<T> hs && (equalityComparer is null || Equals(hs.Comparer, equalityComparer))
        ? hs
        : items.ToHashSet(equalityComparer);

    var newArray = _list.Where(x => !set.Contains(x)).ToArray();
    return newArray.Length == _list.Length
      ? this
      : new(newArray);
  }

  public RecordCollection<T> RemoveRange(int index, int count)
  {
    return new(_list[index..(index + count)]);
  }

  public RecordCollection<T> Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer)
  {
    var firstIndex = IndexOf(oldValue, 0, Count, equalityComparer ?? EqualityComparer<T>.Default);

    return firstIndex is -1
      ? this
      : new([.._list[..firstIndex], newValue, .._list[firstIndex..]]);
  }

  public RecordCollection<T> SetItem(int index, T value)
  {
    return index >= Count
      ? this
      : new([.._list[..index], value, .._list[(index + 1)..]]);
  }

  #endregion IImmutableList implementation overrides

  #region IImmutableList implementation

  public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_list).GetEnumerator();

  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

  public int Count => _list.Length;

  public T this[int index] => _list[index];

  IImmutableList<T> IImmutableList<T>.Add(T value)
    => Add(value);

  IImmutableList<T> IImmutableList<T>.AddRange(IEnumerable<T> items)
    => AddRange(items);

  IImmutableList<T> IImmutableList<T>.Clear()
    => Clear();

  public int IndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer)
  {
    if (index < 0)
      throw new IndexOutOfRangeException($"index {index} is greater than size");

    equalityComparer ??= EqualityComparer<T>.Default;

    return _list
      .Skip(index)
      .Take(count)
      .Where(x => equalityComparer.Equals(x, item))
      .Select((_, i) => i + index as int?)
      .FirstOrDefault() ?? -1;
  }

  public int LastIndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer)
  {
    if (index < 0)
      throw new IndexOutOfRangeException($"index {index} is greater than size");

    equalityComparer ??= EqualityComparer<T>.Default;

    return _list
      .Skip(index)
      .Take(count)
      .Where(x => equalityComparer.Equals(x, item))
      .Select((_, i) => i + index as int?)
      .LastOrDefault() ?? -1;
  }

  IImmutableList<T> IImmutableList<T>.Insert(int index, T element)
    => Insert(index, element);

  IImmutableList<T> IImmutableList<T>.InsertRange(int index, IEnumerable<T> items)
    => InsertRange(index, items);

  IImmutableList<T> IImmutableList<T>.Remove(T value, IEqualityComparer<T>? equalityComparer)
    => Remove(value, equalityComparer);

  IImmutableList<T> IImmutableList<T>.RemoveAll(Predicate<T> match)
    => RemoveAll(match);

  IImmutableList<T> IImmutableList<T>.RemoveAt(int index)
    => RemoveAt(index);

  IImmutableList<T> IImmutableList<T>.RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer)
    => RemoveRange(items, equalityComparer);

  IImmutableList<T> IImmutableList<T>.RemoveRange(int index, int count)
    => RemoveRange(index, count);

  IImmutableList<T> IImmutableList<T>.Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer)
    => Replace(oldValue, newValue, equalityComparer);

  IImmutableList<T> IImmutableList<T>.SetItem(int index, T value)
    => SetItem(index, value);

  #endregion IImmutableList implementation

  #region equality

  public override bool Equals(object? obj)
    => Equals(obj as IImmutableList<T>);

  public bool Equals(IImmutableList<T>? other)
    => other is not null && this.SequenceEqual(other);

  // private int? _hashCache;
  public override int GetHashCode()
  {
    var hash = new HashCode();
    foreach (var item in _list)
    {
      hash.Add(item);
    }

    return hash.ToHashCode();
    // unchecked // allow int to overflow and wrap around
    // {
    //   // ReSharper disable once NonReadonlyMemberInGetHashCode
    //   return _hashCache ??= this.Aggregate(17, (acc, curr) => acc * 23 + curr?.GetHashCode() ?? 1);
    // }
  }

  #endregion equality
}

}
