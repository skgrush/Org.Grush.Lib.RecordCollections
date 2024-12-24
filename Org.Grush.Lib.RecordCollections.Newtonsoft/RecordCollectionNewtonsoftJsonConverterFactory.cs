using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Reflection;
using Newtonsoft.Json;

namespace Org.Grush.Lib.RecordCollections.Newtonsoft;

public class RecordCollectionNewtonsoftJsonConverterFactory : JsonConverter
{
  private static readonly ConcurrentDictionary<string, JsonConverter> _converterCache = new();

  public override bool CanWrite => false;

  public override bool CanConvert(Type typeToConvert)
    => typeToConvert.IsGenericType &&
       typeToConvert.GetGenericTypeDefinition() == typeof(RecordCollection<>);

  public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) => throw new NotImplementedException();

  public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
  {
    var converter = _converterCache.GetOrAdd(
      key: objectType.FullName,
      valueFactory: static (_, capturedObjectType) =>
      {
        Type elementType = capturedObjectType.GetGenericArguments()[0];
        var converter = (JsonConverter)Activator.CreateInstance(
          typeof(RecordCollectionNewtonsoftJsonConverter<>).MakeGenericType([
            elementType
          ]),
          BindingFlags.Instance | BindingFlags.Public,
          binder: null,
          args: [],
          culture: null);

        return converter;
      },
      factoryArgument: objectType
    );

    return converter.ReadJson(reader, objectType, existingValue, serializer);
  }
}

public class RecordCollectionNewtonsoftJsonConverter<T> : JsonConverter<RecordCollection<T>>
{
  public override bool CanWrite => false;

  public override void WriteJson(JsonWriter writer, RecordCollection<T>? value, JsonSerializer serializer)
  {
    serializer.Serialize(writer, value, typeof(IImmutableList<T>));
  }

  public override RecordCollection<T>? ReadJson(JsonReader reader, Type objectType, RecordCollection<T>? existingValue, bool hasExistingValue,
    JsonSerializer serializer)
  {
    var list = serializer.Deserialize<List<T>>(reader);
    return list is null
      ? null :
      RecordCollection.CreateRange(list);
  }
}
