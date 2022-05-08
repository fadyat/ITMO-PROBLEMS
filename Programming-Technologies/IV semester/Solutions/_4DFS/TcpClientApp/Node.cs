using System.Net;
using System.Net.Sockets;

namespace TcpClientApp;

public class Node
{
    private string _basePath;
    private readonly string _name;
    private readonly int _size;
    private readonly TcpListener _nodeTcpClient;
    private readonly int _reserved;

    private Node(string basePath, string name, string ipAddress, string port, string size)
    {
        if (!Directory.Exists(basePath))
        {
            throw new FileNotFoundException($"Directory '{basePath}' for node doesn't exists!");
        }

        _basePath = basePath;
        _name = name;
        _size = Convert.ToInt32(size);
        _nodeTcpClient = new TcpListener(IPAddress.Parse(ipAddress), Convert.ToInt32(port));
        _reserved = 0;
    }

    public Node(IReadOnlyList<string> args)
        : this(args[0], args[1], args[2], args[3], args[4])
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
        return $"{_name}: {_reserved}/{_size}";
    }
}