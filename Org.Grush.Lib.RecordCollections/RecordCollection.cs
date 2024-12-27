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

#if NET8_0_OR_GREATER
  [System.Runtime.CompilerServices.CollectionBuilder(typeof (RecordCollection), "Create")]
#endif
  [JsonConverter(typeof(RecordCollectionJsonConverterFactory))]
  public sealed class RecordCollection<T> : IImmutableList<T>, IEquatable<IImmutableList<T>>
  {
    public static readonly RecordCollection<T> Empty = ImmutableList<T>.Empty;

    private readonly ImmutableList<T> _list;

    private RecordCollection(ImmutableList<T> list) => _list = list;

    // we only allow conversion to/from ImmutableList<T> because it's sealed and immutable
    public static implicit operator RecordCollection<T>(ImmutableList<T> l) => new(list: l);
    public static explicit operator ImmutableList<T>(RecordCollection<T> r) => r._list;

    public bool IsEmpty => _list.Count is 0;

    internal RecordCollection<T> AddRange(ReadOnlySpan<T> items)
    {
      if (items.IsEmpty)
        return this;

      var immutableItems = ImmutableList.Create(items);
      if (IsEmpty)
        return immutableItems;
      return AddRange(immutableItems);
    }

    #region IImmutableList implementation overrides

    public RecordCollection<T> Add(T value) => _list.Add(value);

    public RecordCollection<T> AddRange(IEnumerable<T> items)
    {
      if (
#if NET8_0_OR_GREATER
        items.TryGetNonEnumeratedCount(out int count) && count is 0
#else
        items is ICollection<T> { Count: 0 }
#endif
      )
        return this;

      return _list.AddRange(items);
    }

    public RecordCollection<T> Clear()
      => _list.Clear();

    public RecordCollection<T> Insert(int index, T element)
      => _list.Insert(index, element);

    public RecordCollection<T> InsertRange(int index, IEnumerable<T> items)
      => _list.InsertRange(index, items);

    public RecordCollection<T> Remove(T value, IEqualityComparer<T>? equalityComparer) =>
      _list.Remove(value, equalityComparer);

    public RecordCollection<T> RemoveAll(Predicate<T> match)
      => _list.RemoveAll(match);

    public RecordCollection<T> RemoveAt(int index)
      => _list.RemoveAt(index);

    public RecordCollection<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer) =>
      _list.RemoveRange(items, equalityComparer);

    public RecordCollection<T> RemoveRange(int index, int count)
      => _list.RemoveRange(index, count);

    public RecordCollection<T> Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer) =>
      _list.Replace(oldValue, newValue, equalityComparer);

    public RecordCollection<T> SetItem(int index, T value)
      => _list.SetItem(index, value);

    #endregion IImmutableList implementation overrides

    #region IImmutableList implementation
    public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public int Count => _list.Count;

    public T this[int index] => _list[index];

    IImmutableList<T> IImmutableList<T>.Add(T value)
      => Add(value);

    IImmutableList<T> IImmutableList<T>.AddRange(IEnumerable<T> items)
      => AddRange(items);

    IImmutableList<T> IImmutableList<T>.Clear()
      => Clear();

    public int IndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer)
      => _list.IndexOf(item, index, count, equalityComparer);

    IImmutableList<T> IImmutableList<T>.Insert(int index, T element)
      => Insert(index, element);

    IImmutableList<T> IImmutableList<T>.InsertRange(int index, IEnumerable<T> items)
      => InsertRange(index, items);

    public int LastIndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer)
      => _list.LastIndexOf(item, index, count, equalityComparer);

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
