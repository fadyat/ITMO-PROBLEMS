using AntlrCSharp.Analysis;

namespace AntlrCSharp;

public static class Program
{
    private static void Main()
    {
        const string controllerPath =
            "../../../../../JavaServer/src/main/java/ru/artyomfadeyev/javaserver/controllers/StudentController.java";

        var parsingSetup = new ParsingSetup(controllerPath);
        parsingSetup.Run();
    }
}