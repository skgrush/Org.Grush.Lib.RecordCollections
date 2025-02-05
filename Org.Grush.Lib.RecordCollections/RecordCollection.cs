using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Text.Json.Serialization;
#if !NETSTANDARD2_0
using System.Diagnostics.CodeAnalysis;
#endif

namespace Org.Grush.Lib.RecordCollections;

internal interface IRecordCollection : IList, IStructuralEquatable
{
  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  internal IStructuralEquatable InnerData { get; }
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
  IRecordCollection
{
  /// <summary>Static empty instance of a <typeparamref name="T"/> record collection.</summary>
  public static readonly RecordCollection<T> Empty = ImmutableArray<T>.Empty;

  [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
  private readonly ImmutableArray<T> _data;
  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  IStructuralEquatable IRecordCollection.InnerData => _data;

  private RecordCollection(ImmutableArray<T> data) => _data = data;

  public static bool operator==(RecordCollection<T> a, RecordCollection<T> b) => a.Equals(b);
  public static bool operator!=(RecordCollection<T> a, RecordCollection<T> b) => !a.Equals(b);
  public static bool operator==(RecordCollection<T>? a, RecordCollection<T>? b) => Equals(a, b);
  public static bool operator!=(RecordCollection<T>? a, RecordCollection<T>? b) => !Equals(a, b);

  /// <summary>true if-and-only-if the collection is empty.</summary>
  [Pure]
  public bool IsEmpty => _data.IsEmpty;
  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  bool IList.IsFixedSize => true;
  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  bool ICollection<T>.IsReadOnly => true;
  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  bool IList.IsReadOnly => true;
  /// <summary>Number of elements in the collection.</summary>
  [Pure]
  public int Count => _data.Length;

  bool ICollection.IsSynchronized => true;
  object ICollection.SyncRoot => ((ICollection)_data).SyncRoot;


  [Pure]
  public T this[int index]
  {
    get => _data[index];
    [Obsolete($"Will throw '{ExceptionMessage.Immutable}'", error: true)]
    set => throw new NotSupportedException(ExceptionMessage.Immutable);
  }

  [Pure]
  public ref readonly T ItemRef(int index) => ref _data.ItemRef(index);

  [Pure]
  public ImmutableArray<T>.Enumerator GetEnumerator() => _data.GetEnumerator();
  IEnumerator<T> IEnumerable<T>.GetEnumerator() => ((IEnumerable<T>)_data).GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<T>)_data).GetEnumerator();

  #region Conversions

  // we only allow conversion to/from ImmutableArray<T> because it's sealed and immutable
  public static implicit operator RecordCollection<T>(ImmutableArray<T> data) => new(data: data);
  public static explicit operator ImmutableArray<T>(RecordCollection<T> r) => r._data;

  [Pure]
  public ReadOnlySpan<T> AsSpan() => _data.AsSpan();
  [Pure]
  public ReadOnlyMemory<T> AsMemory() => _data.AsMemory();

  public RecordCollection<
#nullable disable
    TOther
#nullable restore
  > As<TOther>() where TOther : class?
  {
    var result = _data.As<TOther>();
    if (result.IsDefault)
      throw new InvalidCastException($"Invalid As() cast from {typeof(T)} to {typeof(TOther)}.");
    return result;
  }

  public static RecordCollection<
#nullable disable
    T
#nullable restore
  > CastUp<TDerived>(RecordCollection<TDerived> items)
    where TDerived : class?, T
  {
    return ImmutableArray<T>.CastUp((ImmutableArray<TDerived>)items);
  }

  #endregion Conversions

  #region IImmutableList implementation overrides

  /// <inheritdoc cref="IImmutableList{T}.Add"/>
  [Pure]
  public RecordCollection<T> Add(T value) => _data.Add(value);

  /// <inheritdoc cref="IImmutableList{T}.AddRange"/>
  [Pure]
  public RecordCollection<T> AddRange(IEnumerable<T> items)
    => _data.AddRange(items);

  /// <summary>Creates a new empty list of the same type.</summary>
  [Pure]
  public RecordCollection<T> Clear()
    => Empty;

  /// <inheritdoc cref="IImmutableList{T}.Insert"/>
  [Pure]
  public RecordCollection<T> Insert(int index, T element)
    => _data.Insert(index, element);

  /// <inheritdoc cref="IImmutableList{T}.InsertRange"/>
  [Pure]
  public RecordCollection<T> InsertRange(int index, IEnumerable<T> items)
    => _data.InsertRange(index, items);

  /// <inheritdoc cref="IImmutableList{T}.Remove"/>
  [Pure]
  public RecordCollection<T> Remove(T value, IEqualityComparer<T>? equalityComparer = null) =>
    _data.Remove(value, equalityComparer);

  /// <inheritdoc cref="IImmutableList{T}.RemoveAll"/>
  [Pure]
  public RecordCollection<T> RemoveAll(Predicate<T> match)
    => _data.RemoveAll(match);

  /// <inheritdoc cref="IImmutableList{T}.RemoveAt"/>
  [Pure]
  public RecordCollection<T> RemoveAt(int index)
    => _data.RemoveAt(index);

  /// <inheritdoc cref="IImmutableList{T}.RemoveRange(System.Collections.Generic.IEnumerable{T},System.Collections.Generic.IEqualityComparer{T}?)"/>
  [Pure]
  public RecordCollection<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer = null) =>
    _data.RemoveRange(items, equalityComparer);

  /// <inheritdoc cref="IImmutableList{T}.RemoveRange(int,int)"/>
  [Pure]
  public RecordCollection<T> RemoveRange(int index, int count)
    => _data.RemoveRange(index, count);

  /// <inheritdoc cref="IImmutableList{T}.Replace"/>
  [Pure]
  public RecordCollection<T> Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer = null) =>
    _data.Replace(oldValue, newValue, equalityComparer);

  /// <inheritdoc cref="IImmutableList{T}.SetItem"/>
  [Pure]
  public RecordCollection<T> SetItem(int index, T value)
    => _data.SetItem(index, value);

  /// <inheritdoc cref="IImmutableList{T}.IndexOf"/>
  [Pure]
  public int IndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer = null)
    => _data.IndexOf(item, index, count, equalityComparer);

  /// <inheritdoc cref="IImmutableList{T}.LastIndexOf"/>
  [Pure]
  public int LastIndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer = null)
    => _data.LastIndexOf(item, index, count, equalityComparer);

  #endregion IImmutableList implementation overrides

  #region IList<T> implementation
  void ICollection<T>.Add(T ele) => throw new NotSupportedException(ExceptionMessage.Immutable);
  void ICollection<T>.Clear() => throw new NotSupportedException(ExceptionMessage.Immutable);
  bool ICollection<T>.Remove(T item) => throw new NotSupportedException(ExceptionMessage.Immutable);
  void IList<T>.Insert(int index, T item) => throw new NotSupportedException(ExceptionMessage.Immutable);
  void IList<T>.RemoveAt(int index) => throw new NotSupportedException(ExceptionMessage.Immutable);
  /// <inheritdoc cref="IList{T}.IndexOf"/>
  [Pure]
  public int IndexOf(T item) => _data.IndexOf(item);
  /// <inheritdoc cref="ICollection{T}.Contains"/>
  [Pure]
  public bool Contains(T item) => _data.Contains(item);
  /// <inheritdoc cref="ImmutableArray{T}.CopyTo(T[],int)"/>
  public void CopyTo(T[] array, int arrayIndex) => _data.CopyTo(array, arrayIndex);
  /// <summary>Copies the contents of this collection to a <see cref="Span{T}"/>.</summary>
  public void CopyTo(Span<T> destination) => _data.CopyTo(destination);
  #endregion IList<T> implementation

  #region IList implementation
  object? IList.this[int index]
  {
    get => this[index];
    [Obsolete($"Will throw '{ExceptionMessage.Immutable}'", error: true)]
    set => throw new NotSupportedException(ExceptionMessage.Immutable);
  }
  bool IList.Contains(object? value) => value is T t && _data.Contains(t);
  int IList.IndexOf(object? value) => value is T t ? _data.IndexOf(t) : -1;

  void IList.Clear() => throw new NotSupportedException(ExceptionMessage.Immutable);
  void IList.RemoveAt(int index) => throw new NotSupportedException(ExceptionMessage.Immutable);
  int IList.Add(object? value) => throw new NotSupportedException(ExceptionMessage.Immutable);
  void ICollection.CopyTo(Array array, int index) => throw new NotSupportedException(ExceptionMessage.Immutable);
  void IList.Insert(int index, object? value) => throw new NotSupportedException(ExceptionMessage.Immutable);
  void IList.Remove(object? value) => throw new NotSupportedException(ExceptionMessage.Immutable);

  #endregion IList implementation

  #region IImmutableList<T> implementation
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
  #endregion IImmutableList<T> implementation

  #region equality
  [Pure]
  public override bool Equals(
    [NotNullWhen(true)] object? obj
  ) => obj is RecordCollection<T> recordCollection && Equals(recordCollection);

  /// <summary>Compares sequence-equality with any other <see cref="IImmutableList{T}"/>.</summary>
  [Pure]
  public bool Equals(RecordCollection<T> other)
    => _data.SequenceEqual(other._data);
  [Pure]
  public bool Equals(RecordCollection<T> other, IEqualityComparer<T> comparer)
    => _data.SequenceEqual(other._data, comparer);

  [Pure]
  public static bool Equals(RecordCollection<T>? a, RecordCollection<T>? b)
    => RecordCollection.Equals(a, b);

  /// <summary>
  /// Gets/caches the combined <see cref="HashCode"/> of each item in the sequence.
  /// </summary>
  [Pure]
  public override int GetHashCode() => GetHashCode(null);

  [Pure]
  public int GetHashCode(IEqualityComparer<T>? comparer)
  {
    var hash = new HashCode();

    foreach (var item in _data)
      hash.Add(item, comparer);

    return hash.ToHashCode();
  }

  /// <summary>
  /// Sequence equality check for an enumerable sequence of unknown type.
  /// </summary>
  [Pure]
  public bool UntypedSequenceEqual(IEnumerable otherEnumerable, IEqualityComparer comparer)
  {
    int count = Count;
    if (otherEnumerable is ICollection list && count != list.Count)
      return false;

    int i = 0;
    foreach (object item in otherEnumerable)
    {
      if (i == count)
        return false;

      if (!comparer.Equals(_data[i], item))
        return false;

      ++i;
    }

    return i == count;
  }

  #endregion equality

  #region IStructuralEquatable

  bool IStructuralEquatable.Equals(
    [NotNullWhen(true)] object? other,
    IEqualityComparer comparer
  )
  {
    if (other is null)
      return false;

    if (other is IEnumerable<T> otherOfT && comparer is IEqualityComparer<T> comparerOfT)
    {
      if (other is RecordCollection<T> r)
        return _data.SequenceEqual(r._data, comparerOfT);

      return _data.SequenceEqual(otherOfT, comparerOfT);
    }

    if (other is IRecordCollection ir)
      return ((IStructuralEquatable)_data).Equals(ir.InnerData, comparer);

    if (other is IEnumerable enumerable)
      return UntypedSequenceEqual(enumerable, comparer);

    if (other is IStructuralEquatable equatable)
      return ((IStructuralEquatable)this).GetHashCode(comparer) == equatable.GetHashCode(comparer);

    return false;
  }

  int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
  {
    if (comparer is IEqualityComparer<T> innerComparer)
      return GetHashCode(innerComparer);

    var hash = new HashCode();
    // minor optimization if value-typed to avoid null-checks
    if (typeof(T).IsValueType)
    {
      foreach (var item in _data)
        hash.Add(comparer.GetHashCode(item!));
    }
    else
    {
      foreach (var item in _data)
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
