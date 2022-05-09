using System.Net.Sockets;
using Analyzer;

namespace TcpClientApp;

public class NodeService : RequestAnalyzer
{
    private new static readonly Dictionary<string, int> Requests = new()
    {
        {"/create-node", 3}, // <fs_path> <ip> <port>
        {"/stop", 0}
    };

    public NodeService()
    {
        Active = false;
    }

    public override void AnalyzeRequests()
    {
        Console.WriteLine("Enter command for node-service: ");
        string[]? parsedCommand = null;
        while (!IsCorrectCommand(parsedCommand))
        {
            parsedCommand = ParseInputCommand();
        }
        
        var mainCommand = parsedCommand![0];
        
        if (Equals("/create-node", mainCommand))
        {
            CreateNode(parsedCommand[1..]);
        }
        else if (Equals("/stop", mainCommand))
        {
            Stop();
        }
    }

    protected override bool IsCorrectCommand(IReadOnlyList<string>? parsedCommand)
    {
        return !Equals(parsedCommand, null) && Requests.Any() && Requests.ContainsKey(parsedCommand[0])
               && Equals(Requests[parsedCommand[0]], parsedCommand.Count - 1);
    }

    private static void CreateNode(IReadOnlyList<string> args)
    {
        try
        {
            var node = new Node(args);
            Console.WriteLine("Created node: '{0}'", node);
        }
        catch (Exception e) when (e is FileNotFoundException or SocketException or FormatException)
        {
            Console.WriteLine(e.Message);
        }
    }
}