using System.Text.Json;
using System.Text.Json.Serialization;

namespace Org.Grush.Lib.RecordCollections;

#if NET8_0_OR_GREATER
[System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("JSON de/serialization requires references to type T which may not be statically analyzed.")]
[System.Diagnostics.CodeAnalysis.RequiresDynamicCode("JSON de/serialization requires a JsonConverter for <T>, which cannot be statically analyzed. For AOT, use this in a JsonSerializerContext partial in the Converters list on the JsonSourceGenerationOptionsAttribute.")]
#endif
public class RecordCollectionStrictJsonConverter<T> : JsonConverter<RecordCollection<T>>
{
  public override bool HandleNull => false;

  private JsonConverter<T>? SubConverter { get; set; }

  public override RecordCollection<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    if (reader.TokenType is not JsonTokenType.StartArray)
      throw new JsonException($"Bad json start; expected StartArray but found {reader.TokenType}");

    JsonConverter<T> subConverter = GetSubConverter(options);

    List<T> list = [];
    while (reader.Read())
    {
      if (reader.TokenType is JsonTokenType.EndArray)
      {
        return list.ToRecordCollection();
      }

      list.Add(
        subConverter.Read(ref reader, typeof(T), options)!
      );
    }

    throw new JsonException("Bad json end; expected EndArray but reached end of sequence.");
  }

  public override void Write(Utf8JsonWriter writer, RecordCollection<T> value, JsonSerializerOptions options)
  {
    writer.WriteStartArray();
    var subConverter = GetSubConverter(options);

    foreach (T subValue in value)
      subConverter.Write(writer, subValue, options);

    writer.WriteEndArray();
  }

  private JsonConverter<T> GetSubConverter(JsonSerializerOptions options)
  {
    if (SubConverter is not null)
      return SubConverter;

    var genericConverter = options.GetConverter(typeof(T));
    return genericConverter is JsonConverter<T> converter
      ? SubConverter = converter
      : throw new JsonException($"SubConverter for {typeof(T)} ({genericConverter}) is not a JsonConverter<T>.");
  }

}
