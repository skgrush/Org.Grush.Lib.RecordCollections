# Benchmarks
> (2025-01-21T20:18:21.8116110-06:00)


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
| ListOfIntegers             | .NET 8.0      | 32 | 22.173 ns | 0.0505 ns | 0.0472 ns | 0.0293 |     184 B |
| RecordCollectionOfIntegers | .NET 8.0      | 32 |  9.195 ns | 0.0881 ns | 0.0824 ns | 0.0242 |     152 B |
| ArrayOfIntegers            | .NET 8.0      | 32 |  7.231 ns | 0.0895 ns | 0.0837 ns | 0.0242 |     152 B |
| ListOfIntegers             | NativeAOT 8.0 | 32 | 22.077 ns | 0.0687 ns | 0.0643 ns | 0.0293 |     184 B |
| RecordCollectionOfIntegers | NativeAOT 8.0 | 32 |  8.884 ns | 0.0226 ns | 0.0212 ns | 0.0242 |     152 B |
| ArrayOfIntegers            | NativeAOT 8.0 | 32 |  7.430 ns | 0.0145 ns | 0.0136 ns | 0.0242 |     152 B |


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
| **ListOfIntegers**             | **.NET 8.0**      | **10**    |     **11.30 ns** |  **0.006 ns** |  **0.005 ns** |         **-** |
| RecordCollectionOfIntegers | .NET 8.0      | 10    |     10.43 ns |  0.008 ns |  0.007 ns |         - |
| ArrayOfIntegers            | .NET 8.0      | 10    |     10.44 ns |  0.009 ns |  0.008 ns |         - |
| ListOfIntegers             | NativeAOT 8.0 | 10    |     10.52 ns |  0.004 ns |  0.003 ns |         - |
| RecordCollectionOfIntegers | NativeAOT 8.0 | 10    |     10.17 ns |  0.012 ns |  0.011 ns |         - |
| ArrayOfIntegers            | NativeAOT 8.0 | 10    |     10.16 ns |  0.014 ns |  0.013 ns |         - |
| **ListOfIntegers**             | **.NET 8.0**      | **1000**  |  **1,123.48 ns** |  **1.044 ns** |  **0.977 ns** |         **-** |
| RecordCollectionOfIntegers | .NET 8.0      | 1000  |  1,127.72 ns |  1.824 ns |  1.706 ns |         - |
| ArrayOfIntegers            | .NET 8.0      | 1000  |  1,127.28 ns |  2.127 ns |  1.989 ns |         - |
| ListOfIntegers             | NativeAOT 8.0 | 1000  |  1,423.13 ns |  2.789 ns |  2.609 ns |         - |
| RecordCollectionOfIntegers | NativeAOT 8.0 | 1000  |  1,391.37 ns |  2.139 ns |  2.001 ns |         - |
| ArrayOfIntegers            | NativeAOT 8.0 | 1000  |  1,391.13 ns |  1.817 ns |  1.611 ns |         - |
| **ListOfIntegers**             | **.NET 8.0**      | **10000** | **11,177.43 ns** |  **8.116 ns** |  **7.591 ns** |         **-** |
| RecordCollectionOfIntegers | .NET 8.0      | 10000 | 11,283.48 ns | 14.174 ns | 13.259 ns |         - |
| ArrayOfIntegers            | .NET 8.0      | 10000 | 11,281.11 ns | 15.181 ns | 14.200 ns |         - |
| ListOfIntegers             | NativeAOT 8.0 | 10000 | 14,340.41 ns | 14.716 ns | 13.046 ns |         - |
| RecordCollectionOfIntegers | NativeAOT 8.0 | 10000 | 14,134.13 ns | 19.140 ns | 15.983 ns |         - |
| ArrayOfIntegers            | NativeAOT 8.0 | 10000 | 14,123.67 ns | 24.201 ns | 22.638 ns |         - |


## System.Text.Json Deserialization - Source-generated, integers
### Deserialize integer collections using explicit `JsonSerializerContext`s.
Class: Org.Grush.Lib.RecordCollections.Benchmark.Tests.JsonDeserializationInteger

```

BenchmarkDotNet v0.14.0, macOS Sequoia 15.2 (24C101) [Darwin 24.2.0]
Apple M1, 1 CPU, 8 logical and 8 physical cores
.NET SDK 8.0.301
  [Host]        : .NET 8.0.6 (8.0.624.26715), Arm64 RyuJIT AdvSIMD
  .NET 8.0      : .NET 8.0.6 (8.0.624.26715), Arm64 RyuJIT AdvSIMD
  NativeAOT 8.0 : .NET 8.0.6, Arm64 NativeAOT AdvSIMD


```
| Method                              | Job           | Runtime       | N     | Mean         | Error     | StdDev    | Gen0    | Gen1   | Allocated |
|------------------------------------ |-------------- |-------------- |------ |-------------:|----------:|----------:|--------:|-------:|----------:|
| **Deserialization_IntList**             | **.NET 8.0**      | **.NET 8.0**      | **10**    |     **360.6 ns** |   **0.44 ns** |   **0.41 ns** |  **0.0343** |      **-** |     **216 B** |
| Deserialization_IntRecordCollection | .NET 8.0      | .NET 8.0      | 10    |     339.4 ns |   0.29 ns |   0.24 ns |  0.0443 |      - |     280 B |
| Deserialization_IntArray            | .NET 8.0      | .NET 8.0      | 10    |     376.7 ns |   1.99 ns |   1.77 ns |  0.0443 |      - |     280 B |
| Deserialization_IntList             | NativeAOT 8.0 | NativeAOT 8.0 | 10    |     468.4 ns |   0.15 ns |   0.14 ns |  0.0343 |      - |     216 B |
| Deserialization_IntRecordCollection | NativeAOT 8.0 | NativeAOT 8.0 | 10    |     476.2 ns |   0.35 ns |   0.31 ns |  0.0439 |      - |     280 B |
| Deserialization_IntArray            | NativeAOT 8.0 | NativeAOT 8.0 | 10    |     479.0 ns |   0.36 ns |   0.32 ns |  0.0439 |      - |     280 B |
| **Deserialization_IntList**             | **.NET 8.0**      | **.NET 8.0**      | **1000**  |  **26,529.7 ns** |  **15.96 ns** |  **14.93 ns** |  **1.3123** |      **-** |    **8425 B** |
| Deserialization_IntRecordCollection | .NET 8.0      | .NET 8.0      | 1000  |  25,881.9 ns |  26.97 ns |  23.90 ns |  1.9836 |      - |   12449 B |
| Deserialization_IntArray            | .NET 8.0      | .NET 8.0      | 1000  |  26,688.0 ns |   7.03 ns |   5.87 ns |  1.9836 |      - |   12449 B |
| Deserialization_IntList             | NativeAOT 8.0 | NativeAOT 8.0 | 1000  |  32,657.8 ns |  15.26 ns |  14.27 ns |  1.2817 |      - |    8426 B |
| Deserialization_IntRecordCollection | NativeAOT 8.0 | NativeAOT 8.0 | 1000  |  33,361.8 ns |  26.37 ns |  24.67 ns |  1.9531 |      - |   12450 B |
| Deserialization_IntArray            | NativeAOT 8.0 | NativeAOT 8.0 | 1000  |  32,640.7 ns |  42.67 ns |  37.83 ns |  1.9531 |      - |   12450 B |
| **Deserialization_IntList**             | **.NET 8.0**      | **.NET 8.0**      | **10000** | **263,408.9 ns** | **221.46 ns** | **207.15 ns** | **20.5078** |      **-** |  **131656 B** |
| Deserialization_IntRecordCollection | .NET 8.0      | .NET 8.0      | 10000 | 258,538.6 ns | 314.12 ns | 293.83 ns | 26.8555 | 5.3711 |  171680 B |
| Deserialization_IntArray            | .NET 8.0      | .NET 8.0      | 10000 | 264,266.8 ns | 232.39 ns | 206.01 ns | 26.8555 | 5.3711 |  171680 B |
| Deserialization_IntList             | NativeAOT 8.0 | NativeAOT 8.0 | 10000 | 325,244.4 ns | 306.85 ns | 272.01 ns | 20.5078 | 3.9063 |  131656 B |
| Deserialization_IntRecordCollection | NativeAOT 8.0 | NativeAOT 8.0 | 10000 | 329,488.8 ns |  95.04 ns |  88.90 ns | 26.8555 | 5.3711 |  171680 B |
| Deserialization_IntArray            | NativeAOT 8.0 | NativeAOT 8.0 | 10000 | 323,556.6 ns | 257.73 ns | 241.09 ns | 26.8555 | 5.3711 |  171680 B |


## System.Text.Json Deserialization - Source-generated, record of three types.
### Deserialize `(int, double, string)` collections using explicit `JsonSerializerContext`s.
Class: Org.Grush.Lib.RecordCollections.Benchmark.Tests.JsonDeserializationObject

```

BenchmarkDotNet v0.14.0, macOS Sequoia 15.2 (24C101) [Darwin 24.2.0]
Apple M1, 1 CPU, 8 logical and 8 physical cores
.NET SDK 8.0.301
  [Host]        : .NET 8.0.6 (8.0.624.26715), Arm64 RyuJIT AdvSIMD
  .NET 8.0      : .NET 8.0.6 (8.0.624.26715), Arm64 RyuJIT AdvSIMD
  NativeAOT 8.0 : .NET 8.0.6, Arm64 NativeAOT AdvSIMD


```
| Method                                        | Job           | Runtime       | N     | Mean         | Error      | StdDev     | Gen0     | Gen1     | Gen2     | Allocated  |
|---------------------------------------------- |-------------- |-------------- |------ |-------------:|-----------:|-----------:|---------:|---------:|---------:|-----------:|
| **Deserialization_NumberObject_List**             | **.NET 8.0**      | **.NET 8.0**      | **10**    |     **3.354 μs** |  **0.0026 μs** |  **0.0023 μs** |   **0.4272** |        **-** |        **-** |    **2.63 KB** |
| Deserialization_NumberObject_RecordCollection | .NET 8.0      | .NET 8.0      | 10    |     3.042 μs |  0.0010 μs |  0.0008 μs |   0.3738 |        - |        - |     2.3 KB |
| Deserialization_NumberObject_Array            | .NET 8.0      | .NET 8.0      | 10    |     3.329 μs |  0.0026 μs |  0.0024 μs |   0.4425 |        - |        - |    2.73 KB |
| Deserialization_NumberObject_List             | NativeAOT 8.0 | NativeAOT 8.0 | 10    |     4.210 μs |  0.0033 μs |  0.0029 μs |   0.4272 |        - |        - |    2.63 KB |
| Deserialization_NumberObject_RecordCollection | NativeAOT 8.0 | NativeAOT 8.0 | 10    |     4.134 μs |  0.0053 μs |  0.0045 μs |   0.3738 |        - |        - |     2.3 KB |
| Deserialization_NumberObject_Array            | NativeAOT 8.0 | NativeAOT 8.0 | 10    |     4.227 μs |  0.0037 μs |  0.0034 μs |   0.4425 |        - |        - |    2.73 KB |
| **Deserialization_NumberObject_List**             | **.NET 8.0**      | **.NET 8.0**      | **1000**  |   **311.134 μs** |  **0.3935 μs** |  **0.3489 μs** |  **33.2031** |   **7.3242** |        **-** |  **204.27 KB** |
| Deserialization_NumberObject_RecordCollection | .NET 8.0      | .NET 8.0      | 1000  |   288.644 μs |  0.2186 μs |  0.1825 μs |  34.1797 |   7.3242 |        - |  211.67 KB |
| Deserialization_NumberObject_Array            | .NET 8.0      | .NET 8.0      | 1000  |   428.423 μs |  0.2964 μs |  0.2628 μs |  34.1797 |   6.8359 |        - |   212.1 KB |
| Deserialization_NumberObject_List             | NativeAOT 8.0 | NativeAOT 8.0 | 1000  |   397.886 μs |  0.1178 μs |  0.0984 μs |  33.2031 |   7.8125 |        - |  204.27 KB |
| Deserialization_NumberObject_RecordCollection | NativeAOT 8.0 | NativeAOT 8.0 | 1000  |   396.105 μs |  0.1645 μs |  0.1458 μs |  34.1797 |   7.8125 |        - |  211.67 KB |
| Deserialization_NumberObject_Array            | NativeAOT 8.0 | NativeAOT 8.0 | 1000  |   397.684 μs |  0.2370 μs |  0.2217 μs |  34.1797 |   6.8359 |        - |   212.1 KB |
| **Deserialization_NumberObject_List**             | **.NET 8.0**      | **.NET 8.0**      | **10000** | **3,919.794 μs** |  **6.7586 μs** |  **6.3220 μs** | **492.1875** | **328.1250** | **164.0625** | **2685.45 KB** |
| Deserialization_NumberObject_RecordCollection | .NET 8.0      | .NET 8.0      | 10000 | 3,637.810 μs | 14.7700 μs | 13.8158 μs | 500.0000 | 335.9375 | 167.9688 | 2763.23 KB |
| Deserialization_NumberObject_Array            | .NET 8.0      | .NET 8.0      | 10000 | 3,880.440 μs |  6.5752 μs |  6.1505 μs | 503.9063 | 335.9375 | 167.9688 | 2763.71 KB |
| Deserialization_NumberObject_List             | NativeAOT 8.0 | NativeAOT 8.0 | 10000 | 4,870.061 μs |  4.9831 μs |  4.6612 μs | 484.3750 | 328.1250 | 164.0625 | 2685.84 KB |
| Deserialization_NumberObject_RecordCollection | NativeAOT 8.0 | NativeAOT 8.0 | 10000 | 4,765.698 μs |  8.7325 μs |  7.2920 μs | 500.0000 | 328.1250 | 164.0625 | 2763.63 KB |
| Deserialization_NumberObject_Array            | NativeAOT 8.0 | NativeAOT 8.0 | 10000 | 4,780.327 μs |  3.2912 μs |  2.9176 μs | 500.0000 | 328.1250 | 164.0625 |  2764.1 KB |


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
| Method                                    | N   | Mean          | Error      | StdDev     | Gen0      | Gen1     | Gen2     | Allocated   |
|------------------------------------------ |---- |--------------:|-----------:|-----------:|----------:|---------:|---------:|------------:|
| **ListOfDtosOfLists**                         | **10**  |      **6.108 μs** |  **0.0049 μs** |  **0.0043 μs** |    **0.9232** |   **0.0076** |        **-** |      **5.7 KB** |
| RecordCollectionOfDtosOfRecordCollections | 10  |      5.126 μs |  0.0033 μs |  0.0031 μs |    1.0300 |   0.0076 |        - |     6.34 KB |
| **ListOfDtosOfLists**                         | **500** |  **9,507.797 μs** | **89.5378 μs** | **83.7537 μs** | **1562.5000** | **906.2500** | **312.5000** |  **9255.28 KB** |
| RecordCollectionOfDtosOfRecordCollections | 500 | 11,791.138 μs | 17.4397 μs | 16.3131 μs | 1781.2500 | 859.3750 | 343.7500 | 10745.33 KB |


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
| **ListSelectToList**             | **10**     |    **33.13 ns** |  **0.021 ns** |  **0.020 ns** | **0.0268** |     **168 B** |
| RecordCollectionSelectToList | 10     |    39.16 ns |  0.033 ns |  0.027 ns | 0.0280 |     176 B |
| ListSelectToRecordCollection | 10     |    40.04 ns |  0.142 ns |  0.132 ns | 0.0216 |     136 B |
| **ListSelectToList**             | **1000**   |   **604.53 ns** |  **2.216 ns** |  **2.073 ns** | **0.6571** |    **4128 B** |
| RecordCollectionSelectToList | 1000   |   707.13 ns |  0.657 ns |  0.583 ns | 0.6590 |    4136 B |
| ListSelectToRecordCollection | 1000   |   581.56 ns |  1.969 ns |  1.842 ns | 0.6523 |    4096 B |
| **ListSelectToList**             | **10000**  | **5,645.67 ns** |  **4.539 ns** |  **4.024 ns** | **6.3629** |   **40128 B** |
| RecordCollectionSelectToList | 10000  | 6,230.58 ns | 23.033 ns | 21.545 ns | 6.3629 |   40136 B |
| ListSelectToRecordCollection | 10000  | 5,569.98 ns |  5.806 ns |  5.147 ns | 6.3629 |   40096 B |
