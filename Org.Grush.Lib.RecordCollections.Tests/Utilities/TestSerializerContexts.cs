using System.Text.Json;
using System.Text.Json.Serialization;

namespace Org.Grush.Lib.RecordCollections.Tests.Utilities;


[JsonSourceGenerationOptions(Converters = [typeof(RecordCollectionStrictJsonConverter<string>)])]
[JsonSerializable(typeof(RecordCollection<string>))]
[JsonSerializable(typeof(string))]
internal partial class RecordCollectionOfStringContext : JsonSerializerContext;

[JsonSourceGenerationOptions(WriteIndented = true, AllowTrailingCommas = true, ReadCommentHandling = JsonCommentHandling.Skip, Converters = [typeof(RecordCollectionStrictJsonConverter<string>)])]
[JsonSerializable(typeof(RecordCollection<string>))]
[JsonSerializable(typeof(string))]
internal partial class RecordCollectionOfStringNonStandardContext : JsonSerializerContext;

[JsonSourceGenerationOptions(Converters = [typeof(RecordCollectionStrictJsonConverter<int>), typeof(RecordCollectionStrictJsonConverter<string>)])]
[JsonSerializable(typeof(TestRecord<RecordCollection<int>, RecordCollection<string>>))]
[JsonSerializable(typeof(RecordCollection<string>))]
[JsonSerializable(typeof(RecordCollection<int>))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(int))]
internal partial class TestRecordRecordCollectionIntStringContext : JsonSerializerContext;
