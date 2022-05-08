using System.Collections.Immutable;
using System.Net;
using System.Net.Sockets;
using TcpClientApp;

namespace TcpServerApp;

public class Server
{
    private readonly TcpListener _server;
    private static readonly Dictionary<string, int> ServerOptions = new ()
    {
        {"/add-node", 5},           // <base-path> <name> <ip> <port> <size>
        {"/add-file", 2},           // <file-path> <partial-path>
        {"/remove-file", 1},        // <file-path>
        {"/exec", 1},               // <file-path>
        {"/clean-node", 1},         // <node-name>
        {"/stop-node", 1},          // <node-name>
        {"/balance-node", 0},
        {"/nodes-list", 0},
        {"/stop-server", 0}
    };

    private readonly Dictionary<string, Node> _nodes;

    public bool Active { get; private set; }

    public Server(IPAddress ipAddress, int port)
    {
        _server = new TcpListener(ipAddress, port);
        _nodes = new Dictionary<string, Node>();
        Active = true;
    }

    public void StartServer()
    {
        _server.Start();
    }

    public void StopServer()
    {
        _server.Stop();
    }

    public void AnalyzeRequests()
    {
        Console.WriteLine("Enter command for server: ");
        var parsedCommand = ParseInputCommand();
        var mainCommand = parsedCommand[0];
        
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
        else if (Equals(mainCommand, "/stop-node"))
        {
            StopNode(parsedCommand[1]);
        }
        else if (Equals(mainCommand, "/balance-node"))
        {
            // ...
        }
        else if (Equals(mainCommand, "/nodes-list"))
        {
            NodesList();
        }
        else if (Equals(mainCommand, "/stop-server"))
        {
            Active = false;
        }
    }

    private static string[] ParseInputCommand()
    {
        var parsedCommand = Array.Empty<string>();
        while (!IsCorrectCommand(parsedCommand))
        { 
            var command = Console.ReadLine();
            if (string.IsNullOrEmpty(command)) continue;
            parsedCommand = command.Split(' ');
            if (!IsCorrectCommand(parsedCommand))
            {
                Console.WriteLine("Command '{0}' was incorrect!", command);
            }
        }

        return parsedCommand;
    }

    private static bool IsCorrectCommand(IReadOnlyList<string> parsedCommand)
    {
        return parsedCommand.Any() && ServerOptions.ContainsKey(parsedCommand[0]) && 
               Equals(ServerOptions[parsedCommand[0]], parsedCommand.Count - 1);
    }
    
    private void AddNode(IReadOnlyList<string> args)
    {
        try
        {
            var newNode = new Node(args);
            _nodes.Add(args[1], newNode);
            newNode.StartNode();
        }
        catch (Exception e) when (e is FileNotFoundException or SocketException or FormatException)
        {
            Console.WriteLine(e.Message);
        }
    }

    private void NodesList()
    {
        Console.WriteLine("All nodes: ");
        _nodes.Values.ToImmutableList().ForEach(Console.WriteLine);
    }

    private void StopNode(string nodeName)
    {
        try
        {
            _nodes[nodeName].StopNode();
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e.Message);
        }
    }
}