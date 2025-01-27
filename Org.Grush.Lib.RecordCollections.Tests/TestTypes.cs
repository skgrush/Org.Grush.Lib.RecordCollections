
global using TestRecordOfInts = Org.Grush.Lib.RecordCollections.Tests.TestRecord<int, int>;
global using TestRecordOfCollections = Org.Grush.Lib.RecordCollections.Tests.TestRecord<Org.Grush.Lib.RecordCollections.RecordCollection<int>, Org.Grush.Lib.RecordCollections.RecordCollection<int>>;

namespace Org.Grush.Lib.RecordCollections.Tests;

public record TestRecord<TA, TB>(TA A, TB B);
