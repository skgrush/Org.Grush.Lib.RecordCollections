using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Org.Grush.Lib.RecordCollections;

#if NET8_0_OR_GREATER
[System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Dynamically references generic types that may not be available at runtime.")]
#endif
public class RecordCollectionJsonConverterFactory : JsonConverterFactory
{
  public override bool CanConvert(Type typeToConvert)
    => typeToConvert.IsGenericType &&
       typeToConvert.GetGenericTypeDefinition() == typeof(RecordCollection<>);

  public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
  {
    Type elementType = typeToConvert.GetGenericArguments()[0];

    return Activator.CreateInstance(
      typeof(RecordCollectionStrictJsonConverter<>).MakeGenericType([
        elementType
      ]),
      BindingFlags.Instance | BindingFlags.Public,
      binder: null,
      args: [],
      culture: null) as JsonConverter;
  }
}
