``` ini

BenchmarkDotNet=v0.13.1, OS=macOS Monterey 12.3.1 (21E258) [Darwin 21.4.0]
Apple M1, 1 CPU, 8 logical and 8 physical cores
.NET SDK=6.0.202
  [Host]     : .NET 6.0.4 (6.0.422.16404), Arm64 RyuJIT
  Job-RUFEVW : .NET 6.0.4 (6.0.422.16404), Arm64 RyuJIT

IterationCount=5  LaunchCount=1  WarmupCount=5  

```
|                Method |          Mean |            Error |         StdDev |       Median | Rank |    Gen 0 |    Gen 1 |    Gen 2 | Allocated |
|---------------------- |--------------:|-----------------:|---------------:|-------------:|-----:|---------:|---------:|---------:|----------:|
|            NodeAdding |      11.98 μs |         0.031 μs |       0.008 μs |     11.97 μs |    1 |   8.0872 |        - |        - |     17 KB |
|         CleaningNodes |  12,566.78 μs |     8,469.757 μs |   1,310.704 μs | 12,491.03 μs |    2 | 333.3333 | 333.3333 | 333.3333 |  2,185 KB |
|            FileAdding |  26,217.06 μs |    65,565.631 μs |  17,027.186 μs | 30,474.83 μs |    2 | 130.8594 | 130.8594 | 130.8594 |    481 KB |
| FileAddingAndRemoving |  72,203.65 μs |   322,268.681 μs |  83,692.153 μs | 49,119.63 μs |    2 | 125.0000 | 125.0000 | 125.0000 |    494 KB |
|        BalancingNodes | 167,352.22 μs | 1,406,981.872 μs | 217,732.002 μs | 97,410.02 μs |    2 |        - |        - |        - |    958 KB |
