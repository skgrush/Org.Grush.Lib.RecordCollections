# Benchmarks
```
Finished: 2025-01-21T23:38:12.4861650-06:00
Elapsed: 00:24:02.1762191
```


## Initializing the type with a collection expression
### e.g. `= [1,2,3,4,.......,31,32]`
Class: Org.Grush.Lib.RecordCollections.Benchmark.Tests.CollectionExpressionInit

```

BenchmarkDotNet v0.14.0, macOS Sequoia 15.2 (24C101) [Darwin 24.2.0]
Apple M1, 1 CPU, 8 logical and 8 physical cores
.NET SDK 8.0.301
  [Host]               : .NET 8.0.6 (8.0.624.26715), Arm64 RyuJIT AdvSIMD
  CollectionExpression : .NET 8.0.6 (8.0.624.26715), Arm64 RyuJIT AdvSIMD

Job=CollectionExpression  

```
| Method                     | Runtime       | N  | Mean      | Error     | StdDev    | Ratio | Gen0   | Allocated | Alloc Ratio |
|--------------------------- |-------------- |--- |----------:|----------:|----------:|------:|-------:|----------:|------------:|
| ListOfIntegers             | .NET 8.0      | 32 | 22.162 ns | 0.0668 ns | 0.0625 ns |  1.00 | 0.0293 |     184 B |        1.00 |
| RecordCollectionOfIntegers | .NET 8.0      | 32 |  9.112 ns | 0.0127 ns | 0.0112 ns |  0.41 | 0.0242 |     152 B |        0.83 |
| ArrayOfIntegers            | .NET 8.0      | 32 |  7.230 ns | 0.0253 ns | 0.0237 ns |  0.33 | 0.0242 |     152 B |        0.83 |
|                            |               |    |           |           |           |       |        |           |             |
| ListOfIntegers             | NativeAOT 8.0 | 32 | 21.970 ns | 0.0392 ns | 0.0366 ns |  1.00 | 0.0293 |     184 B |        1.00 |
| RecordCollectionOfIntegers | NativeAOT 8.0 | 32 |  8.784 ns | 0.0101 ns | 0.0090 ns |  0.40 | 0.0242 |     152 B |        0.83 |
| ArrayOfIntegers            | NativeAOT 8.0 | 32 |  7.458 ns | 0.0096 ns | 0.0085 ns |  0.34 | 0.0242 |     152 B |        0.83 |


## `foreach` loop enumeration
### Iterate over a pre-built collection of `N` integers using `foreach`.
Class: Org.Grush.Lib.RecordCollections.Benchmark.Tests.ForEachIteration

```

BenchmarkDotNet v0.14.0, macOS Sequoia 15.2 (24C101) [Darwin 24.2.0]
Apple M1, 1 CPU, 8 logical and 8 physical cores
.NET SDK 8.0.301
  [Host]  : .NET 8.0.6 (8.0.624.26715), Arm64 RyuJIT AdvSIMD
  ForEach : .NET 8.0.6 (8.0.624.26715), Arm64 RyuJIT AdvSIMD

Job=ForEach  

```
| Method                     | Runtime       | N     | Mean         | Error     | StdDev    | Ratio | Allocated | Alloc Ratio |
|--------------------------- |-------------- |------ |-------------:|----------:|----------:|------:|----------:|------------:|
| **ListOfIntegers**             | **.NET 8.0**      | **10**    |     **11.29 ns** |  **0.009 ns** |  **0.008 ns** |  **1.00** |         **-** |          **NA** |
| RecordCollectionOfIntegers | .NET 8.0      | 10    |     10.41 ns |  0.012 ns |  0.010 ns |  0.92 |         - |          NA |
| ArrayOfIntegers            | .NET 8.0      | 10    |     10.42 ns |  0.015 ns |  0.014 ns |  0.92 |         - |          NA |
|                            |               |       |              |           |           |       |           |             |
| ListOfIntegers             | NativeAOT 8.0 | 10    |     10.51 ns |  0.012 ns |  0.011 ns |  1.00 |         - |          NA |
| RecordCollectionOfIntegers | NativeAOT 8.0 | 10    |     10.12 ns |  0.018 ns |  0.017 ns |  0.96 |         - |          NA |
| ArrayOfIntegers            | NativeAOT 8.0 | 10    |     10.10 ns |  0.019 ns |  0.017 ns |  0.96 |         - |          NA |
|                            |               |       |              |           |           |       |           |             |
| **ListOfIntegers**             | **.NET 8.0**      | **1000**  |  **1,120.24 ns** |  **1.301 ns** |  **1.154 ns** |  **1.00** |         **-** |          **NA** |
| RecordCollectionOfIntegers | .NET 8.0      | 1000  |  1,123.49 ns |  2.106 ns |  1.758 ns |  1.00 |         - |          NA |
| ArrayOfIntegers            | .NET 8.0      | 1000  |  1,123.03 ns |  2.149 ns |  1.905 ns |  1.00 |         - |          NA |
|                            |               |       |              |           |           |       |           |             |
| ListOfIntegers             | NativeAOT 8.0 | 1000  |  1,415.77 ns |  1.729 ns |  1.617 ns |  1.00 |         - |          NA |
| RecordCollectionOfIntegers | NativeAOT 8.0 | 1000  |  1,385.79 ns |  1.546 ns |  1.370 ns |  0.98 |         - |          NA |
| ArrayOfIntegers            | NativeAOT 8.0 | 1000  |  1,384.64 ns |  1.211 ns |  1.133 ns |  0.98 |         - |          NA |
|                            |               |       |              |           |           |       |           |             |
| **ListOfIntegers**             | **.NET 8.0**      | **10000** | **11,140.88 ns** | **21.739 ns** | **20.335 ns** |  **1.00** |         **-** |          **NA** |
| RecordCollectionOfIntegers | .NET 8.0      | 10000 | 11,246.24 ns | 32.533 ns | 28.839 ns |  1.01 |         - |          NA |
| ArrayOfIntegers            | .NET 8.0      | 10000 | 11,224.97 ns | 18.193 ns | 17.018 ns |  1.01 |         - |          NA |
|                            |               |       |              |           |           |       |           |             |
| ListOfIntegers             | NativeAOT 8.0 | 10000 | 14,269.52 ns | 12.879 ns | 12.047 ns |  1.00 |         - |          NA |
| RecordCollectionOfIntegers | NativeAOT 8.0 | 10000 | 14,059.82 ns | 13.969 ns | 12.383 ns |  0.99 |         - |          NA |
| ArrayOfIntegers            | NativeAOT 8.0 | 10000 | 14,056.19 ns | 19.480 ns | 17.268 ns |  0.99 |         - |          NA |


## System.Text.Json Deserialization - Source-generated, integers
### Deserialize integer collections using explicit `JsonSerializerContext`s.
Class: Org.Grush.Lib.RecordCollections.Benchmark.Tests.JsonDeserializationInteger

```

BenchmarkDotNet v0.14.0, macOS Sequoia 15.2 (24C101) [Darwin 24.2.0]
Apple M1, 1 CPU, 8 logical and 8 physical cores
.NET SDK 8.0.301
  [Host]                     : .NET 8.0.6 (8.0.624.26715), Arm64 RyuJIT AdvSIMD
  JsonDeserializationInteger : .NET 8.0.6 (8.0.624.26715), Arm64 RyuJIT AdvSIMD

Job=JsonDeserializationInteger  

```
| Method              | Runtime       | N     | Mean         | Error     | StdDev    | Ratio | Gen0    | Gen1   | Allocated | Alloc Ratio |
|-------------------- |-------------- |------ |-------------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **IntList**             | **.NET 8.0**      | **10**    |     **373.4 ns** |   **1.27 ns** |   **1.19 ns** |  **1.00** |  **0.0343** |      **-** |     **216 B** |        **1.00** |
| IntRecordCollection | .NET 8.0      | 10    |     348.9 ns |   0.42 ns |   0.37 ns |  0.93 |  0.0443 |      - |     280 B |        1.30 |
| IntArray            | .NET 8.0      | 10    |     354.9 ns |   0.29 ns |   0.25 ns |  0.95 |  0.0443 |      - |     280 B |        1.30 |
|                     |               |       |              |           |           |       |         |        |           |             |
| IntList             | NativeAOT 8.0 | 10    |     459.6 ns |   0.58 ns |   0.54 ns |  1.00 |  0.0343 |      - |     216 B |        1.00 |
| IntRecordCollection | NativeAOT 8.0 | 10    |     477.8 ns |   1.17 ns |   1.04 ns |  1.04 |  0.0443 |      - |     280 B |        1.30 |
| IntArray            | NativeAOT 8.0 | 10    |     485.5 ns |   0.88 ns |   0.73 ns |  1.06 |  0.0439 |      - |     280 B |        1.30 |
|                     |               |       |              |           |           |       |         |        |           |             |
| **IntList**             | **.NET 8.0**      | **1000**  |  **26,268.1 ns** |  **23.54 ns** |  **20.87 ns** |  **1.00** |  **1.3123** |      **-** |    **8425 B** |        **1.00** |
| IntRecordCollection | .NET 8.0      | 1000  |  26,102.2 ns |  12.10 ns |  11.32 ns |  0.99 |  1.9836 |      - |   12449 B |        1.48 |
| IntArray            | .NET 8.0      | 1000  |  26,467.9 ns |  31.77 ns |  29.71 ns |  1.01 |  1.9836 |      - |   12449 B |        1.48 |
|                     |               |       |              |           |           |       |         |        |           |             |
| IntList             | NativeAOT 8.0 | 1000  |  32,892.6 ns |  24.67 ns |  23.08 ns |  1.00 |  1.2817 |      - |    8426 B |        1.00 |
| IntRecordCollection | NativeAOT 8.0 | 1000  |  33,441.1 ns |  28.19 ns |  26.37 ns |  1.02 |  1.9531 |      - |   12450 B |        1.48 |
| IntArray            | NativeAOT 8.0 | 1000  |  32,667.2 ns |  29.41 ns |  27.51 ns |  0.99 |  1.9531 |      - |   12450 B |        1.48 |
|                     |               |       |              |           |           |       |         |        |           |             |
| **IntList**             | **.NET 8.0**      | **10000** | **261,209.1 ns** | **355.68 ns** | **315.30 ns** |  **1.00** | **20.5078** |      **-** |  **131656 B** |        **1.00** |
| IntRecordCollection | .NET 8.0      | 10000 | 257,067.0 ns | 393.74 ns | 368.30 ns |  0.98 | 26.8555 | 5.3711 |  171680 B |        1.30 |
| IntArray            | .NET 8.0      | 10000 | 262,939.6 ns | 442.54 ns | 392.30 ns |  1.01 | 26.8555 | 5.3711 |  171680 B |        1.30 |
|                     |               |       |              |           |           |       |         |        |           |             |
| IntList             | NativeAOT 8.0 | 10000 | 324,851.6 ns | 280.43 ns | 262.31 ns |  1.00 | 20.5078 | 3.9063 |  131656 B |        1.00 |
| IntRecordCollection | NativeAOT 8.0 | 10000 | 334,511.0 ns | 249.96 ns | 221.58 ns |  1.03 | 26.8555 | 5.3711 |  171680 B |        1.30 |
| IntArray            | NativeAOT 8.0 | 10000 | 322,955.2 ns | 266.28 ns | 249.08 ns |  0.99 | 26.8555 | 5.3711 |  171680 B |        1.30 |


## System.Text.Json Deserialization - Source-generated, record of three types.
### Deserialize `(int, double, string)` collections using explicit `JsonSerializerContext`s.
Class: Org.Grush.Lib.RecordCollections.Benchmark.Tests.JsonDeserializationObject

```

BenchmarkDotNet v0.14.0, macOS Sequoia 15.2 (24C101) [Darwin 24.2.0]
Apple M1, 1 CPU, 8 logical and 8 physical cores
.NET SDK 8.0.301
  [Host]                    : .NET 8.0.6 (8.0.624.26715), Arm64 RyuJIT AdvSIMD
  JsonDeserializationObject : .NET 8.0.6 (8.0.624.26715), Arm64 RyuJIT AdvSIMD

Job=JsonDeserializationObject  

```
| Method                        | Runtime       | N     | Mean         | Error      | StdDev     | Ratio | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------------------------ |-------------- |------ |-------------:|-----------:|-----------:|------:|---------:|---------:|---------:|-----------:|------------:|
| **NumberObject_List**             | **.NET 8.0**      | **10**    |     **3.324 μs** |  **0.0016 μs** |  **0.0015 μs** |  **1.00** |   **0.4272** |        **-** |        **-** |    **2.63 KB** |        **1.00** |
| NumberObject_RecordCollection | .NET 8.0      | 10    |     3.074 μs |  0.0022 μs |  0.0019 μs |  0.92 |   0.3738 |        - |        - |     2.3 KB |        0.88 |
| NumberObject_Array            | .NET 8.0      | 10    |     3.322 μs |  0.0013 μs |  0.0011 μs |  1.00 |   0.4425 |        - |        - |    2.73 KB |        1.04 |
|                               |               |       |              |            |            |       |          |          |          |            |             |
| NumberObject_List             | NativeAOT 8.0 | 10    |     4.131 μs |  0.0206 μs |  0.0193 μs |  1.00 |   0.4272 |        - |        - |    2.63 KB |        1.00 |
| NumberObject_RecordCollection | NativeAOT 8.0 | 10    |     4.071 μs |  0.0063 μs |  0.0059 μs |  0.99 |   0.3738 |        - |        - |     2.3 KB |        0.88 |
| NumberObject_Array            | NativeAOT 8.0 | 10    |     4.148 μs |  0.0207 μs |  0.0193 μs |  1.00 |   0.4425 |        - |        - |    2.73 KB |        1.04 |
|                               |               |       |              |            |            |       |          |          |          |            |             |
| **NumberObject_List**             | **.NET 8.0**      | **1000**  |   **313.537 μs** |  **0.5979 μs** |  **0.5300 μs** |  **1.00** |  **33.2031** |   **7.8125** |        **-** |  **204.27 KB** |        **1.00** |
| NumberObject_RecordCollection | .NET 8.0      | 1000  |   289.083 μs |  0.1725 μs |  0.1529 μs |  0.92 |  34.1797 |   7.3242 |        - |  211.67 KB |        1.04 |
| NumberObject_Array            | .NET 8.0      | 1000  |   312.730 μs |  0.1148 μs |  0.1018 μs |  1.00 |  34.1797 |   6.8359 |        - |   212.1 KB |        1.04 |
|                               |               |       |              |            |            |       |          |          |          |            |             |
| NumberObject_List             | NativeAOT 8.0 | 1000  |   394.230 μs |  1.2400 μs |  1.1599 μs |  1.00 |  33.2031 |   8.3008 |        - |  204.26 KB |        1.00 |
| NumberObject_RecordCollection | NativeAOT 8.0 | 1000  |   389.367 μs |  0.4757 μs |  0.4450 μs |  0.99 |  34.1797 |   7.3242 |        - |  211.67 KB |        1.04 |
| NumberObject_Array            | NativeAOT 8.0 | 1000  |   390.122 μs |  1.7996 μs |  1.6833 μs |  0.99 |  34.1797 |   6.8359 |        - |   212.1 KB |        1.04 |
|                               |               |       |              |            |            |       |          |          |          |            |             |
| **NumberObject_List**             | **.NET 8.0**      | **10000** | **3,908.718 μs** |  **6.9117 μs** |  **6.1271 μs** |  **1.00** | **492.1875** | **328.1250** | **164.0625** | **2685.34 KB** |        **1.00** |
| NumberObject_RecordCollection | .NET 8.0      | 10000 | 3,619.086 μs | 10.7997 μs | 10.1020 μs |  0.93 | 507.8125 | 335.9375 | 167.9688 | 2763.33 KB |        1.03 |
| NumberObject_Array            | .NET 8.0      | 10000 | 3,880.507 μs |  4.6815 μs |  4.1500 μs |  0.99 | 507.8125 | 335.9375 | 167.9688 | 2763.62 KB |        1.03 |
|                               |               |       |              |            |            |       |          |          |          |            |             |
| NumberObject_List             | NativeAOT 8.0 | 10000 | 4,825.922 μs |  8.9759 μs |  8.3961 μs |  1.00 | 484.3750 | 328.1250 | 164.0625 | 2685.69 KB |        1.00 |
| NumberObject_RecordCollection | NativeAOT 8.0 | 10000 | 4,642.236 μs | 25.8496 μs | 24.1798 μs |  0.96 | 500.0000 | 328.1250 | 164.0625 | 2763.52 KB |        1.03 |
| NumberObject_Array            | NativeAOT 8.0 | 10000 | 4,776.750 μs | 12.4353 μs | 11.6320 μs |  0.99 | 500.0000 | 328.1250 | 164.0625 | 2763.95 KB |        1.03 |


## System.Text.Json Deserialization - Dynamic
### Deserialize collections using automatically-selected factories and converters.
Class: Org.Grush.Lib.RecordCollections.Benchmark.Tests.JsonDynamicDeserialization

```

BenchmarkDotNet v0.14.0, macOS Sequoia 15.2 (24C101) [Darwin 24.2.0]
Apple M1, 1 CPU, 8 logical and 8 physical cores
.NET SDK 8.0.301
  [Host]   : .NET 8.0.6 (8.0.624.26715), Arm64 RyuJIT AdvSIMD
  .NET 8.0 : .NET 8.0.6 (8.0.624.26715), Arm64 RyuJIT AdvSIMD

Job=.NET 8.0  Runtime=.NET 8.0  

```
| Method                                    | N   | Mean          | Error      | StdDev     | Ratio | Gen0      | Gen1     | Gen2     | Allocated   | Alloc Ratio |
|------------------------------------------ |---- |--------------:|-----------:|-----------:|------:|----------:|---------:|---------:|------------:|------------:|
| **ListOfDtosOfLists**                         | **10**  |      **6.192 μs** |  **0.0111 μs** |  **0.0104 μs** |  **1.00** |    **0.9232** |        **-** |        **-** |      **5.7 KB** |        **1.00** |
| RecordCollectionOfDtosOfRecordCollections | 10  |      5.118 μs |  0.0029 μs |  0.0024 μs |  0.83 |    1.0300 |        - |        - |     6.34 KB |        1.11 |
|                                           |     |               |            |            |       |           |          |          |             |             |
| **ListOfDtosOfLists**                         | **500** |  **9,658.596 μs** | **90.8350 μs** | **84.9672 μs** |  **1.00** | **1562.5000** | **906.2500** | **312.5000** |  **9255.57 KB** |        **1.00** |
| RecordCollectionOfDtosOfRecordCollections | 500 | 11,861.105 μs | 26.2926 μs | 24.5941 μs |  1.23 | 1796.8750 | 875.0000 | 359.3750 | 10745.42 KB |        1.16 |


## LINQ iteration
### Iterate of a prebuilt collection with `.Select(x => x).ToList();`
Class: Org.Grush.Lib.RecordCollections.Benchmark.Tests.SelectToList

```

BenchmarkDotNet v0.14.0, macOS Sequoia 15.2 (24C101) [Darwin 24.2.0]
Apple M1, 1 CPU, 8 logical and 8 physical cores
.NET SDK 8.0.301
  [Host]   : .NET 8.0.6 (8.0.624.26715), Arm64 RyuJIT AdvSIMD
  .NET 8.0 : .NET 8.0.6 (8.0.624.26715), Arm64 RyuJIT AdvSIMD

Job=.NET 8.0  Runtime=.NET 8.0  

```
| Method                       | Length | Mean        | Error     | StdDev    | Ratio | Gen0   | Allocated | Alloc Ratio |
|----------------------------- |------- |------------:|----------:|----------:|------:|-------:|----------:|------------:|
| **ListSelectToList**             | **10**     |    **33.16 ns** |  **0.053 ns** |  **0.044 ns** |  **1.00** | **0.0268** |     **168 B** |        **1.00** |
| RecordCollectionSelectToList | 10     |    38.83 ns |  0.026 ns |  0.024 ns |  1.17 | 0.0280 |     176 B |        1.05 |
| ListSelectToRecordCollection | 10     |    40.02 ns |  0.098 ns |  0.092 ns |  1.21 | 0.0216 |     136 B |        0.81 |
|                              |        |             |           |           |       |        |           |             |
| **ListSelectToList**             | **1000**   |   **594.86 ns** |  **1.334 ns** |  **1.247 ns** |  **1.00** | **0.6571** |    **4128 B** |        **1.00** |
| RecordCollectionSelectToList | 1000   |   702.20 ns |  0.757 ns |  0.708 ns |  1.18 | 0.6590 |    4136 B |        1.00 |
| ListSelectToRecordCollection | 1000   |   579.15 ns |  1.238 ns |  1.098 ns |  0.97 | 0.6523 |    4096 B |        0.99 |
|                              |        |             |           |           |       |        |           |             |
| **ListSelectToList**             | **10000**  | **5,623.77 ns** | **32.427 ns** | **30.332 ns** |  **1.00** | **6.3629** |   **40128 B** |        **1.00** |
| RecordCollectionSelectToList | 10000  | 6,235.55 ns | 17.952 ns | 16.792 ns |  1.11 | 6.3629 |   40136 B |        1.00 |
| ListSelectToRecordCollection | 10000  | 5,549.50 ns | 28.133 ns | 26.316 ns |  0.99 | 6.3629 |   40096 B |        1.00 |
