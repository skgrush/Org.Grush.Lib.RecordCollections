using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Org.Grush.Lib.RecordCollections {

public static class RecordCollection
{
  public static RecordCollection<T> Create<T>() => RecordCollection<T>.Empty;

  public static RecordCollection<T> Create<T>(ReadOnlySpan<T> items)
  {
    return RecordCollection<T>.Empty.AddRange(items);
  }

  // public static RecordCollection<T> Create<T>(IImmutableList<T> items)
  //   => RecordCollection<T>.Empty.AddRange(items);

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

  public bool IsEmpty => _list.IsEmpty;

  internal RecordCollection<T> AddRange(ReadOnlySpan<T> items)
  {
    var immutableItems = ImmutableList.Create(items);
    if (IsEmpty)
      return NewIfDifferent(immutableItems);
    return AddRange(immutableItems);
  }

  #region IImmutableList implementation overrides

  /// <inheritdoc cref="IImmutableList{T}.Add"/>
  public RecordCollection<T> Add(T value) => new(_list.Add(value));

  /// <inheritdoc cref="IImmutableList{T}.AddRange"/>
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

    return NewIfDifferent(_list.AddRange(items));
  }

  /// <inheritdoc cref="IImmutableList{T}.Clear"/>
  public RecordCollection<T> Clear()
    => NewIfDifferent(_list.Clear());

  /// <inheritdoc cref="IImmutableList{T}.Insert"/>
  public RecordCollection<T> Insert(int index, T element)
    => NewIfDifferent(_list.Insert(index, element));

  /// <inheritdoc cref="IImmutableList{T}.InsertRange"/>
  public RecordCollection<T> InsertRange(int index, IEnumerable<T> items)
    => NewIfDifferent(_list.InsertRange(index, items));

  /// <inheritdoc cref="IImmutableList{T}.Remove"/>
  public RecordCollection<T> Remove(T value, IEqualityComparer<T>? equalityComparer) =>
    NewIfDifferent(_list.Remove(value, equalityComparer));

  /// <inheritdoc cref="IImmutableList{T}.RemoveAll"/>
  public RecordCollection<T> RemoveAll(Predicate<T> match)
    => NewIfDifferent(_list.RemoveAll(match));

  /// <inheritdoc cref="IImmutableList{T}.RemoveAt"/>
  public RecordCollection<T> RemoveAt(int index)
    => NewIfDifferent(_list.RemoveAt(index));

  /// <inheritdoc cref="IImmutableList{T}.RemoveRange(System.Collections.Generic.IEnumerable{T},System.Collections.Generic.IEqualityComparer{T}?)"/>
  public RecordCollection<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer) =>
    NewIfDifferent(_list.RemoveRange(items, equalityComparer));

  /// <inheritdoc cref="IImmutableList{T}.RemoveRange(int,int)"/>
  public RecordCollection<T> RemoveRange(int index, int count)
    => NewIfDifferent(_list.RemoveRange(index, count));

  /// <inheritdoc cref="IImmutableList{T}.Replace"/>
  public RecordCollection<T> Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer) =>
    NewIfDifferent(_list.Replace(oldValue, newValue, equalityComparer));

  /// <inheritdoc cref="IImmutableList{T}.SetItem"/>
  public RecordCollection<T> SetItem(int index, T value)
    => NewIfDifferent(_list.SetItem(index, value));

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

  /// <summary>Compares sequence-equality with any other <see cref="IImmutableList{T}"/>.</summary>
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

  private RecordCollection<T> NewIfDifferent(ImmutableList<T> other)
  {
    if (ReferenceEquals(other, _list))
      return this;
    if (other.Count is 0)
      return Empty;
    return new(other);
  }
}
}
