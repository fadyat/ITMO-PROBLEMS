namespace AntlrCSharp;

public static class Program
{
    private static void Main()
    {
        const string controllerPath =
            "../../../../../JavaServer/src/main/java/ru/artyomfadeyev/javaserver/controllers/StudentController.java";
        var studentController = File.ReadAllText(controllerPath);
        Console.WriteLine(studentController);
    }
}