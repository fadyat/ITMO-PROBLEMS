namespace TcpClientApp;

public static class Program
{
    private static void Main()
    {
        try
        {
            var service = new NodeService();
            service.Start();
            while (service.Active)
            {
                service.AnalyzeRequests();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}