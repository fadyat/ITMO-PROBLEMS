using System.Collections.Immutable;
using AntlrParser.Analysis;
using AntlrParser.Visitors;
using RoslynGenerator.Generation;
using RoslynGenerator.RoslynGenerated;


namespace RoslynGenerator;

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

        const string classesPath = "../../../../../JavaServer/src/main/java/ru/artyomfadeyev/javaserver/classes";
        Directory.GetFiles(classesPath).ToImmutableList().ForEach(classPath =>
        {
            var serverModelVisitor = new ServerModelVisitor();
            ParsingSetup.Run(classPath, serverModelVisitor);
            var result2 = serverModelVisitor.ModelDeclaration;

            // generate classes for client
            ModelGeneration.Generate(result2);
        });
        
        var s = StudentServiceGenerated.getStudent(1);
        Console.WriteLine($"{s.id}, {s.name}, {s.socials.tg}\n");

        var tmpStudent = new Student
        {
            id = 6,
            name = "Aboba",
            socials = new Socials
            {
                tg = "@aboba_ya"
            }
        };

        StudentServiceGenerated.saveStudent(tmpStudent);
        var students = StudentServiceGenerated.getStudents(null);
        students?.ForEach(student => { Console.WriteLine($"{student.id}, {student.name}, {student.socials.tg}"); });
    }
}