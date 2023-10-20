using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using TcpServerApp;

namespace Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
[SimpleJob(launchCount: 1, warmupCount: 5, targetCount: 5)]
// [SimpleJob(launchCount: 1, warmupCount: 1, targetCount: 1)]
public class ServerBenchmarks
{
    // running node args
    private readonly (string name, string ip, string port, string size) _node8889 =
        ("node1", "127.0.0.1", "8889", "10000");

    private readonly (string name, string ip, string port, string size) _node8890 =
        ("node2", "127.0.0.1", "8890", "10000");

    // tmp file for benchmarking
    private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "../REMOVE_DIR/REMOVE_ME.txt");
    private const string FileText = "i'm text inside a new tmp file!";

    [GlobalSetup]
    public void Setup()
    {
        Directory.CreateDirectory(Directory.GetParent(_filePath)!.FullName);
        using var fs = File.Create(_filePath);
        {
            var bytes = Encoding.ASCII.GetBytes(FileText);
            fs.Write(bytes, 0, bytes.Length);
        }
    }

    [Benchmark]
    public void NodeAdding()
    {
        var server = new Server();
        server.Start();
        server.AnalyzeRequests($"/add-node {_node8889.name} {_node8889.ip} {_node8889.port} {_node8889.size}");
        server.Stop();
    }

    [Benchmark]
    public void FileAdding()
    {
        var server = new Server();
        server.Start();
        server.AnalyzeRequests($"/add-node {_node8889.name} {_node8889.ip} {_node8889.port} {_node8889.size}");
        server.AnalyzeRequests($"/add-file \"{_filePath}\" tmp/tmp.txt");
        server.Stop();
    }

    [Benchmark]
    public void FileAddingAndRemoving()
    {
        var server = new Server();
        server.Start();
        server.AnalyzeRequests($"/add-node {_node8889.name} {_node8889.ip} {_node8889.port} {_node8889.size}");
        server.AnalyzeRequests($"/add-file \"{_filePath}\" tmp/tmp.txt");
        server.AnalyzeRequests($"/remove-file \"{_filePath}\"");
        server.Stop();
    }

    [Benchmark]
    public void BalancingNodes()
    {
        var server = new Server();
        server.Start();
        server.AnalyzeRequests($"/add-node {_node8889.name} {_node8889.ip} {_node8889.port} {_node8889.size}");
        server.AnalyzeRequests($"/add-node {_node8890.name} {_node8890.ip} {_node8890.port} {_node8890.size}");
        for (var i = 0; i < 6; i++)
        {
            server.AnalyzeRequests($"/add-file \"{_filePath}\" tmp/tmp{i}.txt");
        }

        server.AnalyzeRequests("/balance-nodes");
        server.Stop();
    }

    [Benchmark]
    public void CleaningNodes()
    {
        var server = new Server();
        server.Start();
        server.AnalyzeRequests($"/add-node {_node8889.name} {_node8889.ip} {_node8889.port} {_node8889.size}");
        server.AnalyzeRequests($"/add-node {_node8890.name} {_node8890.ip} {_node8890.port} {_node8890.size}");

        // files will be added on first non-empty node -> _node8889 -> we can clean here
        for (var i = 0; i < 6; i++)
        {
            server.AnalyzeRequests($"/add-file \"{_filePath}\" tmp/tmp{i}.txt");
        }

        server.AnalyzeRequests($"/clean-node {_node8889.name}");
        server.Stop();
    }

    [GlobalCleanup]
    public void CleanUp()
    {
        File.Delete(_filePath);
    }
}