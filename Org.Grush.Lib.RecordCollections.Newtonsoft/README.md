# `RecordCollection<T>` Newtonsoft serialization extension

Newtonsoft extension for the `Org.Grush.Lib.RecordCollections` core package
(see the core package's
[NuGet package](https://www.nuget.org/packages/Org.Grush.Lib.RecordCollections) or
[GitHub source](https://github.com/skgrush/Org.Grush.Lib.RecordCollections/tree/main/Org.Grush.Lib.RecordCollections)).

## Example

Specific usage where collection types are known:

```cs
var collection = JsonConvert.DeserializeObject<RecordCollection<string>>(
  value: str,
  settings: new JsonSerializerSettings
    {
      Converters = { new RecordCollectionNewtonsoftJsonConverter<string>() }
    }
);
```

Alternatively if using "global" de/serializer options, e.g. in ASP.NET,
to deserialize any record collections.

```cs
services.AddControllers().AddNewtonsoftJson(o =>
{
  o.SerializerSettings.Converters =
  {
    new RecordCollectionNewtonsoftJsonConverterFactory()
  }
});
```
