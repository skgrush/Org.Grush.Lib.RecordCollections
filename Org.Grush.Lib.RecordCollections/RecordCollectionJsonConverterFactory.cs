using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Org.Grush.Lib.RecordCollections;

/// <summary>
/// Dynamically resolve <see cref="RecordCollectionStrictJsonConverter{T}"/>s
/// if not in AOT mode.
/// </summary>
#if NET8_0_OR_GREATER
[System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Dynamically references generic RecordCollection<> for determining if factory can deserialize type.")]
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
      typeof(RecordCollectionStrictJsonConverter<>).MakeGenericType(
        elementType
      ),
      BindingFlags.Instance | BindingFlags.Public,
      binder: null,
      args: [],
      culture: null) as JsonConverter;
  }
}
