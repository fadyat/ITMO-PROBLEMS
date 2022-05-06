using System.Text;
using AntlrParser.Analysis;
using AntlrParser.Visitors;
using Roslyn.Generation;

namespace Roslyn;

public static class Program
{
    private static void Main()
    {
        const string controllerPath = "../../../../../JavaServer/src/main/java/ru/artyomfadeyev/javaserver/controllers/StudentController.java";
        
        var serverMethodVisitor = new ServerMethodVisitor();
        ParsingSetup.Run(controllerPath, serverMethodVisitor);

        var result1 = serverMethodVisitor.PreviousMethodDeclarations;
        Console.WriteLine(ServiceGeneration.Generate(result1, "StudentService"));
      
        var getResponse = StudentService.getStudent(1);
        var getResponseBody = getResponse.Result.Content.ReadAsStringAsync().Result;
        
        Console.WriteLine(getResponseBody);

        // var newObject = new StringContent(File.ReadAllText("../../../postThis.json"), Encoding.UTF8, "application/json");
        // var postResponse = StudentService.saveStudent(newObject);
        
        // Console.WriteLine(postResponse.Result);

        var getResponseOfStudents = StudentService.getStudents(new List<int> {1, 3});
        var getResponseOfStudentsBody = getResponseOfStudents.Result.Content.ReadAsStringAsync().Result;
        
        Console.WriteLine(getResponseOfStudentsBody);
    }
}