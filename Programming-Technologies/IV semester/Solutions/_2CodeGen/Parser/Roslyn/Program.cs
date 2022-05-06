using System.Text;
using AntlrParser.Analysis;
using AntlrParser.Visitors;
using Roslyn.Generation;
using Roslyn.RoslynGenerated;

namespace Roslyn;

public static class Program
{
    private static void Main()
    {
        const string controllerPath = "../../../../../JavaServer/src/main/java/ru/artyomfadeyev/javaserver/controllers/StudentController.java";
        
        var serverMethodVisitor = new ServerMethodVisitor();
        ParsingSetup.Run(controllerPath, serverMethodVisitor);

        var result1 = serverMethodVisitor.PreviousMethodDeclarations;
        
        // generate client class
        ServiceGeneration.Generate(result1, "StudentService");
        
        var getResponse = StudentServiceGenerated.getStudent(1);
        var getResponseBody = getResponse.Result.Content.ReadAsStringAsync().Result;
        
        Console.WriteLine(getResponseBody);

        var newObject = new StringContent(File.ReadAllText("../../../postThis.json"), Encoding.UTF8, "application/json");
        var postResponse = StudentServiceGenerated.saveStudent(newObject);
        Console.WriteLine(postResponse.Result);

        var getResponseOfStudents = StudentServiceGenerated.getStudents(new List<int>());
        var getResponseOfStudentsBody = getResponseOfStudents.Result.Content.ReadAsStringAsync().Result;
        
        Console.WriteLine(getResponseOfStudentsBody);
    }
}