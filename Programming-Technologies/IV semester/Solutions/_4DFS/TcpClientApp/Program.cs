using System.Net.Sockets;
using System.Text;

namespace TcpClientApp;

internal static class Program
{
    private const int Port = 8888;
    private const string ServerIp = "127.0.0.1";

    private static void Main()
    {
        try
        {
            var server = new TcpClient();
            server.Connect(ServerIp, Port);

            var data = new byte[256];
            var response = new StringBuilder();
            var stream = server.GetStream();

            do
            {
                var bytes = stream.Read(data, 0, data.Length);
                response.Append(Encoding.UTF8.GetString(data, 0, bytes));
            } while (stream.DataAvailable);

            Console.WriteLine($"Message received: {response.ToString()}");

            stream.Close();
            server.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception: {e.Message}");
        }

        Console.WriteLine("Request completed...");
    }
}