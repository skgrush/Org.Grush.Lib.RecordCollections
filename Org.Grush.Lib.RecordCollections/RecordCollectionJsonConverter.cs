#if NET8_0_OR_GREATER
using System.Reflection;
using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Org.Grush.Lib.RecordCollections;

public class RecordCollectionJsonConverterFactory : JsonConverterFactory
{
  public override bool CanConvert(Type typeToConvert)
    => typeToConvert.IsGenericType &&
       typeToConvert.GetGenericTypeDefinition() == typeof(RecordCollection<>);

  public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
  {
    Type elementType = typeToConvert.GetGenericArguments()[0];

    return Activator.CreateInstance(
      typeof(RecordCollectionJsonConverter<>).MakeGenericType([
        elementType
      ]),
      BindingFlags.Instance | BindingFlags.Public,
      binder: null,
      args: [],
      culture: null) as JsonConverter;
  }
}

public class RecordCollectionJsonConverter<T> : JsonConverter<RecordCollection<T>>
{
  public override RecordCollection<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    var jsonDeserialized = JsonSerializer.Deserialize<ImmutableList<T>>(
      reader: ref reader,
      options
    );

    return jsonDeserialized is null
      ? null
      : new RecordCollection<T>(jsonDeserialized);
  }

  public override void Write(Utf8JsonWriter writer, RecordCollection<T> value, JsonSerializerOptions options)
  {
    writer.WriteRawValue(JsonSerializer.Serialize((ImmutableList<T>)value));
  }
}
#endif
