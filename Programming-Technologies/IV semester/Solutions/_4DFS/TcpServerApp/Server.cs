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
        {"/clean-node", 1},         // <name>
        {"/stop-node", 1},          // <name>
        {"/balance-node", 0},
        {"/nodes-list", 0},
        {"/stop-server", 0}
    };

    private Dictionary<string, Node> _nodes;

    public Server(IPAddress ipAddress, int port)
    {
        _server = new TcpListener(ipAddress, port);
        _nodes = new Dictionary<string, Node>();
    }

    public void StartServer()
    {
        _server.Start();
    }

    public void StopServer()
    {
        _server.Stop();
    }

    public bool AnalyzeRequests()
    {
        // Console.WriteLine("Waiting for connections... ");
        //
        // var client = _server.AcceptTcpClient();
        // Console.WriteLine("Client connected. Completing a request...");
        //
        // var stream = client.GetStream();
        // const string response = "Hello world!";
        // var data = Encoding.UTF8.GetBytes(response);
        //
        // stream.Write(data, 0, data.Length);
        // Console.WriteLine($"Message sent: {response}\n");
        //         
        // stream.Close();
        // client.Close();
        
        Console.WriteLine("Enter command for server: ");
        var parsedCommand = Array.Empty<string>();
        while (!parsedCommand.Any() || !ServerOptions.ContainsKey(parsedCommand[0]) || 
               !Equals(ServerOptions[parsedCommand[0]], parsedCommand.Length - 1))
        { 
            var command = Console.ReadLine();
            if (string.IsNullOrEmpty(command)) continue;
            parsedCommand = command.Split(' ');
        }

        var mainCommand = parsedCommand[0];
        var args = parsedCommand[1..];
        var nodeName = args[1];

        if (Equals(mainCommand, "/stop-server")) return true;

        if (Equals(mainCommand, "/add-node"))
        {
            var newNode = new Node(args);
            _nodes.Add(nodeName, newNode);
            newNode.StartNode();
        }
        else if (Equals(mainCommand, "/nodes-list"))
        {
            _nodes.Values.ToImmutableList().ForEach(Console.WriteLine);
        }
        else if (Equals(mainCommand, "/stop-node"))
        {
            _nodes[nodeName].StopNode();
        }
        
        return false;
    }
}