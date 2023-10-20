namespace TcpServerApp;

public static class Program
{
    private static void Main()
    {
        var server = new Server();
        server.Start();
        while (server.Active)
        {
            server.AnalyzeRequests();
        }
    }
}