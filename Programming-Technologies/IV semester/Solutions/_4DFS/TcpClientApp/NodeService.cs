using System.Collections.Immutable;
using System.Net.Sockets;
using Analyzer;

namespace TcpClientApp;

// You need to run this project in parallel run, if want many clients
public class NodeService : RequestAnalyzer
{
    private new static readonly Dictionary<string, int> Requests = new()
    {
        {"/new-node", 3}, // <fs_path> <ip> <port>
        {"/listen", 0},
        {"/stop", 0},
    };

    private static readonly List<string> ServerOptions = new()
    {
        "/save",
        "/remove"
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
            Console.WriteLine("\tListen mode ON");
            Listen();
            Console.WriteLine("\tListen mode OFF");
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
        }
        catch (Exception e) when (e is FileNotFoundException or SocketException or FormatException)
        {
            Console.WriteLine(e.Message);
        }
    }
    
    // fix null
    private void Listen()
    {
        var data = _currentNode?.Listen()?.Split(' ');
        if (Equals(data, null)) return;
        
        var option = data[0];
        if (!IsCorrectServerOption(option))
        {
            Console.WriteLine("Unknown option");
            return;
        }
        
        var listenCnt = Convert.ToInt32(data[1]);
        var responses = new List<string?>();
        for (var i = 0; i < listenCnt; i++) responses.Add(_currentNode?.Listen());

        if (Equals(option, "/save"))
        {
            Console.WriteLine("\tSave option ON");
            Save(responses[0]!, responses[1]!);
            Console.WriteLine("\tSave option OFF");
        }
        else if (Equals(option, "/remove"))
        {
            Console.WriteLine("\tRemove option ON");
            Remove(responses.ToImmutableList());
            Console.WriteLine("\tRemove option OFF");
        }
    }

    public override void Stop()
    {
        base.Stop();
        _currentNode?.Stop();
    }

    private void Save(string fsPath, string data)
    {
        _currentNode?.Save(fsPath, data);
    }

    private void Remove(ImmutableList<string?> files)
    {
        _currentNode?.Remove(files);
    }

    private static bool IsCorrectServerOption(string? option)
    {
        return !Equals(option, null) && ServerOptions.Contains(option);
    }
}