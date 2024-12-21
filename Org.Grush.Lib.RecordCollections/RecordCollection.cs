using System.Collections;
using System.Collections.Immutable;

namespace Org.Grush.Lib.RecordCollections
{
  public static class RecordCollection
  {
    public static RecordCollection<T> Create<T>() => RecordCollection<T>.Empty;

    public static RecordCollection<T> Create<T>(ReadOnlySpan<T> items)
      => RecordCollection<T>.Empty.AddRange(items);
  }

#if NET8_0_OR_GREATER
[System.Runtime.CompilerServices.CollectionBuilder(typeof (RecordCollection), "Create")]
#endif
  public sealed class RecordCollection<T> : IImmutableList<T>, IEquatable<IImmutableList<T>>
  {
    public static readonly RecordCollection<T> Empty =
#if NET8_0_OR_GREATER
      new([]);
#else
    new RecordCollection<T>(ImmutableArray<T>.Empty);
#endif

    private readonly IImmutableList<T> _list;

    private RecordCollection(IImmutableList<T> list) => _list = list;

    public bool IsEmpty => _list.Count is 0;

    internal RecordCollection<T> AddRange(ReadOnlySpan<T> items)
    {
      if (items.IsEmpty)
        return this;

      var immutableItems = ImmutableList.Create(items);
      if (IsEmpty)
        return new RecordCollection<T>(immutableItems);
      return (RecordCollection<T>)AddRange(immutableItems);
    }

    #region IImmutableList implementation
    public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public int Count => _list.Count;

    public T this[int index] => _list[index];

    public IImmutableList<T> Add(T value)
      => new RecordCollection<T>(_list.Add(value));

    public IImmutableList<T> AddRange(IEnumerable<T> items)
    {
      if (items is ICollection<T> { Count: 0 })
        return this;
      return new RecordCollection<T>(_list.AddRange(items));
    }

    public IImmutableList<T> Clear()
      => new RecordCollection<T>(_list.Clear());

    public int IndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer)
      => _list.IndexOf(item, index, count, equalityComparer);

    public IImmutableList<T> Insert(int index, T element)
      => new RecordCollection<T>(_list.Insert(index, element));

    public IImmutableList<T> InsertRange(int index, IEnumerable<T> items)
      => new RecordCollection<T>(_list.InsertRange(index, items));

    public int LastIndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer)
      => _list.LastIndexOf(item, index, count, equalityComparer);

    public IImmutableList<T> Remove(T value, IEqualityComparer<T>? equalityComparer)
      => new RecordCollection<T>(_list.Remove(value, equalityComparer));

    public IImmutableList<T> RemoveAll(Predicate<T> match)
      => new RecordCollection<T>(_list.RemoveAll(match));

    public IImmutableList<T> RemoveAt(int index)
      => new RecordCollection<T>(_list.RemoveAt(index));

    public IImmutableList<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer)
      => new RecordCollection<T>(_list.RemoveRange(items, equalityComparer));

    public IImmutableList<T> RemoveRange(int index, int count)
      => new RecordCollection<T>(_list.RemoveRange(index, count));

    public IImmutableList<T> Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer)
      => new RecordCollection<T>(_list.Replace(oldValue, newValue, equalityComparer));

    public IImmutableList<T> SetItem(int index, T value)
      => new RecordCollection<T>(_list.SetItem(index, value));
    #endregion IImmutableList implementation

    #region equality
    public override bool Equals(object? obj)
      => Equals(obj as IImmutableList<T>);

    public bool Equals(IImmutableList<T>? other)
      => this.SequenceEqual(other ?? ImmutableList<T>.Empty);

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
