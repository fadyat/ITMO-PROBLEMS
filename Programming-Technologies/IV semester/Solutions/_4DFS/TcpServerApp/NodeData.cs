using System.Collections.Immutable;
using System.Net;
using System.Net.NetworkInformation;

namespace TcpServerApp;

public class NodeData
{
    public IPAddress IpAddress { get; }
    public int Port { get; }
    public int Size { get; }

    private NodeData(string ip, string port, string size)
    {
        IpAddress = IPAddress.Parse(ip);
        Port = Convert.ToInt32(port);
        Size = Convert.ToInt32(size);
        CheckTcpConnection();
    }

    public NodeData(IReadOnlyList<string> args)
        : this(args[0], args[1], args[2])
    {
        
    }
    
    private void CheckTcpConnection()
    {
        var active = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners().ToImmutableList();
        var correctNode = active.Any(x => Equals(x.Address, IpAddress) & Equals(x.Port, Port));

        if (!correctNode)
        {
            throw new FormatException($"Node with TcpListener '{IpAddress}:{Port}' doesn't exist!");
        }
    }

    public override string ToString()
    {
        return $"{IpAddress}:{Port}";
    }
}