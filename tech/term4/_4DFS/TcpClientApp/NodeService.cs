using System.Collections.Immutable;
using System.Net.Sockets;
using Analyzer;

namespace TcpClientApp;

// You need to run this project in parallel run, if want many clients
public class NodeService
{
    private static readonly Dictionary<string, int> Requests = new()
    {
        {"/new-node", 3}, // <fs_path> <ip> <port>
        {"/listen", 0},
        {"/stop", 0}
    };

    private static readonly HashSet<string> ServerOptions = new()
    {
        "/save",
        "/remove",
        "/send"
    };
    
    public bool Active { get; private set; }

    private Node? _currentNode;

    public NodeService()
    {
        Active = false;
        _currentNode = null;
    }

    public void AnalyzeRequests()
    {
        Console.WriteLine("Enter command for node-service: ");
        var parsedCommand = new List<string>();
        var correct = false;
        while (!correct)
        {
            try
            {
                var command = Console.ReadLine();
                parsedCommand = CommandAnalyzer.ParseInputCommand(command).ToList();
                correct = CommandAnalyzer.IsCorrectCommand(Requests, parsedCommand);
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
                correct = false;
            }
        }

        CommandSelector(parsedCommand);
    }

    private void CommandSelector(IReadOnlyList<string> parsedCommand)
    {
        var mainCommand = parsedCommand[0];
        switch (mainCommand)
        {
            case "/new-node":
                CreateNode(parsedCommand[1], parsedCommand[2], parsedCommand[3]);
                break;
            case "/listen":
                Listen();
                break;
            case "/stop":
                Stop();
                break;
        }
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
        switch (option)
        {
            case "/save":
                Save(responses[0]!, responses[1]!);
                break;
            case "/remove":
                Remove(responses.ToImmutableList());
                break;
        }
    }

    public void Start()
    {
        Active = true;
    }

    public void Stop()
    {
        Active = false;
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