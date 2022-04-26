using AntlrParser.Analysis;
using AntlrParser.Visitors;

namespace AntlrParser;

public static class Program
{
    private static void Main()
    {
        const string controllerPath =
            "../../../../../JavaServer/src/main/java/ru/artyomfadeyev/javaserver/controllers/StudentController.java";

        var visitor1 = new ServerMethodVisitor();
        new ParsingSetup(controllerPath, visitor1)
            .Run();

        var result1 = visitor1.PreviousMethodDeclarations;
        Console.WriteLine(string.Join("\n", result1));

        const string studentPath =
            "../../../../../JavaServer/src/main/java/ru/artyomfadeyev/javaserver/classes/Student.java";
        var visitor2 = new ServerModelVisitor();
        new ParsingSetup(studentPath, visitor2)
            .Run();
        var result2 = visitor2.ModelDeclaration;
        Console.WriteLine(string.Join("\n", result2));

        const string socialsPath =
            "../../../../../JavaServer/src/main/java/ru/artyomfadeyev/javaserver/classes/Socials.java";

        var visitor3 = new ServerModelVisitor();
        new ParsingSetup(socialsPath, visitor3)
            .Run();
        var result3 = visitor3.ModelDeclaration;
        Console.WriteLine(string.Join("\n", result3));
    }
}