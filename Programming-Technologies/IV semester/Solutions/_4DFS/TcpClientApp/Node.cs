using System.Collections.Immutable;
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

    public Node(string basePath, string ipAddress, string port)
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

    public void Start()
    {
        _nodeTcpClient.Start();
        Console.WriteLine("\tNode '{0}' started!", ToString());
    }

    public void Stop()
    {
        _nodeTcpClient.Stop();
        Console.WriteLine("\tNode '{0}' stopped!", ToString());
    }

    public string? Listen()
    {
        try
        {
            var client = _nodeTcpClient.AcceptTcpClient();
            var stream = client.GetStream();
            var data = new byte[256];
            var response = new StringBuilder();
            do
            {
                var bytes = stream.ReadAsync(data, 0, data.Length).Result;
                response.Append(Encoding.UTF8.GetString(data, 0, bytes));
            } while (stream.DataAvailable);

            var responseQuick = response.ToString()[..Math.Min(10, response.Length)];
            if (response.Length > 10)
            {
                responseQuick = string.Concat(responseQuick, "...");
            }

            Console.WriteLine("\tAccepted data from server: \n\t'{0}'", responseQuick);
            stream.Close();
            client.Close();
            return response.ToString();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }

    public void Save(string fsPath, string data)
    {
        try
        {
            var actualPath = Path.Combine(_basePath, fsPath);
            var directory = Path.GetDirectoryName(actualPath);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using var fs = File.Create(actualPath);
            {
                var bytes = Encoding.ASCII.GetBytes(data);
                fs.WriteAsync(bytes, 0, bytes.Length);
            }

            Console.WriteLine($"\t'{actualPath}' saved!");
        }
        catch (Exception e)
        {
            Console.WriteLine($"\t{e.Message}");
        }
    }

    public void Remove(ImmutableList<string?> files)
    {
        files.ForEach(fsPath =>
        {
            try
            {
                var actualPath = Path.Combine(_basePath, fsPath!);
                File.Delete(actualPath);
                // ... remove empty directories
                Console.WriteLine($"\t'{actualPath}' removed!");
            }
            catch (Exception e)
            {
                Console.WriteLine($"\t{e.Message}");
            }
        });
    }
    

    public override string ToString()
    {
        return $"{_ipAddress}:{_port} | {_basePath}";
    }
}