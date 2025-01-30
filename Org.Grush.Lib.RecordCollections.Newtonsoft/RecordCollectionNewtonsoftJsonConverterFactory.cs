using System.Collections.Concurrent;
using System.Reflection;
using Newtonsoft.Json;

namespace Org.Grush.Lib.RecordCollections.Newtonsoft;

public class RecordCollectionNewtonsoftJsonConverterFactory(bool useConverterCache = true) : JsonConverter
{
  private static readonly ConcurrentDictionary<Type, JsonConverter> ConverterCache = new();

  public override bool CanWrite => false;

  public override bool CanConvert(Type typeToConvert)
    => typeToConvert.IsGenericType &&
       typeToConvert.GetGenericTypeDefinition() == typeof(RecordCollection<>);

  public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) => throw new NotSupportedException();

  public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
  {
    if (!useConverterCache)
      return GetConverter(objectType);

    var converter = ConverterCache.GetOrAdd(
      key: objectType,
      valueFactory: GetConverter
    );

    return converter.ReadJson(reader, objectType, existingValue, serializer);
  }

  private static JsonConverter GetConverter(Type capturedObjectType)
  {
    Type elementType = capturedObjectType.GetGenericArguments()[0];
    return (JsonConverter)Activator.CreateInstance(
      typeof(RecordCollectionNewtonsoftJsonConverter<>).MakeGenericType(
        elementType
      ),
      BindingFlags.Instance | BindingFlags.Public,
      binder: null,
      args: [],
      culture: null)!; // this can't be null
  }
}
