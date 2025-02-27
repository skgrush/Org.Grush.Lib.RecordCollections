# `RecordCollection<T>`

A read-only, equatable, de/serializable, generic collection type for use in record classes.

TOC:
* [Initialization](#initialization)
* [Equating](#equating)
* [Serialization](#serialization)
* [Deserialization](#deserialization)
* [Target versions](#target-versions)

## Initialization

`RecordCollection`s can be initialized using the new collection expression approach:

```cs
RecordCollection<double> c = [3.14159, double.NaN, double.PositiveInfinity];
```

or using the static `Create` methods, e.g.

```cs
var a = RecordCollection.Create(["a", "b", "c"]);
```

or using the LINQ-like extension on an existing IEnumerable:

```cs
List<int> oldList = [1, 2, 3];
var a = oldList.ToRecordCollection();
```

## Equating

Two `RecordCollection`s that are sequence-equal will return true from `Equals()`:

```cs
RecordCollection<int> collectionA = [1, 2, 3];
var collectionB = RecordCollection.Create([1, 2, 3]);

var areEqual = collectionA.Equals(collectionB);

areEqual.Should().BeTrue();
```

This means that two records that contain `RecordCollection<T>` properties are still equatable

```cs
record MyRecord(string Name, RecordCollection<string> Aliases);

MyRecord a = new("Joseph", ["Joe", "Joey"]);
MyRecord b = new("Joseph", ["Joe", "Joey"]);

var areEqual = a.Equals(b);
var areEqualsSign = a == b;

areEqual.Should().BeTrue();
areEqualsSign.Should().BeTrue();
```

additionally, `RecordCollection<T>`s can be used in hash structures like `HashSet`s:

```cs
HashSet<RecordCollection<double>> set = [
  [1.1, 2.2]
];

var contains = set.Contains([1.1, 2.2]);

contains.Should().BeTrue();
```

**`Equals` rules:**
1. Calls to `RecordCollection<T>#Equals(object)` or `RecordCollection<T>#Equals(object, IEqualityComparer<T>)` are only supported for other `RecordCollection<T>`.
2. Explicit calls to `((IStructuralEquatable)RecordCollection<T>)#Equals(object?, IEqualityComparer)`
follow a priority order for checking:
   1. if other implements `IEnumerable<T>` AND comparer implements `IEqualityComparer<T>`,
      we run a check using the `<T>`-typed equality comparison.
   2. if other implements `IEnumerable` not `<T>`, a de-optimized `SequenceEqual` is used.
   3. if other implements `IStructuralEquatable`, we call `#GetHashCode(comparer)` on each instance and compare;
      **NOTE: this eagerly evaluates the entire sequence**.

## Serialization

Serialization is implicitly supported by both System.Text.Json and Newtonsoft.

## Deserialization


### System.Text.Json
Reflection-based serialization **is supported implicitly**.

For AOT-compatible serialization the `RecordCollectionStrictJsonConverter<T>` is provided by the core package:

```cs
using System.Text.Json;
using System.Text.Json.Serialization;
using Org.Grush.Lib.RecordCollections;

namespace TestProgram;

string jsonData =
  """
  [{ "Name": "Joseph", "Alias": "Joey" }, { "Name": "Tom" }]
  """;

RecordCollection<Datum>? data = JsonSerializer.Deserialize(
  json: jsonData,
  jsonTypeInfo: RecordCollectionOfDataContext.Default.RecordCollectionDatum
);



record Datum(string Name, string? Alias);

[JsonSourceGenerationOptions(WriteIndented = true, Converters = [typeof(RecordCollectionStrictJsonConverter<Datum>)])]
[JsonSerializable(typeof(RecordCollection<Datum>))]
[JsonSerializable(typeof(ImmutableArray<Datum>))]
[JsonSerializable(typeof(Datum))]
internal partial class RecordCollectionOfDataContext : JsonSerializerContext;
```

### Newtonsoft
Newtonsoft deserialization is supported using the supplementary `Org.Grush.Lib.RecordCollections.Newtonsoft` package,
either with the generic `RecordCollectionNewtonsoftJsonConverterFactory`,
or if a specific type is known then `RecordCollectionNewtonsoftJsonConverter<T>` converter can be used directly.

See the [NuGet package](https://www.nuget.org/packages/Org.Grush.Lib.RecordCollections.Newtonsoft)
or the [GitHub source](https://github.com/skgrush/Org.Grush.Lib.RecordCollections/tree/main/Org.Grush.Lib.RecordCollections.Newtonsoft).

```cs
using Newtonsoft.Json;
using Org.Grush.Lib.RecordCollections.Newtonsoft;

namespace TestProgram;

string jsonData =
  """
  {
    "Strings": ["a", "b"],
    "Ints": [1, 2]
  }
  """;

PairOfLists? pair1 = JsonConvert.DeserializeObject<PairOfLists>(jsonData, new JsonSerializerSettings
{
  Converters = { new RecordCollectionNewtonsoftJsonConverterFactory() }
});

PairOfLists? pair2 = JsonConvert.DeserializeObject<PairOfLists>(jsonData, new JsonSerializerSettings
{
  Converters = {
    new RecordCollectionNewtonsoftJsonConverter<int>(),
    new RecordCollectionNewtonsoftJsonConverter<string>(),
  }
});

record PairOfLists(RecordCollection<string> Strings, RecordCollection<int> Ints);
```


## Target Versions

Features differ subtly between target versions.

.NET 8+ version has no dependencies or shims,
is AOT-compilation compatible,
and supports the collection builder syntax.

.NET Standard 2.1 version requires two System NuGet packages, `System.Collections.Immutable` and `System.Text.Json`,
but supports .NET 5–7.

.NET Standard 2.0 version requires the above System NuGet packages and also the `Microsoft.Bcl.HashCode` NuGet package,
loses some nullability checks,
but supports a significantly broader set of .NET versions including 4.6.1–4.8, Mono 5.4, and UWP.
