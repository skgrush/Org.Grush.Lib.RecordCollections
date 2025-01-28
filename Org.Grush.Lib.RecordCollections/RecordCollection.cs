using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Org.Grush.Lib.RecordCollections {

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

/// <summary>
/// Collection type with record compatibility (value equal, immutable, de/serializable).
///
/// Casting to and from <see cref="ImmutableArray{T}"/> is effectively free.
/// </summary>
/// <typeparam name="T">The element type.</typeparam>
#if NET8_0_OR_GREATER
[System.Runtime.CompilerServices.CollectionBuilder(typeof(RecordCollection), nameof(RecordCollection.Create))]
#endif
[JsonConverter(typeof(RecordCollectionJsonConverterFactory))]
public readonly struct RecordCollection<T> :
  IList<T>,
  IImmutableList<T>,
  IEquatable<RecordCollection<T>>,
  IStructuralEquatable
{
  /// <summary>Static empty instance of a <typeparamref name="T"/> record collection.</summary>
  public static readonly RecordCollection<T> Empty = ImmutableArray<T>.Empty;

  [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
  private readonly ImmutableArray<T> _data;

  private RecordCollection(ImmutableArray<T> data) => _data = data;

  // we only allow conversion to/from ImmutableArray<T> because it's sealed and immutable
  public static implicit operator RecordCollection<T>(ImmutableArray<T> data) => new(data: data);
  public static explicit operator ImmutableArray<T>(RecordCollection<T> r) => r._data;

  public ReadOnlySpan<T> AsSpan() => _data.AsSpan();
  public ReadOnlyMemory<T> AsMemory() => _data.AsMemory();

  public static bool operator==(RecordCollection<T> a, RecordCollection<T> b) => a.Equals(b);
  public static bool operator!=(RecordCollection<T> a, RecordCollection<T> b) => !a.Equals(b);
  public static bool operator==(RecordCollection<T>? a, RecordCollection<T>? b) => Equals(a, b);
  public static bool operator!=(RecordCollection<T>? a, RecordCollection<T>? b) => !Equals(a, b);

  /// <summary>true if-and-only-if the collection is empty.</summary>
  public bool IsEmpty => _data.IsEmpty;
  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  bool ICollection<T>.IsReadOnly => true;
  /// <summary>Number of elements in the collection.</summary>
  public int Count => _data.Length;

  public T this[int index]
  {
    get => _data[index];
    [DoesNotReturn, Obsolete($"Will throw '{ExceptionMessage.Immutable}'")]
    set => throw new NotSupportedException(ExceptionMessage.Immutable);
  }

  public ref readonly T ItemRef(int index) => ref _data.ItemRef(index);

  public ImmutableArray<T>.Enumerator GetEnumerator() => _data.GetEnumerator();
  IEnumerator<T> IEnumerable<T>.GetEnumerator() => ((IEnumerable<T>)_data).GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<T>)_data).GetEnumerator();

  #region IImmutableList implementation overrides

  /// <inheritdoc cref="IImmutableList{T}.Add"/>
  public RecordCollection<T> Add(T value) => _data.Add(value);

  /// <inheritdoc cref="IImmutableList{T}.AddRange"/>
  public RecordCollection<T> AddRange(IEnumerable<T> items)
    => _data.AddRange(items);

  /// <summary>Creates a new empty list of the same type.</summary>
  public RecordCollection<T> Clear()
    => Empty;

  /// <inheritdoc cref="IImmutableList{T}.Insert"/>
  public RecordCollection<T> Insert(int index, T element)
    => _data.Insert(index, element);

  /// <inheritdoc cref="IImmutableList{T}.InsertRange"/>
  public RecordCollection<T> InsertRange(int index, IEnumerable<T> items)
    => _data.InsertRange(index, items);

  /// <inheritdoc cref="IImmutableList{T}.Remove"/>
  public RecordCollection<T> Remove(T value, IEqualityComparer<T>? equalityComparer) =>
    _data.Remove(value, equalityComparer);

  /// <inheritdoc cref="IImmutableList{T}.RemoveAll"/>
  public RecordCollection<T> RemoveAll(Predicate<T> match)
    => _data.RemoveAll(match);

  /// <inheritdoc cref="IImmutableList{T}.RemoveAt"/>
  public RecordCollection<T> RemoveAt(int index)
    => _data.RemoveAt(index);

  /// <inheritdoc cref="IImmutableList{T}.RemoveRange(System.Collections.Generic.IEnumerable{T},System.Collections.Generic.IEqualityComparer{T}?)"/>
  public RecordCollection<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer) =>
    _data.RemoveRange(items, equalityComparer);

  /// <inheritdoc cref="IImmutableList{T}.RemoveRange(int,int)"/>
  public RecordCollection<T> RemoveRange(int index, int count)
    => _data.RemoveRange(index, count);

  /// <inheritdoc cref="IImmutableList{T}.Replace"/>
  public RecordCollection<T> Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer) =>
    _data.Replace(oldValue, newValue, equalityComparer);

  /// <inheritdoc cref="IImmutableList{T}.SetItem"/>
  public RecordCollection<T> SetItem(int index, T value)
    => _data.SetItem(index, value);

  /// <inheritdoc cref="IImmutableList{T}.IndexOf"/>
  public int IndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer)
    => _data.IndexOf(item, index, count, equalityComparer);

  /// <inheritdoc cref="IImmutableList{T}.LastIndexOf"/>
  public int LastIndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer)
    => _data.LastIndexOf(item, index, count, equalityComparer);

  #endregion IImmutableList implementation overrides

  #region IList implementation
  void ICollection<T>.Add(T ele) => throw new NotSupportedException(ExceptionMessage.Immutable);
  void ICollection<T>.Clear() => throw new NotSupportedException(ExceptionMessage.Immutable);
  bool ICollection<T>.Remove(T item) => throw new NotSupportedException(ExceptionMessage.Immutable);
  void IList<T>.Insert(int index, T item) => throw new NotSupportedException(ExceptionMessage.Immutable);
  void IList<T>.RemoveAt(int index) => throw new NotSupportedException(ExceptionMessage.Immutable);
  /// <inheritdoc cref="IList{T}.IndexOf"/>
  public int IndexOf(T item) => _data.IndexOf(item);
  /// <inheritdoc cref="ICollection{T}.Contains"/>
  public bool Contains(T item) => _data.Contains(item);
  /// <inheritdoc cref="ImmutableArray{T}.CopyTo(T[],int)"/>
  public void CopyTo(T[] array, int arrayIndex) => _data.CopyTo(array, arrayIndex);
  /// <summary>Copies the contents of this collection to a <see cref="Span{T}"/>.</summary>
  public void CopyTo(Span<T> destination) => _data.CopyTo(destination);
  #endregion IList implementation

  #region IImmutableList implementation
  IImmutableList<T> IImmutableList<T>.Add(T value) => Add(value);
  IImmutableList<T> IImmutableList<T>.AddRange(IEnumerable<T> items) => AddRange(items);
  IImmutableList<T> IImmutableList<T>.Clear() => Clear();
  IImmutableList<T> IImmutableList<T>.Insert(int index, T element) => Insert(index, element);
  IImmutableList<T> IImmutableList<T>.InsertRange(int index, IEnumerable<T> items) => InsertRange(index, items);
  IImmutableList<T> IImmutableList<T>.Remove(T value, IEqualityComparer<T>? equalityComparer) => Remove(value, equalityComparer);
  IImmutableList<T> IImmutableList<T>.RemoveAll(Predicate<T> match) => RemoveAll(match);
  IImmutableList<T> IImmutableList<T>.RemoveAt(int index) => RemoveAt(index);
  IImmutableList<T> IImmutableList<T>.RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer) => RemoveRange(items, equalityComparer);
  IImmutableList<T> IImmutableList<T>.RemoveRange(int index, int count) => RemoveRange(index, count);
  IImmutableList<T> IImmutableList<T>.Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer) => Replace(oldValue, newValue, equalityComparer);
  IImmutableList<T> IImmutableList<T>.SetItem(int index, T value) => SetItem(index, value);
  #endregion IImmutableList implementation

  #region equality
  public override bool Equals([NotNullWhen(true)] object? obj)
    => obj is RecordCollection<T> recordCollection && Equals(recordCollection);

  /// <summary>Compares sequence-equality with any other <see cref="IImmutableList{T}"/>.</summary>
  public bool Equals(RecordCollection<T> other)
    => _data.SequenceEqual(other._data);
  public bool Equals(RecordCollection<T> other, IEqualityComparer<T> comparer)
    => _data.SequenceEqual(other._data, comparer);

  public static bool Equals(RecordCollection<T>? a, RecordCollection<T>? b)
    => RecordCollection.Equals(a, b);

  /// <summary>
  /// Gets/caches the combined <see cref="HashCode"/> of each item in the sequence.
  /// </summary>
  public override int GetHashCode() => GetHashCode(null);

  public int GetHashCode(IEqualityComparer<T>? comparer)
  {
    var hash = new HashCode();

    foreach (var item in _data)
    {
      hash.Add(item, comparer);
    }
    return hash.ToHashCode();
  }

  #endregion equality

  #region IStructuralEquatable

  bool IStructuralEquatable.Equals([NotNullWhen(true)] object? other, IEqualityComparer comparer)
  {
    if (other is null)
      return false;

    if (comparer is IEqualityComparer<T> c)
    {
      if (other is RecordCollection<T> r)
        return Equals(r, c);

      return (other as IEnumerable<T>)?.SequenceEqual(_data, c) ?? false;
    }

    if (other is IStructuralEquatable equatable)
      return ((IStructuralEquatable)this).GetHashCode(comparer) == equatable.GetHashCode(comparer);

    return false;
  }

  int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
  {
    if (comparer is IEqualityComparer<T> innerComparer)
      return GetHashCode(innerComparer);

    var hash = new HashCode();
    foreach (var item in _data)
    {
      hash.Add(item is null ? 0 : comparer.GetHashCode(item));
    }
    return hash.ToHashCode();
  }

  #endregion IStructuralEquatable


  private static class ExceptionMessage
  {
    public const string Immutable = nameof(RecordCollection) + " is immutable.";
  }
}
}
