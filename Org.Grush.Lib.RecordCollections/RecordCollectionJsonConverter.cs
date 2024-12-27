using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Org.Grush.Lib.RecordCollections;

#if NET8_0_OR_GREATER
[System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("JSON de/serialization requires references to type T which may not be statically analyzed.")]
[System.Diagnostics.CodeAnalysis.RequiresDynamicCode("JSON de/serialization requires a JsonConverter for <T>, which cannot be statically analyzed. For AOT, use this in a JsonSerializerContext partial in the Converters list on the JsonSourceGenerationOptionsAttribute.")]
#endif
public class RecordCollectionJsonConverter<T> : JsonConverter<RecordCollection<T>>
{
  public override RecordCollection<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    var jsonDeserialized = JsonSerializer.Deserialize<ImmutableArray<T>>(
      reader: ref reader,
      options
    );

    return jsonDeserialized;
  }

  public override void Write(Utf8JsonWriter writer, RecordCollection<T> value, JsonSerializerOptions options)
  {
    writer.WriteRawValue(JsonSerializer.Serialize((ImmutableArray<T>)value, options));
  }
}
