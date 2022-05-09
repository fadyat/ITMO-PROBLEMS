using System.Net.Sockets;
using Analyzer;

namespace TcpServerApp;

public class Server : RequestAnalyzer
{
    private new static readonly Dictionary<string, int> Requests = new ()
    {
        {"/add-node", 4},           // <name> <ip> <port> <size>
        {"/add-file", 2},           // <file-path> <partial-path>
        {"/remove-file", 1},        // <file-path>
        {"/exec", 1},               // <file-path>
        {"/clean-node", 1},         // <node-name>
        {"/balance-node", 0},
        {"/stop", 0}
    };

    private readonly Dictionary<string, NodeData> _nodes;

    public Server()
    {
        Active = false;
        _nodes = new Dictionary<string, NodeData>();
    }

    public override void AnalyzeRequests()
    {
        Console.WriteLine("Enter command for server: ");
        string[]? parsedCommand = null;
        while (!IsCorrectCommand(parsedCommand))
        {
            parsedCommand = ParseInputCommand();
        }
        
        var mainCommand = parsedCommand![0];
        
        if (Equals(mainCommand, "/add-node"))
        {
            AddNode(parsedCommand[1..]);
        }
        else if (Equals(mainCommand, "/add-file"))
        {
            // ...
        }
        else if (Equals(mainCommand, "/remove-file"))
        {
            // ...
        }
        else if (Equals(mainCommand, "/exec"))
        {
            // ...
        }
        else if (Equals(mainCommand, "/clean-node"))
        {
            // ...
        }
        else if (Equals(mainCommand, "/balance-node"))
        {
            // ...
        }
        else if (Equals(mainCommand, "/stop"))
        {
            Stop();
        }
    }

    protected override bool IsCorrectCommand(IReadOnlyList<string>? parsedCommand)
    {
        return !Equals(parsedCommand, null) && parsedCommand.Any() && Requests.ContainsKey(parsedCommand[0]) && 
               Equals(Requests[parsedCommand[0]], parsedCommand.Count - 1);
    }
    
    private void AddNode(string[] args)
    {
        try
        {
            var nodeName = args[0];
            var nodeData = new NodeData(args[1..]);
            _nodes.Add(nodeName, nodeData);
        }
        catch (Exception e) when (e is FileNotFoundException or SocketException or FormatException)
        {
            Console.WriteLine(e.Message);
        }
    }
}