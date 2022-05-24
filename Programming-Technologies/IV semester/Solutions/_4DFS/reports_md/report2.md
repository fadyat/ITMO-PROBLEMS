``` ini

BenchmarkDotNet=v0.13.1, OS=macOS Monterey 12.3.1 (21E258) [Darwin 21.4.0]
Apple M1, 1 CPU, 8 logical and 8 physical cores
.NET SDK=6.0.202
  [Host]     : .NET 6.0.4 (6.0.422.16404), Arm64 RyuJIT
  Job-AOEKKY : .NET 6.0.4 (6.0.422.16404), Arm64 RyuJIT

IterationCount=5  LaunchCount=1  WarmupCount=5  

```
|                Method |          Mean |         Error |      StdDev | Rank |     Gen 0 |     Gen 1 |     Gen 2 | Allocated |
|---------------------- |--------------:|--------------:|------------:|-----:|----------:|----------:|----------:|----------:|
|            NodeAdding |      9.794 μs |     0.0814 μs |   0.0126 μs |    1 |   11.2915 |         - |         - |     23 KB |
|            FileAdding |  5,780.928 μs | 3,513.4302 μs | 543.7072 μs |    2 |  326.1719 |  322.2656 |  322.2656 |  1,320 KB |
| FileAddingAndRemoving |  6,233.037 μs | 3,156.7298 μs | 819.7927 μs |    2 |  507.8125 |  500.0000 |  500.0000 |  1,716 KB |
|        BalancingNodes | 14,822.956 μs | 5,695.1444 μs | 881.3299 μs |    3 | 1000.0000 | 1000.0000 | 1000.0000 |  3,838 KB |
|         CleaningNodes | 22,891.525 μs | 4,396.4063 μs | 680.3487 μs |    4 | 1000.0000 | 1000.0000 | 1000.0000 |  5,569 KB |
