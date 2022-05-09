using System.Collections.Immutable;
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
        var correct = false;
        while (!correct)
        {
            try
            {
                parsedCommand = ParseInputCommand();
                correct = IsCorrectCommand(parsedCommand);
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
                correct = false;
            }
        }
        
        var mainCommand = parsedCommand![0];
        
        if (Equals(mainCommand, "/add-node"))
        {
            AddNode(parsedCommand[1..]);
        }
        else if (Equals(mainCommand, "/add-file"))
        {
            AddFile(parsedCommand[1..]);
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
        var correctMainCommand = !Equals(parsedCommand, null) && Requests.Any() && Requests.ContainsKey(parsedCommand[0]);
        if (!correctMainCommand) throw new FormatException("Unknown command!");
        
        var correctArgs = Equals(Requests[parsedCommand![0]], parsedCommand.Count - 1);
        if (!correctArgs) throw new FormatException($"Expected {Requests[parsedCommand[0]]} args, was {parsedCommand.Count - 1}");

        return correctMainCommand & correctArgs;
    }
    
    private void AddNode(string[] args)
    {
        try
        {
            var nodeName = args[0];
            var nodeData = new NodeData(args[1..]);
            _nodes.Add(nodeName, nodeData);
            Console.WriteLine("'{0}' with '{1}' successfully added to connected list!", nodeName, nodeData);
        }
        catch (Exception e) when (e is FileNotFoundException or SocketException or FormatException)
        {
            Console.WriteLine(e.Message);
        }
    }

    private void AddFile(IReadOnlyList<string> args)
    {
        var fsPath = args[0];
        var partialPath = args[1];
        
        // check that node have directory for partialPath
        // find one node, don't do for all nodes
        // try to do: async and await ?
        // how to send new file location ?
        _nodes.Values.ToImmutableList().ForEach(node =>
        {
            try
            {
                var client = new TcpClient();
                client.Connect(node.IpAddress, node.Port);
                var stream = client.GetStream();
                var data = File.ReadAllBytes(fsPath);
                stream.Write(data, 0, data.Length);
                stream.Close();
                client.Close();
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.Message);
            }
        });
    }
}