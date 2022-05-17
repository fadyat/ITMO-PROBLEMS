using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using TcpServerApp;

namespace Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
// [SimpleJob(launchCount: 1, warmupCount: 5, targetCount: 5)]
[SimpleJob(launchCount: 1, warmupCount: 1, targetCount: 1)]
public class ServerBenchmarks
{
    // running node args
    private readonly (string name, string ip, string port, string size) _node = ("node", "127.0.0.1", "8889", "10000");

    // tmp file for benchmarking
    private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "../REMOVE_DIR/REMOVE_ME.txt");
    private const string FileText = "i'm text inside a new tmp file!";

    [GlobalSetup]
    public void Setup()
    {
        Directory.CreateDirectory(Directory.GetParent(_filePath)!.FullName);
        Console.WriteLine(_filePath);
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
        server.AnalyzeRequests($"/add-node {_node.name} {_node.ip} {_node.port} {_node.size}");
        server.Stop();
    }

    [Benchmark]
    public void FileAdding()
    {
        var server = new Server();
        server.Start();
        server.AnalyzeRequests($"/add-node {_node.name} {_node.ip} {_node.port} {_node.size}");
        server.AnalyzeRequests($"/add-file \"{_filePath}\" tmp/tmp.txt");
        server.Stop();
    }

    [Benchmark]
    public void FileAddingAndRemoving()
    {
        var server = new Server();
        server.Start();
        server.AnalyzeRequests($"/add-node {_node.name} {_node.ip} {_node.port} {_node.size}");
        server.AnalyzeRequests($"/add-file \"{_filePath}\" tmp/tmp.txt");
        server.AnalyzeRequests($"/remove-file \"{_filePath}\"");
        server.Stop();
    }

    [GlobalCleanup]
    public void CleanUp()
    {
        File.Delete(_filePath);
    }
}