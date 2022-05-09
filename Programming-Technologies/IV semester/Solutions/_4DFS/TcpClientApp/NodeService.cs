using System.Net.Sockets;
using Analyzer;

namespace TcpClientApp;

// You need to run this project in parallel run
public class NodeService : RequestAnalyzer
{
    private new static readonly Dictionary<string, int> Requests = new()
    {
        {"/new-node", 3}, // <fs_path> <ip> <port>
        {"/listen", 0},
        {"/stop", 0},
    };

    private Node? _currentNode;

    public NodeService()
    {
        Active = false;
        _currentNode = null;
    }

    public override void AnalyzeRequests()
    {
        Console.WriteLine("Enter command for node-service: ");
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
        
        if (Equals("/new-node", mainCommand))
        {
            CreateNode(parsedCommand[1..]);
        }
        else if (Equals("/listen", mainCommand))
        {
            Console.WriteLine("Enter listen mode!");
            Listen();
            Console.WriteLine("Out of listen mode!");
        }
        else if (Equals("/stop", mainCommand))
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

    private void CreateNode(IReadOnlyList<string> args)
    {
        try
        {
            var node = new Node(args);
            node.Start();
            _currentNode?.Stop();
            _currentNode = node;
            Console.WriteLine("Created node: '{0}'", node);
        }
        catch (Exception e) when (e is FileNotFoundException or SocketException or FormatException)
        {
            Console.WriteLine(e.Message);
        }
    }

    private void Listen()
    {
        _currentNode?.Listen();
    }

    public override void Stop()
    {
        base.Stop();
        _currentNode?.Stop();
    }
}