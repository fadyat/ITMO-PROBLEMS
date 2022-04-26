using AntlrParser.Analysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Roslyn.Generation;

public static class ServiceGeneration
{
    public static string Generate(IEnumerable<MethodDeclaration> methodsDeclaration, string serviceName)
    {
        var namespaceDeclaration = SyntaxFactory.NamespaceDeclaration(
            SyntaxFactory.ParseName($"{serviceName}Generated")
        );

        var classDeclaration = SyntaxFactory.ClassDeclaration(serviceName)
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));


        foreach (var method in methodsDeclaration)
        {
            var methodBody = SyntaxFactory.ParseStatement(
                $"return HttpClient().{method.HttpMethodName}(\"{method.Url}\");"
            );

            var methodDeclaration = SyntaxFactory.MethodDeclaration(
                    SyntaxFactory.ParseTypeName(method.ReturnType), method.MethodName)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .WithBody(SyntaxFactory.Block(methodBody))
                .NormalizeWhitespace();

            Console.WriteLine(methodDeclaration);
        }

        namespaceDeclaration = namespaceDeclaration.AddMembers(classDeclaration)
            .NormalizeWhitespace();

        return namespaceDeclaration.ToFullString();
    }
}