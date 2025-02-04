using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Org.Grush.Lib.RecordCollections.Newtonsoft;

public class RecordCollectionNewtonsoftJsonConverter<T> : JsonConverter<RecordCollection<T>>
{
  public override bool CanWrite => false;

  public override void WriteJson(JsonWriter writer, RecordCollection<T> value, JsonSerializer serializer)
    => throw new NotSupportedException();

  public override RecordCollection<T> ReadJson(JsonReader reader, Type objectType, RecordCollection<T> existingValue, bool hasExistingValue,
    JsonSerializer serializer)
  {
    return serializer.Deserialize<ImmutableArray<T>>(reader);
  }
}
