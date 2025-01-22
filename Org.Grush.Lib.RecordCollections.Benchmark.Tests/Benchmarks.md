# Benchmarks
> (2025-01-21T21:30:35.7418220-06:00)


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
| Method                     | Runtime       | N  | Mean      | Error     | StdDev    | Gen0   | Allocated |
|--------------------------- |-------------- |--- |----------:|----------:|----------:|-------:|----------:|
| ListOfIntegers             | .NET 8.0      | 32 | 22.187 ns | 0.0352 ns | 0.0312 ns | 0.0293 |     184 B |
| RecordCollectionOfIntegers | .NET 8.0      | 32 |  9.125 ns | 0.0346 ns | 0.0324 ns | 0.0242 |     152 B |
| ArrayOfIntegers            | .NET 8.0      | 32 |  7.285 ns | 0.0927 ns | 0.0867 ns | 0.0242 |     152 B |
| ListOfIntegers             | NativeAOT 8.0 | 32 | 21.976 ns | 0.0289 ns | 0.0270 ns | 0.0293 |     184 B |
| RecordCollectionOfIntegers | NativeAOT 8.0 | 32 |  8.847 ns | 0.0340 ns | 0.0318 ns | 0.0242 |     152 B |
| ArrayOfIntegers            | NativeAOT 8.0 | 32 |  7.308 ns | 0.0232 ns | 0.0217 ns | 0.0242 |     152 B |


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
| Method                     | Runtime       | N     | Mean         | Error     | StdDev    | Allocated |
|--------------------------- |-------------- |------ |-------------:|----------:|----------:|----------:|
| **ListOfIntegers**             | **.NET 8.0**      | **10**    |     **11.27 ns** |  **0.011 ns** |  **0.010 ns** |         **-** |
| RecordCollectionOfIntegers | .NET 8.0      | 10    |     10.38 ns |  0.020 ns |  0.019 ns |         - |
| ArrayOfIntegers            | .NET 8.0      | 10    |     10.38 ns |  0.022 ns |  0.021 ns |         - |
| ListOfIntegers             | NativeAOT 8.0 | 10    |     10.49 ns |  0.016 ns |  0.015 ns |         - |
| RecordCollectionOfIntegers | NativeAOT 8.0 | 10    |     10.10 ns |  0.016 ns |  0.015 ns |         - |
| ArrayOfIntegers            | NativeAOT 8.0 | 10    |     10.10 ns |  0.018 ns |  0.016 ns |         - |
| **ListOfIntegers**             | **.NET 8.0**      | **1000**  |  **1,119.67 ns** |  **1.513 ns** |  **1.415 ns** |         **-** |
| RecordCollectionOfIntegers | .NET 8.0      | 1000  |  1,122.78 ns |  1.901 ns |  1.778 ns |         - |
| ArrayOfIntegers            | .NET 8.0      | 1000  |  1,121.14 ns |  2.089 ns |  1.954 ns |         - |
| ListOfIntegers             | NativeAOT 8.0 | 1000  |  1,415.40 ns |  2.323 ns |  2.173 ns |         - |
| RecordCollectionOfIntegers | NativeAOT 8.0 | 1000  |  1,387.49 ns |  1.688 ns |  1.579 ns |         - |
| ArrayOfIntegers            | NativeAOT 8.0 | 1000  |  1,385.02 ns |  1.068 ns |  0.999 ns |         - |
| **ListOfIntegers**             | **.NET 8.0**      | **10000** | **11,134.49 ns** | **14.106 ns** | **13.194 ns** |         **-** |
| RecordCollectionOfIntegers | .NET 8.0      | 10000 | 11,225.89 ns | 22.729 ns | 21.261 ns |         - |
| ArrayOfIntegers            | .NET 8.0      | 10000 | 11,223.57 ns | 22.880 ns | 20.283 ns |         - |
| ListOfIntegers             | NativeAOT 8.0 | 10000 | 14,286.88 ns | 18.849 ns | 17.632 ns |         - |
| RecordCollectionOfIntegers | NativeAOT 8.0 | 10000 | 14,095.40 ns | 16.274 ns | 15.223 ns |         - |
| ArrayOfIntegers            | NativeAOT 8.0 | 10000 | 14,062.13 ns | 12.096 ns | 10.723 ns |         - |


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
| Method              | Runtime       | N     | Mean         | Error     | StdDev    | Gen0    | Gen1   | Allocated |
|-------------------- |-------------- |------ |-------------:|----------:|----------:|--------:|-------:|----------:|
| **IntList**             | **.NET 8.0**      | **10**    |     **357.5 ns** |   **0.48 ns** |   **0.45 ns** |  **0.0343** |      **-** |     **216 B** |
| IntRecordCollection | .NET 8.0      | 10    |     341.6 ns |   0.55 ns |   0.52 ns |  0.0443 |      - |     280 B |
| IntArray            | .NET 8.0      | 10    |     370.0 ns |   0.39 ns |   0.34 ns |  0.0443 |      - |     280 B |
| IntList             | NativeAOT 8.0 | 10    |     461.8 ns |   0.50 ns |   0.47 ns |  0.0343 |      - |     216 B |
| IntRecordCollection | NativeAOT 8.0 | 10    |     475.6 ns |   0.50 ns |   0.44 ns |  0.0439 |      - |     280 B |
| IntArray            | NativeAOT 8.0 | 10    |     495.8 ns |   2.84 ns |   2.52 ns |  0.0439 |      - |     280 B |
| **IntList**             | **.NET 8.0**      | **1000**  |  **26,338.3 ns** | **179.57 ns** | **149.95 ns** |  **1.3123** |      **-** |    **8425 B** |
| IntRecordCollection | .NET 8.0      | 1000  |  25,857.7 ns |  18.83 ns |  17.61 ns |  1.9836 |      - |   12449 B |
| IntArray            | .NET 8.0      | 1000  |  26,576.1 ns |  26.03 ns |  23.08 ns |  1.9836 |      - |   12449 B |
| IntList             | NativeAOT 8.0 | 1000  |  32,650.3 ns |  16.13 ns |  14.30 ns |  1.2817 |      - |    8426 B |
| IntRecordCollection | NativeAOT 8.0 | 1000  |  33,322.8 ns |  24.04 ns |  22.49 ns |  1.9531 |      - |   12450 B |
| IntArray            | NativeAOT 8.0 | 1000  |  32,672.7 ns |  23.67 ns |  19.77 ns |  1.9531 |      - |   12450 B |
| **IntList**             | **.NET 8.0**      | **10000** | **261,267.4 ns** | **190.76 ns** | **169.11 ns** | **20.5078** |      **-** |  **131656 B** |
| IntRecordCollection | .NET 8.0      | 10000 | 258,331.2 ns | 186.14 ns | 165.01 ns | 26.8555 | 5.3711 |  171680 B |
| IntArray            | .NET 8.0      | 10000 | 264,799.6 ns | 342.20 ns | 320.09 ns | 26.8555 | 5.3711 |  171680 B |
| IntList             | NativeAOT 8.0 | 10000 | 325,118.1 ns | 283.02 ns | 264.74 ns | 20.5078 | 3.9063 |  131656 B |
| IntRecordCollection | NativeAOT 8.0 | 10000 | 329,834.1 ns | 302.30 ns | 267.98 ns | 26.8555 | 5.3711 |  171680 B |
| IntArray            | NativeAOT 8.0 | 10000 | 323,044.6 ns | 127.64 ns | 113.15 ns | 26.8555 | 5.3711 |  171680 B |


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
| Method                        | Runtime       | N     | Mean         | Error      | StdDev     | Gen0     | Gen1     | Gen2     | Allocated  |
|------------------------------ |-------------- |------ |-------------:|-----------:|-----------:|---------:|---------:|---------:|-----------:|
| **NumberObject_List**             | **.NET 8.0**      | **10**    |     **3.540 μs** |  **0.0020 μs** |  **0.0017 μs** |   **0.4272** |        **-** |        **-** |    **2.63 KB** |
| NumberObject_RecordCollection | .NET 8.0      | 10    |     3.054 μs |  0.0035 μs |  0.0033 μs |   0.3738 |        - |        - |     2.3 KB |
| NumberObject_Array            | .NET 8.0      | 10    |     3.343 μs |  0.0017 μs |  0.0015 μs |   0.4425 |        - |        - |    2.73 KB |
| NumberObject_List             | NativeAOT 8.0 | 10    |     4.265 μs |  0.0080 μs |  0.0075 μs |   0.4272 |        - |        - |    2.63 KB |
| NumberObject_RecordCollection | NativeAOT 8.0 | 10    |     4.134 μs |  0.0027 μs |  0.0025 μs |   0.3738 |        - |        - |     2.3 KB |
| NumberObject_Array            | NativeAOT 8.0 | 10    |     4.269 μs |  0.0049 μs |  0.0043 μs |   0.4425 |        - |        - |    2.73 KB |
| **NumberObject_List**             | **.NET 8.0**      | **1000**  |   **311.718 μs** |  **0.5182 μs** |  **0.4847 μs** |  **33.2031** |   **7.3242** |        **-** |  **204.27 KB** |
| NumberObject_RecordCollection | .NET 8.0      | 1000  |   290.969 μs |  0.3580 μs |  0.3348 μs |  34.1797 |   7.3242 |        - |  211.67 KB |
| NumberObject_Array            | .NET 8.0      | 1000  |   312.646 μs |  0.0941 μs |  0.0834 μs |  34.1797 |   6.8359 |        - |   212.1 KB |
| NumberObject_List             | NativeAOT 8.0 | 1000  |   398.666 μs |  0.3236 μs |  0.2868 μs |  33.2031 |   7.8125 |        - |  204.27 KB |
| NumberObject_RecordCollection | NativeAOT 8.0 | 1000  |   395.903 μs |  0.1378 μs |  0.1222 μs |  34.1797 |   7.3242 |        - |  211.67 KB |
| NumberObject_Array            | NativeAOT 8.0 | 1000  |   398.400 μs |  0.3234 μs |  0.2525 μs |  34.1797 |   6.8359 |        - |   212.1 KB |
| **NumberObject_List**             | **.NET 8.0**      | **10000** | **4,068.437 μs** | **19.7877 μs** | **18.5094 μs** | **515.6250** | **375.0000** | **234.3750** | **2685.56 KB** |
| NumberObject_RecordCollection | .NET 8.0      | 10000 | 3,850.408 μs | 10.0235 μs |  9.3760 μs | 515.6250 | 359.3750 | 195.3125 | 2763.15 KB |
| NumberObject_Array            | .NET 8.0      | 10000 | 4,048.904 μs | 33.1392 μs | 30.9985 μs | 523.4375 | 375.0000 | 226.5625 | 2763.69 KB |
| NumberObject_List             | NativeAOT 8.0 | 10000 | 4,998.470 μs | 15.7650 μs | 14.7466 μs | 507.8125 | 367.1875 | 218.7500 | 2685.87 KB |
| NumberObject_RecordCollection | NativeAOT 8.0 | 10000 | 4,953.542 μs | 17.3009 μs | 16.1833 μs | 500.0000 | 351.5625 | 218.7500 | 2763.68 KB |
| NumberObject_Array            | NativeAOT 8.0 | 10000 | 4,997.514 μs | 18.3427 μs | 16.2603 μs | 523.4375 | 382.8125 | 195.3125 | 2764.16 KB |


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
| Method                                    | N   | Mean          | Error       | StdDev     | Gen0      | Gen1     | Gen2     | Allocated  |
|------------------------------------------ |---- |--------------:|------------:|-----------:|----------:|---------:|---------:|-----------:|
| **ListOfDtosOfLists**                         | **10**  |      **6.054 μs** |   **0.0021 μs** |  **0.0016 μs** |    **0.9232** |   **0.0076** |        **-** |     **5.7 KB** |
| RecordCollectionOfDtosOfRecordCollections | 10  |      5.117 μs |   0.0057 μs |  0.0051 μs |    1.0300 |   0.0076 |        - |    6.34 KB |
| **ListOfDtosOfLists**                         | **500** | **10,405.051 μs** | **106.4875 μs** | **99.6085 μs** | **1578.1250** | **906.2500** | **312.5000** | **9254.58 KB** |
| RecordCollectionOfDtosOfRecordCollections | 500 | 11,230.055 μs |  66.4236 μs | 51.8592 μs | 1718.7500 | 843.7500 | 343.7500 | 10745.1 KB |


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
| Method                       | Length | Mean        | Error     | StdDev    | Gen0   | Allocated |
|----------------------------- |------- |------------:|----------:|----------:|-------:|----------:|
| **ListSelectToList**             | **10**     |    **34.60 ns** |  **0.027 ns** |  **0.021 ns** | **0.0268** |     **168 B** |
| RecordCollectionSelectToList | 10     |    38.82 ns |  0.083 ns |  0.073 ns | 0.0280 |     176 B |
| ListSelectToRecordCollection | 10     |    40.13 ns |  0.066 ns |  0.062 ns | 0.0216 |     136 B |
| **ListSelectToList**             | **1000**   |   **598.77 ns** |  **1.319 ns** |  **1.234 ns** | **0.6571** |    **4128 B** |
| RecordCollectionSelectToList | 1000   |   701.78 ns |  0.610 ns |  0.571 ns | 0.6590 |    4136 B |
| ListSelectToRecordCollection | 1000   |   580.66 ns |  1.411 ns |  1.319 ns | 0.6523 |    4096 B |
| **ListSelectToList**             | **10000**  | **5,627.66 ns** |  **8.179 ns** |  **7.651 ns** | **6.3629** |   **40128 B** |
| RecordCollectionSelectToList | 10000  | 6,202.02 ns | 27.270 ns | 24.175 ns | 6.3629 |   40136 B |
| ListSelectToRecordCollection | 10000  | 5,557.53 ns | 10.081 ns |  9.430 ns | 6.3629 |   40096 B |
