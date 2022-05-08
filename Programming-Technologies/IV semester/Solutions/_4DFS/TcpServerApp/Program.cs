using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TcpServerApp;

public static class Program
{
    private static void Main()
    {
        TcpListener? server = null;
        const int port = 8888;
        try
        {
            var serverIp = IPAddress.Parse("127.0.0.1");
            server = new TcpListener(serverIp, port);
            server.Start();

            while (true)
            {
                Console.WriteLine("Waiting for connections... ");
                
                var client = server.AcceptTcpClient();
                Console.WriteLine("Client connected. Completing a request...");

                var stream = client.GetStream();
                const string response = "Hello world!";
                var data = Encoding.UTF8.GetBytes(response);

                stream.Write(data, 0, data.Length);
                Console.WriteLine($"Message sent: {response}\n");
                
                stream.Close();
                client.Close();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            server?.Stop();
        }
    }
}