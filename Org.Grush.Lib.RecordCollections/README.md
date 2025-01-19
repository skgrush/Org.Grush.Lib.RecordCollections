# `RecordCollection<T>`

A read-only, equatable, de/serializable, generic collection type for use in record classes.

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

areEqual.Should().BeTrue();
```

additionally, `RecordCollection<T>`s can be used in hash structures like `HashSet`s:

```cs
HashSet<RecordCollection<double>> set = [
  [1.1, 2.2]
];

var contains = set.Contains([1.1, 2.2]);

contains.Should().BeTrue();
```

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
