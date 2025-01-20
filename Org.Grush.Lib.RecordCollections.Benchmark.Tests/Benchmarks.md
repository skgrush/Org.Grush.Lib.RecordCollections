#Benchmarks
> (2025-01-20T11:25:03.9368920-06:00)


## Org.Grush.Lib.RecordCollections.Benchmark.Tests.CollectionExpressionInit-20250120-110226

```

BenchmarkDotNet v0.14.0, macOS Sequoia 15.2 (24C101) [Darwin 24.2.0]
Apple M1, 1 CPU, 8 logical and 8 physical cores
.NET SDK 8.0.301
  [Host]        : .NET 8.0.6 (8.0.624.26715), Arm64 RyuJIT AdvSIMD
  .NET 8.0      : .NET 8.0.6 (8.0.624.26715), Arm64 RyuJIT AdvSIMD
  NativeAOT 8.0 : .NET 8.0.6, Arm64 NativeAOT AdvSIMD


```
| Method                             | Job           | Runtime       | N  | Mean      | Error     | StdDev    |
|----------------------------------- |-------------- |-------------- |--- |----------:|----------:|----------:|
| ExpressionInit_IntList             | .NET 8.0      | .NET 8.0      | 32 | 21.899 ns | 0.1697 ns | 0.1587 ns |
| ExpressionInit_IntRecordCollection | .NET 8.0      | .NET 8.0      | 32 |  9.295 ns | 0.0215 ns | 0.0201 ns |
| ExpressionInit_IntArray            | .NET 8.0      | .NET 8.0      | 32 |  7.242 ns | 0.0145 ns | 0.0135 ns |
| ExpressionInit_IntList             | NativeAOT 8.0 | NativeAOT 8.0 | 32 | 21.710 ns | 0.1808 ns | 0.1691 ns |
| ExpressionInit_IntRecordCollection | NativeAOT 8.0 | NativeAOT 8.0 | 32 |  8.599 ns | 0.0196 ns | 0.0174 ns |
| ExpressionInit_IntArray            | NativeAOT 8.0 | NativeAOT 8.0 | 32 |  7.352 ns | 0.0326 ns | 0.0305 ns |


## Org.Grush.Lib.RecordCollections.Benchmark.Tests.ForEachIteration-20250120-110426

```

BenchmarkDotNet v0.14.0, macOS Sequoia 15.2 (24C101) [Darwin 24.2.0]
Apple M1, 1 CPU, 8 logical and 8 physical cores
.NET SDK 8.0.301
  [Host]        : .NET 8.0.6 (8.0.624.26715), Arm64 RyuJIT AdvSIMD
  .NET 8.0      : .NET 8.0.6 (8.0.624.26715), Arm64 RyuJIT AdvSIMD
  NativeAOT 8.0 : .NET 8.0.6, Arm64 NativeAOT AdvSIMD


```
| Method                   | Job           | Runtime       | N     | Mean         | Error      | StdDev       |
|------------------------- |-------------- |-------------- |------ |-------------:|-----------:|-------------:|
| **ForEach_List**             | **.NET 8.0**      | **.NET 8.0**      | **10**    |     **11.27 ns** |   **0.004 ns** |     **0.004 ns** |
| ForEach_RecordCollection | .NET 8.0      | .NET 8.0      | 10    |     29.40 ns |   0.234 ns |     0.219 ns |
| ForEach_Array            | .NET 8.0      | .NET 8.0      | 10    |     10.40 ns |   0.020 ns |     0.018 ns |
| ForEach_List             | NativeAOT 8.0 | NativeAOT 8.0 | 10    |     10.51 ns |   0.008 ns |     0.008 ns |
| ForEach_RecordCollection | NativeAOT 8.0 | NativeAOT 8.0 | 10    |     52.85 ns |   0.287 ns |     0.239 ns |
| ForEach_Array            | NativeAOT 8.0 | NativeAOT 8.0 | 10    |     10.13 ns |   0.091 ns |     0.086 ns |
| **ForEach_List**             | **.NET 8.0**      | **.NET 8.0**      | **1000**  |  **1,121.18 ns** |   **0.402 ns** |     **0.356 ns** |
| ForEach_RecordCollection | .NET 8.0      | .NET 8.0      | 1000  |  2,212.31 ns |  14.999 ns |    14.030 ns |
| ForEach_Array            | .NET 8.0      | .NET 8.0      | 1000  |  1,114.97 ns |   8.830 ns |     8.259 ns |
| ForEach_List             | NativeAOT 8.0 | NativeAOT 8.0 | 1000  |  1,410.09 ns |  12.266 ns |    11.474 ns |
| ForEach_RecordCollection | NativeAOT 8.0 | NativeAOT 8.0 | 1000  |  4,509.43 ns |   8.331 ns |     7.385 ns |
| ForEach_Array            | NativeAOT 8.0 | NativeAOT 8.0 | 1000  |  1,389.04 ns |  13.292 ns |    12.433 ns |
| **ForEach_List**             | **.NET 8.0**      | **.NET 8.0**      | **10000** | **11,160.03 ns** |   **7.247 ns** |     **6.779 ns** |
| ForEach_RecordCollection | .NET 8.0      | .NET 8.0      | 10000 | 21,705.34 ns |  33.403 ns |    26.079 ns |
| ForEach_Array            | .NET 8.0      | .NET 8.0      | 10000 | 11,186.56 ns |  93.137 ns |    87.120 ns |
| ForEach_List             | NativeAOT 8.0 | NativeAOT 8.0 | 10000 | 14,251.10 ns | 127.945 ns |   119.680 ns |
| ForEach_RecordCollection | NativeAOT 8.0 | NativeAOT 8.0 | 10000 | 44,000.57 ns | 861.737 ns | 1,179.556 ns |
| ForEach_Array            | NativeAOT 8.0 | NativeAOT 8.0 | 10000 | 14,007.17 ns |  15.862 ns |    12.384 ns |


## Org.Grush.Lib.RecordCollections.Benchmark.Tests.JsonDeserialization_NoContext-20250120-111020

```

BenchmarkDotNet v0.14.0, macOS Sequoia 15.2 (24C101) [Darwin 24.2.0]
Apple M1, 1 CPU, 8 logical and 8 physical cores
.NET SDK 8.0.301
  [Host]   : .NET 8.0.6 (8.0.624.26715), Arm64 RyuJIT AdvSIMD
  .NET 8.0 : .NET 8.0.6 (8.0.624.26715), Arm64 RyuJIT AdvSIMD

Job=.NET 8.0  Runtime=.NET 8.0  

```
| Method                                  | N   | Mean          | Error       | StdDev      |
|---------------------------------------- |---- |--------------:|------------:|------------:|
| **DynamicDeserialization_List**             | **10**  |      **6.066 μs** |   **0.0135 μs** |   **0.0113 μs** |
| DynamicDeserialization_RecordCollection | 10  |      5.108 μs |   0.0060 μs |   0.0053 μs |
| **DynamicDeserialization_List**             | **500** |  **9,095.927 μs** |  **71.6079 μs** |  **66.9821 μs** |
| DynamicDeserialization_RecordCollection | 500 | 11,346.510 μs | 155.6985 μs | 145.6405 μs |


## Org.Grush.Lib.RecordCollections.Benchmark.Tests.JsonDeserializationInteger-20250120-111129

```

BenchmarkDotNet v0.14.0, macOS Sequoia 15.2 (24C101) [Darwin 24.2.0]
Apple M1, 1 CPU, 8 logical and 8 physical cores
.NET SDK 8.0.301
  [Host]        : .NET 8.0.6 (8.0.624.26715), Arm64 RyuJIT AdvSIMD
  .NET 8.0      : .NET 8.0.6 (8.0.624.26715), Arm64 RyuJIT AdvSIMD
  NativeAOT 8.0 : .NET 8.0.6, Arm64 NativeAOT AdvSIMD


```
| Method                              | Job           | Runtime       | N     | Mean         | Error     | StdDev    |
|------------------------------------ |-------------- |-------------- |------ |-------------:|----------:|----------:|
| **Deserialization_IntList**             | **.NET 8.0**      | **.NET 8.0**      | **10**    |     **353.5 ns** |   **0.43 ns** |   **0.40 ns** |
| Deserialization_IntRecordCollection | .NET 8.0      | .NET 8.0      | 10    |     351.1 ns |   0.77 ns |   0.64 ns |
| Deserialization_IntArray            | .NET 8.0      | .NET 8.0      | 10    |     365.9 ns |   0.65 ns |   0.57 ns |
| Deserialization_IntList             | NativeAOT 8.0 | NativeAOT 8.0 | 10    |     458.6 ns |   0.13 ns |   0.12 ns |
| Deserialization_IntRecordCollection | NativeAOT 8.0 | NativeAOT 8.0 | 10    |     468.8 ns |   0.30 ns |   0.23 ns |
| Deserialization_IntArray            | NativeAOT 8.0 | NativeAOT 8.0 | 10    |     469.9 ns |   0.39 ns |   0.34 ns |
| **Deserialization_IntList**             | **.NET 8.0**      | **.NET 8.0**      | **1000**  |  **26,639.9 ns** |  **13.67 ns** |  **12.11 ns** |
| Deserialization_IntRecordCollection | .NET 8.0      | .NET 8.0      | 1000  |  25,797.8 ns |  13.51 ns |  11.98 ns |
| Deserialization_IntArray            | .NET 8.0      | .NET 8.0      | 1000  |  27,177.8 ns |  12.35 ns |  11.55 ns |
| Deserialization_IntList             | NativeAOT 8.0 | NativeAOT 8.0 | 1000  |  32,689.5 ns |  23.43 ns |  20.77 ns |
| Deserialization_IntRecordCollection | NativeAOT 8.0 | NativeAOT 8.0 | 1000  |  33,072.9 ns |  23.87 ns |  22.33 ns |
| Deserialization_IntArray            | NativeAOT 8.0 | NativeAOT 8.0 | 1000  |  32,419.0 ns |  47.71 ns |  37.25 ns |
| **Deserialization_IntList**             | **.NET 8.0**      | **.NET 8.0**      | **10000** | **270,774.6 ns** | **489.43 ns** | **433.87 ns** |
| Deserialization_IntRecordCollection | .NET 8.0      | .NET 8.0      | 10000 | 256,876.6 ns | 163.05 ns | 144.54 ns |
| Deserialization_IntArray            | .NET 8.0      | .NET 8.0      | 10000 | 271,003.8 ns | 159.02 ns | 148.75 ns |
| Deserialization_IntList             | NativeAOT 8.0 | NativeAOT 8.0 | 10000 | 324,697.5 ns | 136.40 ns | 127.59 ns |
| Deserialization_IntRecordCollection | NativeAOT 8.0 | NativeAOT 8.0 | 10000 | 328,631.8 ns | 269.49 ns | 252.08 ns |
| Deserialization_IntArray            | NativeAOT 8.0 | NativeAOT 8.0 | 10000 | 322,175.2 ns | 174.89 ns | 136.54 ns |


## Org.Grush.Lib.RecordCollections.Benchmark.Tests.JsonDeserializationObject-20250120-111705

```

BenchmarkDotNet v0.14.0, macOS Sequoia 15.2 (24C101) [Darwin 24.2.0]
Apple M1, 1 CPU, 8 logical and 8 physical cores
.NET SDK 8.0.301
  [Host]        : .NET 8.0.6 (8.0.624.26715), Arm64 RyuJIT AdvSIMD
  .NET 8.0      : .NET 8.0.6 (8.0.624.26715), Arm64 RyuJIT AdvSIMD
  NativeAOT 8.0 : .NET 8.0.6, Arm64 NativeAOT AdvSIMD


```
| Method                                        | Job           | Runtime       | N     | Mean         | Error      | StdDev     |
|---------------------------------------------- |-------------- |-------------- |------ |-------------:|-----------:|-----------:|
| **Deserialization_NumberObject_List**             | **.NET 8.0**      | **.NET 8.0**      | **10**    |     **3.302 μs** |  **0.0021 μs** |  **0.0020 μs** |
| Deserialization_NumberObject_RecordCollection | .NET 8.0      | .NET 8.0      | 10    |     3.053 μs |  0.0013 μs |  0.0012 μs |
| Deserialization_NumberObject_Array            | .NET 8.0      | .NET 8.0      | 10    |     3.313 μs |  0.0050 μs |  0.0042 μs |
| Deserialization_NumberObject_List             | NativeAOT 8.0 | NativeAOT 8.0 | 10    |     4.296 μs |  0.0076 μs |  0.0063 μs |
| Deserialization_NumberObject_RecordCollection | NativeAOT 8.0 | NativeAOT 8.0 | 10    |     4.294 μs |  0.0046 μs |  0.0043 μs |
| Deserialization_NumberObject_Array            | NativeAOT 8.0 | NativeAOT 8.0 | 10    |     4.336 μs |  0.0039 μs |  0.0034 μs |
| **Deserialization_NumberObject_List**             | **.NET 8.0**      | **.NET 8.0**      | **1000**  |   **312.203 μs** |  **0.4418 μs** |  **0.3916 μs** |
| Deserialization_NumberObject_RecordCollection | .NET 8.0      | .NET 8.0      | 1000  |   288.857 μs |  0.2178 μs |  0.2037 μs |
| Deserialization_NumberObject_Array            | .NET 8.0      | .NET 8.0      | 1000  |   314.953 μs |  0.4350 μs |  0.4069 μs |
| Deserialization_NumberObject_List             | NativeAOT 8.0 | NativeAOT 8.0 | 1000  |   411.129 μs |  0.2107 μs |  0.1971 μs |
| Deserialization_NumberObject_RecordCollection | NativeAOT 8.0 | NativeAOT 8.0 | 1000  |   405.977 μs |  1.3161 μs |  1.2311 μs |
| Deserialization_NumberObject_Array            | NativeAOT 8.0 | NativeAOT 8.0 | 1000  |   409.426 μs |  0.9219 μs |  0.8624 μs |
| **Deserialization_NumberObject_List**             | **.NET 8.0**      | **.NET 8.0**      | **10000** | **4,084.100 μs** | **12.1368 μs** | **11.3527 μs** |
| Deserialization_NumberObject_RecordCollection | .NET 8.0      | .NET 8.0      | 10000 | 3,670.351 μs | 72.8732 μs | 94.7558 μs |
| Deserialization_NumberObject_Array            | .NET 8.0      | .NET 8.0      | 10000 | 3,850.593 μs | 10.2622 μs |  8.5694 μs |
| Deserialization_NumberObject_List             | NativeAOT 8.0 | NativeAOT 8.0 | 10000 | 5,099.656 μs | 14.6562 μs | 12.2386 μs |
| Deserialization_NumberObject_RecordCollection | NativeAOT 8.0 | NativeAOT 8.0 | 10000 | 5,034.029 μs | 86.1834 μs | 80.6160 μs |
| Deserialization_NumberObject_Array            | NativeAOT 8.0 | NativeAOT 8.0 | 10000 | 5,101.824 μs | 65.0126 μs | 60.8128 μs |


## Org.Grush.Lib.RecordCollections.Benchmark.Tests.SelectToList-20250120-112227

```

BenchmarkDotNet v0.14.0, macOS Sequoia 15.2 (24C101) [Darwin 24.2.0]
Apple M1, 1 CPU, 8 logical and 8 physical cores
.NET SDK 8.0.301
  [Host]   : .NET 8.0.6 (8.0.624.26715), Arm64 RyuJIT AdvSIMD
  .NET 8.0 : .NET 8.0.6 (8.0.624.26715), Arm64 RyuJIT AdvSIMD

Job=.NET 8.0  Runtime=.NET 8.0  

```
| Method                       | Length | Mean        | Error     | StdDev    |
|----------------------------- |------- |------------:|----------:|----------:|
| **ListSelectToList**             | **10**     |    **32.76 ns** |  **0.108 ns** |  **0.090 ns** |
| RecordCollectionSelectToList | 10     |    39.05 ns |  0.041 ns |  0.036 ns |
| ListSelectToRecordCollection | 10     |    39.93 ns |  0.113 ns |  0.106 ns |
| **ListSelectToList**             | **1000**   |   **602.47 ns** |  **3.924 ns** |  **3.670 ns** |
| RecordCollectionSelectToList | 1000   |   703.04 ns |  1.549 ns |  1.449 ns |
| ListSelectToRecordCollection | 1000   |   577.41 ns |  3.185 ns |  2.980 ns |
| **ListSelectToList**             | **10000**  | **5,610.32 ns** | **19.029 ns** | **16.869 ns** |
| RecordCollectionSelectToList | 10000  | 6,250.70 ns | 50.559 ns | 44.819 ns |
| ListSelectToRecordCollection | 10000  | 5,523.30 ns | 53.139 ns | 49.707 ns |
