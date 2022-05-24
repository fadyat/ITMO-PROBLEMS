using BenchmarkDotNet.Running;

namespace Benchmarks;

public static class Program
{
    public static void Main()
    {
        // dotnet build -c Release
        // dotnet ./Benchmarks/bin/Release/net6.0/Benchmarks.dll
        BenchmarkRunner.Run<ServerBenchmarks>();
    }
}