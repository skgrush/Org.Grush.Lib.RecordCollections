# `RecordCollection<T>` Newtonsoft serialization extension

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
