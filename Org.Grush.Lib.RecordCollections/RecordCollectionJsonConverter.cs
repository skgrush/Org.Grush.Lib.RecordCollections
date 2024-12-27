using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Org.Grush.Lib.RecordCollections;

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
    writer.WriteRawValue(JsonSerializer.Serialize(value._DangerouslyGetInternalArray(), options));
  }
}
