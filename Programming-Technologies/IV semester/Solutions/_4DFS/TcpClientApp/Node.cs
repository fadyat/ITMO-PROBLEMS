using System.Net;
using System.Net.Sockets;

namespace TcpClientApp;

public class Node
{
    private readonly string _basePath;
    private readonly TcpListener _nodeTcpClient;
    private readonly IPAddress _ipAddress;
    private readonly int _port;

    private Node(string basePath, string ipAddress, string port)
    {
        if (!Directory.Exists(basePath))
        {
            throw new FileNotFoundException($"Directory '{basePath}' for node doesn't exists!");
        }

        _basePath = basePath;
        _ipAddress = IPAddress.Parse(ipAddress);
        _port = Convert.ToInt32(port);
        _nodeTcpClient = new TcpListener(_ipAddress, _port);
    }

    public Node(IReadOnlyList<string> args)
        : this(args[0], args[1], args[2])
    {
        
    }

    public void StartNode()
    {
        _nodeTcpClient.Start();
    }

    public void StopNode()
    {
        _nodeTcpClient.Stop();
    }

    public override string ToString()
    {
        return $"{_ipAddress}:{_port} | {_basePath}";
    }

    public bool ExistsDirectory(string? directoryPath)
    {
        return Equals(directoryPath, null) || Directory.Exists(Path.Combine(_basePath, directoryPath));
    }
}