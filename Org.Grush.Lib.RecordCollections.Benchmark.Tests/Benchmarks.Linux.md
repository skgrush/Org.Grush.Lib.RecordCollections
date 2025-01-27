# Benchmarks
```
Finished: 2025-01-27T18:50:35.3148183+00:00
Elapsed: 00:25:02.7728068
```


## Initializing the type with a collection expression
### e.g. `= [1,2,3,4,.......,31,32]`
Class: Org.Grush.Lib.RecordCollections.Benchmark.Tests.CollectionExpressionInit

```

BenchmarkDotNet v0.14.0, Ubuntu 24.04.1 LTS (Noble Numbat)
AMD EPYC 7502P, 30 CPU, 30 logical and 30 physical cores
.NET SDK 8.0.112
  [Host]               : .NET 8.0.12 (8.0.1224.60305), X64 RyuJIT AVX2
  CollectionExpression : .NET 8.0.12 (8.0.1224.60305), X64 RyuJIT AVX2

Job=CollectionExpression  

```
| Method                     | Runtime       | N  | Mean     | Error    | StdDev   | Ratio | Gen0   | Allocated | Alloc Ratio |
|--------------------------- |-------------- |--- |---------:|---------:|---------:|------:|-------:|----------:|------------:|
| ListOfIntegers             | .NET 8.0      | 32 | 31.97 ns | 0.063 ns | 0.056 ns |  1.00 | 0.0220 |     184 B |        1.00 |
| RecordCollectionOfIntegers | .NET 8.0      | 32 | 20.86 ns | 0.334 ns | 0.313 ns |  0.65 | 0.0181 |     152 B |        0.83 |
| ImmutableArrayOfIntegers   | .NET 8.0      | 32 | 12.08 ns | 0.278 ns | 0.341 ns |  0.38 | 0.0182 |     152 B |        0.83 |
|                            |               |    |          |          |          |       |        |           |             |
| ListOfIntegers             | NativeAOT 8.0 | 32 | 30.06 ns | 0.144 ns | 0.134 ns |  1.00 | 0.0220 |     184 B |        1.00 |
| RecordCollectionOfIntegers | NativeAOT 8.0 | 32 | 18.36 ns | 0.313 ns | 0.293 ns |  0.61 | 0.0181 |     152 B |        0.83 |
| ImmutableArrayOfIntegers   | NativeAOT 8.0 | 32 | 11.59 ns | 0.269 ns | 0.277 ns |  0.39 | 0.0182 |     152 B |        0.83 |


## `foreach` loop enumeration
### Iterate over a pre-built collection of `N` integers using `foreach`.
Class: Org.Grush.Lib.RecordCollections.Benchmark.Tests.ForEachIteration

```

BenchmarkDotNet v0.14.0, Ubuntu 24.04.1 LTS (Noble Numbat)
AMD EPYC 7502P, 30 CPU, 30 logical and 30 physical cores
.NET SDK 8.0.112
  [Host]  : .NET 8.0.12 (8.0.1224.60305), X64 RyuJIT AVX2
  ForEach : .NET 8.0.12 (8.0.1224.60305), X64 RyuJIT AVX2

Job=ForEach  

```
| Method                     | Runtime       | N     | Mean         | Error     | StdDev     | Median       | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |-------------- |------ |-------------:|----------:|-----------:|-------------:|------:|--------:|----------:|------------:|
| **ListOfIntegers**             | **.NET 8.0**      | **10**    |     **22.42 ns** |  **0.129 ns** |   **0.121 ns** |     **22.48 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| RecordCollectionOfIntegers | .NET 8.0      | 10    |     19.31 ns |  0.142 ns |   0.133 ns |     19.32 ns |  0.86 |    0.01 |         - |          NA |
| ImmutableArrayOfIntegers   | .NET 8.0      | 10    |     18.93 ns |  0.004 ns |   0.004 ns |     18.93 ns |  0.84 |    0.00 |         - |          NA |
|                            |               |       |              |           |            |              |       |         |           |             |
| ListOfIntegers             | NativeAOT 8.0 | 10    |     22.22 ns |  0.057 ns |   0.048 ns |     22.21 ns |  1.00 |    0.00 |         - |          NA |
| RecordCollectionOfIntegers | NativeAOT 8.0 | 10    |     21.91 ns |  0.102 ns |   0.091 ns |     21.94 ns |  0.99 |    0.00 |         - |          NA |
| ImmutableArrayOfIntegers   | NativeAOT 8.0 | 10    |     19.35 ns |  0.090 ns |   0.084 ns |     19.36 ns |  0.87 |    0.00 |         - |          NA |
|                            |               |       |              |           |            |              |       |         |           |             |
| **ListOfIntegers**             | **.NET 8.0**      | **1000**  |  **2,345.64 ns** | **46.721 ns** | **137.023 ns** |  **2,432.71 ns** |  **1.00** |    **0.08** |         **-** |          **NA** |
| RecordCollectionOfIntegers | .NET 8.0      | 1000  |  2,083.28 ns |  0.528 ns |   0.468 ns |  2,083.44 ns |  0.89 |    0.05 |         - |          NA |
| ImmutableArrayOfIntegers   | .NET 8.0      | 1000  |  2,137.78 ns |  1.431 ns |   1.339 ns |  2,138.00 ns |  0.91 |    0.05 |         - |          NA |
|                            |               |       |              |           |            |              |       |         |           |             |
| ListOfIntegers             | NativeAOT 8.0 | 1000  |  2,113.48 ns |  0.449 ns |   0.420 ns |  2,113.35 ns |  1.00 |    0.00 |         - |          NA |
| RecordCollectionOfIntegers | NativeAOT 8.0 | 1000  |  2,144.89 ns |  5.465 ns |   5.112 ns |  2,144.59 ns |  1.01 |    0.00 |         - |          NA |
| ImmutableArrayOfIntegers   | NativeAOT 8.0 | 1000  |  2,073.74 ns |  0.667 ns |   0.624 ns |  2,073.54 ns |  0.98 |    0.00 |         - |          NA |
|                            |               |       |              |           |            |              |       |         |           |             |
| **ListOfIntegers**             | **.NET 8.0**      | **10000** | **21,051.60 ns** |  **8.788 ns** |   **7.790 ns** | **21,052.17 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| RecordCollectionOfIntegers | .NET 8.0      | 10000 | 21,509.34 ns | 52.070 ns |  48.706 ns | 21,519.52 ns |  1.02 |    0.00 |         - |          NA |
| ImmutableArrayOfIntegers   | .NET 8.0      | 10000 | 21,108.94 ns | 12.040 ns |  10.673 ns | 21,110.52 ns |  1.00 |    0.00 |         - |          NA |
|                            |               |       |              |           |            |              |       |         |           |             |
| ListOfIntegers             | NativeAOT 8.0 | 10000 | 21,048.41 ns |  4.186 ns |   3.915 ns | 21,048.48 ns |  1.00 |    0.00 |         - |          NA |
| RecordCollectionOfIntegers | NativeAOT 8.0 | 10000 | 21,404.19 ns | 69.261 ns |  64.787 ns | 21,403.24 ns |  1.02 |    0.00 |         - |          NA |
| ImmutableArrayOfIntegers   | NativeAOT 8.0 | 10000 | 21,082.83 ns | 20.187 ns |  18.883 ns | 21,089.78 ns |  1.00 |    0.00 |         - |          NA |


## System.Text.Json Deserialization - Source-generated, integers
### Deserialize integer collections using explicit `JsonSerializerContext`s.
Class: Org.Grush.Lib.RecordCollections.Benchmark.Tests.JsonDeserializationInteger

```

BenchmarkDotNet v0.14.0, Ubuntu 24.04.1 LTS (Noble Numbat)
AMD EPYC 7502P, 30 CPU, 30 logical and 30 physical cores
.NET SDK 8.0.112
  [Host]                     : .NET 8.0.12 (8.0.1224.60305), X64 RyuJIT AVX2
  JsonDeserializationInteger : .NET 8.0.12 (8.0.1224.60305), X64 RyuJIT AVX2

Job=JsonDeserializationInteger  

```
| Method              | Runtime       | N     | Mean         | Error       | StdDev      | Ratio | Gen0    | Allocated | Alloc Ratio |
|-------------------- |-------------- |------ |-------------:|------------:|------------:|------:|--------:|----------:|------------:|
| **IntList**             | **.NET 8.0**      | **10**    |     **708.2 ns** |     **1.33 ns** |     **1.24 ns** |  **1.00** |  **0.0257** |     **216 B** |        **1.00** |
| IntRecordCollection | .NET 8.0      | 10    |     698.3 ns |     1.81 ns |     1.69 ns |  0.99 |  0.0334 |     280 B |        1.30 |
| IntImmutableArray   | .NET 8.0      | 10    |     778.2 ns |     1.77 ns |     1.66 ns |  1.10 |  0.0362 |     304 B |        1.41 |
|                     |               |       |              |             |             |       |         |           |             |
| IntList             | NativeAOT 8.0 | 10    |     691.0 ns |     0.88 ns |     0.82 ns |  1.00 |  0.0257 |     216 B |        1.00 |
| IntRecordCollection | NativeAOT 8.0 | 10    |     689.8 ns |     4.73 ns |     4.43 ns |  1.00 |  0.0334 |     280 B |        1.30 |
| IntImmutableArray   | NativeAOT 8.0 | 10    |     785.1 ns |     5.71 ns |     5.06 ns |  1.14 |  0.0362 |     304 B |        1.41 |
|                     |               |       |              |             |             |       |         |           |             |
| **IntList**             | **.NET 8.0**      | **1000**  |  **45,721.4 ns** |    **21.25 ns** |    **18.84 ns** |  **1.00** |  **0.9766** |    **8424 B** |        **1.00** |
| IntRecordCollection | .NET 8.0      | 1000  |  45,918.5 ns |    45.89 ns |    40.68 ns |  1.00 |  1.4648 |   12448 B |        1.48 |
| IntImmutableArray   | .NET 8.0      | 1000  |  45,887.7 ns |    36.59 ns |    32.44 ns |  1.00 |  1.4648 |   12472 B |        1.48 |
|                     |               |       |              |             |             |       |         |           |             |
| IntList             | NativeAOT 8.0 | 1000  |  45,141.5 ns |    25.55 ns |    22.65 ns |  1.00 |  0.9766 |    8424 B |        1.00 |
| IntRecordCollection | NativeAOT 8.0 | 1000  |  44,826.8 ns |   112.30 ns |    99.55 ns |  0.99 |  1.4648 |   12448 B |        1.48 |
| IntImmutableArray   | NativeAOT 8.0 | 1000  |  44,949.4 ns |    29.15 ns |    27.27 ns |  1.00 |  1.4648 |   12472 B |        1.48 |
|                     |               |       |              |             |             |       |         |           |             |
| **IntList**             | **.NET 8.0**      | **10000** | **453,702.5 ns** |   **331.95 ns** |   **310.51 ns** |  **1.00** | **15.6250** |  **131400 B** |        **1.00** |
| IntRecordCollection | .NET 8.0      | 10000 | 458,246.0 ns | 1,016.03 ns |   900.68 ns |  1.01 | 20.0195 |  171424 B |        1.30 |
| IntImmutableArray   | .NET 8.0      | 10000 | 466,792.4 ns | 2,690.19 ns | 2,246.43 ns |  1.03 | 19.5313 |  171449 B |        1.30 |
|                     |               |       |              |             |             |       |         |           |             |
| IntList             | NativeAOT 8.0 | 10000 | 452,239.1 ns | 2,966.37 ns | 2,774.75 ns |  1.00 | 15.6250 |  131400 B |        1.00 |
| IntRecordCollection | NativeAOT 8.0 | 10000 | 454,303.9 ns | 3,861.79 ns | 3,612.32 ns |  1.00 | 20.0195 |  171424 B |        1.30 |
| IntImmutableArray   | NativeAOT 8.0 | 10000 | 454,396.5 ns |   603.07 ns |   564.11 ns |  1.00 | 20.0195 |  171448 B |        1.30 |


## System.Text.Json Deserialization - Source-generated, record of three types.
### Deserialize `(int, double, string)` collections using explicit `JsonSerializerContext`s.
Class: Org.Grush.Lib.RecordCollections.Benchmark.Tests.JsonDeserializationObject

```

BenchmarkDotNet v0.14.0, Ubuntu 24.04.1 LTS (Noble Numbat)
AMD EPYC 7502P, 30 CPU, 30 logical and 30 physical cores
.NET SDK 8.0.112
  [Host]                    : .NET 8.0.12 (8.0.1224.60305), X64 RyuJIT AVX2
  JsonDeserializationObject : .NET 8.0.12 (8.0.1224.60305), X64 RyuJIT AVX2

Job=JsonDeserializationObject  

```
| Method                        | Runtime       | N     | Mean          | Error      | StdDev     | Ratio | Gen0     | Gen1     | Gen2    | Allocated  | Alloc Ratio |
|------------------------------ |-------------- |------ |--------------:|-----------:|-----------:|------:|---------:|---------:|--------:|-----------:|------------:|
| **NumberObject_List**             | **.NET 8.0**      | **10**    |      **7.987 μs** |  **0.0424 μs** |  **0.0396 μs** |  **1.00** |   **0.3204** |        **-** |       **-** |    **2.66 KB** |        **1.00** |
| NumberObject_RecordCollection | .NET 8.0      | 10    |      7.516 μs |  0.0558 μs |  0.0522 μs |  0.94 |   0.2747 |        - |       - |     2.3 KB |        0.86 |
| NumberObject_Array            | .NET 8.0      | 10    |      8.382 μs |  0.0212 μs |  0.0198 μs |  1.05 |   0.3357 |        - |       - |    2.78 KB |        1.05 |
|                               |               |       |               |            |            |       |          |          |         |            |             |
| NumberObject_List             | NativeAOT 8.0 | 10    |      7.463 μs |  0.0172 μs |  0.0152 μs |  1.00 |   0.3204 |        - |       - |    2.66 KB |        1.00 |
| NumberObject_RecordCollection | NativeAOT 8.0 | 10    |      6.883 μs |  0.0242 μs |  0.0226 μs |  0.92 |   0.2747 |        - |       - |     2.3 KB |        0.86 |
| NumberObject_Array            | NativeAOT 8.0 | 10    |      7.635 μs |  0.0415 μs |  0.0389 μs |  1.02 |   0.3357 |        - |       - |    2.78 KB |        1.05 |
|                               |               |       |               |            |            |       |          |          |         |            |             |
| **NumberObject_List**             | **.NET 8.0**      | **1000**  |    **743.372 μs** |  **1.4489 μs** |  **1.3553 μs** |  **1.00** |  **24.4141** |   **5.8594** |       **-** |  **204.17 KB** |        **1.00** |
| NumberObject_RecordCollection | .NET 8.0      | 1000  |    706.090 μs |  2.0102 μs |  1.8803 μs |  0.95 |  25.3906 |   5.8594 |       - |  211.55 KB |        1.04 |
| NumberObject_Array            | .NET 8.0      | 1000  |    762.538 μs |  2.9480 μs |  2.7575 μs |  1.03 |  25.3906 |   5.8594 |       - |  212.03 KB |        1.04 |
|                               |               |       |               |            |            |       |          |          |         |            |             |
| NumberObject_List             | NativeAOT 8.0 | 1000  |    714.386 μs |  2.7070 μs |  2.5321 μs |  1.00 |  24.4141 |   5.8594 |       - |  204.16 KB |        1.00 |
| NumberObject_RecordCollection | NativeAOT 8.0 | 1000  |    677.487 μs |  6.1336 μs |  5.7374 μs |  0.95 |  25.3906 |   5.8594 |       - |  211.55 KB |        1.04 |
| NumberObject_Array            | NativeAOT 8.0 | 1000  |    735.683 μs |  4.3414 μs |  4.0610 μs |  1.03 |  25.3906 |   5.8594 |       - |  212.03 KB |        1.04 |
|                               |               |       |               |            |            |       |          |          |         |            |             |
| **NumberObject_List**             | **.NET 8.0**      | **10000** |  **9,815.400 μs** | **74.5425 μs** | **69.7271 μs** |  **1.00** | **328.1250** | **250.0000** | **93.7500** | **2685.32 KB** |        **1.00** |
| NumberObject_RecordCollection | .NET 8.0      | 10000 |  9,182.034 μs | 26.8327 μs | 20.9492 μs |  0.94 | 343.7500 | 281.2500 | 93.7500 | 2763.05 KB |        1.03 |
| NumberObject_Array            | .NET 8.0      | 10000 | 10,093.120 μs | 52.6614 μs | 49.2595 μs |  1.03 | 343.7500 | 281.2500 | 93.7500 | 2763.62 KB |        1.03 |
|                               |               |       |               |            |            |       |          |          |         |            |             |
| NumberObject_List             | NativeAOT 8.0 | 10000 |  9,006.453 μs | 29.0267 μs | 25.7314 μs |  1.00 | 328.1250 | 265.6250 | 93.7500 | 2685.27 KB |        1.00 |
| NumberObject_RecordCollection | NativeAOT 8.0 | 10000 |  8,628.684 μs | 40.4538 μs | 37.8405 μs |  0.96 | 328.1250 | 234.3750 | 78.1250 | 2762.91 KB |        1.03 |
| NumberObject_Array            | NativeAOT 8.0 | 10000 |  9,234.630 μs | 47.1713 μs | 44.1241 μs |  1.03 | 328.1250 | 234.3750 | 78.1250 | 2763.38 KB |        1.03 |


## System.Text.Json Deserialization - Dynamic
### Deserialize collections using automatically-selected factories and converters.
Class: Org.Grush.Lib.RecordCollections.Benchmark.Tests.JsonDynamicDeserialization

```

BenchmarkDotNet v0.14.0, Ubuntu 24.04.1 LTS (Noble Numbat)
AMD EPYC 7502P, 30 CPU, 30 logical and 30 physical cores
.NET SDK 8.0.112
  [Host]   : .NET 8.0.12 (8.0.1224.60305), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.12 (8.0.1224.60305), X64 RyuJIT AVX2

Job=.NET 8.0  Runtime=.NET 8.0  

```
| Method                                    | N   | Mean         | Error      | StdDev     | Ratio | Gen0      | Gen1      | Gen2     | Allocated   | Alloc Ratio |
|------------------------------------------ |---- |-------------:|-----------:|-----------:|------:|----------:|----------:|---------:|------------:|------------:|
| **ListOfDtosOfLists**                         | **10**  |     **13.83 μs** |   **0.053 μs** |   **0.050 μs** |  **1.00** |    **0.6866** |         **-** |        **-** |     **5.73 KB** |        **1.00** |
| RecordCollectionOfDtosOfRecordCollections | 10  |     11.90 μs |   0.027 μs |   0.025 μs |  0.86 |    0.7629 |         - |        - |     6.34 KB |        1.11 |
|                                           |     |              |            |            |       |           |           |          |             |             |
| **ListOfDtosOfLists**                         | **500** | **16,121.71 μs** |  **64.380 μs** |  **57.071 μs** |  **1.00** | **1031.2500** |  **937.5000** |  **62.5000** |  **9255.27 KB** |        **1.00** |
| RecordCollectionOfDtosOfRecordCollections | 500 | 17,602.12 μs | 220.952 μs | 206.679 μs |  1.09 | 1312.5000 | 1125.0000 | 156.2500 | 10743.69 KB |        1.16 |


## LINQ iteration
### Iterate of a prebuilt collection with `.Select(x => x).ToList();`
Class: Org.Grush.Lib.RecordCollections.Benchmark.Tests.SelectToList

```

BenchmarkDotNet v0.14.0, Ubuntu 24.04.1 LTS (Noble Numbat)
AMD EPYC 7502P, 30 CPU, 30 logical and 30 physical cores
.NET SDK 8.0.112
  [Host]   : .NET 8.0.12 (8.0.1224.60305), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.12 (8.0.1224.60305), X64 RyuJIT AVX2

Job=.NET 8.0  Runtime=.NET 8.0  

```
| Method                       | Length | Mean        | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------------------------- |------- |------------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **ListSelectToList**             | **10**     |    **58.80 ns** |  **0.306 ns** |  **0.255 ns** |  **1.00** |    **0.01** | **0.0200** |     **168 B** |        **1.00** |
| RecordCollectionSelectToList | 10     |    92.28 ns |  0.603 ns |  0.564 ns |  1.57 |    0.01 | 0.0210 |     176 B |        1.05 |
| ListSelectToRecordCollection | 10     |    62.87 ns |  0.314 ns |  0.293 ns |  1.07 |    0.01 | 0.0162 |     136 B |        0.81 |
|                              |        |             |           |           |       |         |        |           |             |
| **ListSelectToList**             | **1000**   |   **859.87 ns** |  **8.838 ns** |  **7.835 ns** |  **1.00** |    **0.01** | **0.4930** |    **4128 B** |        **1.00** |
| RecordCollectionSelectToList | 1000   | 1,012.31 ns | 10.894 ns |  9.657 ns |  1.18 |    0.02 | 0.4940 |    4136 B |        1.00 |
| ListSelectToRecordCollection | 1000   |   896.22 ns | 10.474 ns |  9.797 ns |  1.04 |    0.01 | 0.4892 |    4096 B |        0.99 |
|                              |        |             |           |           |       |         |        |           |             |
| **ListSelectToList**             | **10000**  | **7,454.87 ns** | **76.654 ns** | **71.702 ns** |  **1.00** |    **0.01** | **4.7836** |   **40128 B** |        **1.00** |
| RecordCollectionSelectToList | 10000  | 9,160.35 ns | 95.539 ns | 89.368 ns |  1.23 |    0.02 | 4.7760 |   40136 B |        1.00 |
| ListSelectToRecordCollection | 10000  | 7,437.65 ns | 20.634 ns | 19.301 ns |  1.00 |    0.01 | 4.7607 |   40096 B |        1.00 |
