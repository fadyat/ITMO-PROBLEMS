using System.Net;

namespace TcpServerApp;

public static class Program
{
    private static void Main()
    {
        var server = new Server(IPAddress.Parse("127.0.0.1"), 8888);
        server.StartServer();
        try
        {
            while (server.Active)
            {
                server.AnalyzeRequests();
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