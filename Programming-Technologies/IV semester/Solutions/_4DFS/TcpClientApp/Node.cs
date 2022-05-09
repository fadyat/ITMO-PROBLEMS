using System.Net;
using System.Net.Sockets;
using System.Text;

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

    public void Start()
    {
        _nodeTcpClient.Start();
    }

    public void Stop()
    {
        Console.WriteLine("Node '{0}' stopped!", ToString());
        _nodeTcpClient.Stop();
    }

    // need to get file location
    public void Listen()
    {
        try
        {
            var client = _nodeTcpClient.AcceptTcpClient();
            var stream = client.GetStream();
            var data = new byte[256];
            var response = new StringBuilder();
            do
            {
                var bytes = stream.Read(data, 0, data.Length);
                response.Append(Encoding.UTF8.GetString(data, 0, bytes));
            } while (stream.DataAvailable);

            Console.WriteLine("Accept file from server: \n {0}", response);
            stream.Close();
            client.Close();
        }
        catch (SocketException e)
        {
            Console.WriteLine(e.Message);
        }
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