using System.Net;

namespace TcpServerApp;

public static class Program
{
    private static void Main()
    {
        var serverIp = IPAddress.Parse("127.0.0.1");
        const int port = 8888;
        var server = new Server(serverIp, port);

        server.StartServer();
        try
        {
            while (true)
            {
                var stopServer = server.AnalyzeRequests();
                if (stopServer) break;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            server.StopServer();
        }
    }
}