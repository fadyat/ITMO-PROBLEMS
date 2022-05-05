using System.Collections.Immutable;
using AntlrParser.Analysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslyn.Generation;

public static class ServiceGeneration
{
    public static string Generate(ImmutableList<MethodDeclaration> methodsDeclaration, string serviceName)
    {
        var namespaceDeclaration = SyntaxFactory.NamespaceDeclaration(
            SyntaxFactory.ParseName($"{serviceName}Generated")
        );

        var clientValue = SyntaxFactory.EqualsValueClause(
            SyntaxFactory.ObjectCreationExpression(
                SyntaxFactory.Token(SyntaxKind.NewKeyword),
                SyntaxFactory.ParseTypeName("HttpClient"),
                SyntaxFactory.ArgumentList(),
                null
            ));
        
        var clientField = SyntaxFactory.FieldDeclaration(SyntaxFactory.VariableDeclaration(
            SyntaxFactory.ParseTypeName("HttpClient"),
            SyntaxFactory.SeparatedList(new[]
            {
                SyntaxFactory.VariableDeclarator(
                    SyntaxFactory.Identifier("client"), 
                    null,
                    clientValue
                )
            })))
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));

        var classDeclaration = SyntaxFactory.ClassDeclaration(serviceName)
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
            .AddMembers(clientField);


        methodsDeclaration.ForEach(method =>
        {
            var methodBody = SyntaxFactory.ParseStatement(
                $"return await client.{method.HttpMethodName}Async(\"{method.Url}\");"
            );

            var methodDeclaration = SyntaxFactory.MethodDeclaration(
                    SyntaxFactory.ParseTypeName(method.ReturnType), method.MethodName)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .WithBody(SyntaxFactory.Block(methodBody))
                .WithParameterList(SyntaxFactory.ParameterList())
                .NormalizeWhitespace();

            classDeclaration = classDeclaration.AddMembers(methodDeclaration);
        });

        namespaceDeclaration = namespaceDeclaration.AddMembers(classDeclaration)
            .NormalizeWhitespace();

        return namespaceDeclaration.ToFullString();
    }
}