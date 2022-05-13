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
        {"/stop", 0}
    };

    private static readonly HashSet<string> ServerOptions = new()
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
                var command = Console.ReadLine();
                parsedCommand = ParseInputCommand(command);
                correct = IsCorrectCommand(parsedCommand);
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
                correct = false;
            }
        }

        CommandSelector(parsedCommand);
    }

    protected override void CommandSelector(string[]? parsedCommand)
    {
        var mainCommand = parsedCommand![0];
        if (mainCommand == "/new-node")
        {
            CreateNode(parsedCommand[1], parsedCommand[2], parsedCommand[3]);
        }
        else if (mainCommand == "/listen")
        {
            Listen();
        }
        else if (mainCommand == "/stop")
        {
            Stop();
        }
    }

    protected override bool IsCorrectCommand(IReadOnlyList<string>? parsedCommand)
    {
        var correctMainCommand = parsedCommand != null && Requests.Any() && Requests.ContainsKey(parsedCommand[0]);
        if (!correctMainCommand)
        {
            throw new FormatException("Unknown command!");
        }

        var correctArgs = Equals(Requests[parsedCommand![0]], parsedCommand.Count - 1);
        if (!correctArgs)
        {
            throw new FormatException($"Expected {Requests[parsedCommand[0]]} args, was {parsedCommand.Count - 1}");
        }

        return correctMainCommand & correctArgs;
    }

    private void CreateNode(string fsPath, string ipAddress, string port)
    {
        try
        {
            var node = new Node(fsPath, ipAddress, port);
            node.Start();
            _currentNode?.Stop();
            _currentNode = node;
        }
        catch (Exception e) when (e is FileNotFoundException or SocketException or FormatException)
        {
            Console.WriteLine($"\t{e.Message}");
        }
    }

    // fix null
    private void Listen()
    {
        while (true)
        {
            var data = _currentNode?.Listen()?.Split(' ');
            if (data == null) return;

            var option = data[0];
            if (!IsCorrectServerOption(option)) return;

            var listenCnt = Convert.ToInt32(data[1]);
            var responses = new List<string?>();
            for (var i = 0; i < listenCnt; i++)
            {
                responses.Add(_currentNode?.Listen());
            }

            OptionSelector(option, responses);
        }
    }

    private void OptionSelector(string option, IReadOnlyList<string?> responses)
    {
        if (option == "/save")
        {
            Save(responses[0]!, responses[1]!);
        }
        else if (option == "/remove")
        {
            Remove(responses.ToImmutableList());
        }
    }

    protected override void Stop()
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
        return option != null && ServerOptions.Contains(option);
    }
}