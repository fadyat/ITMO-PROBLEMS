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
            SyntaxFactory.ParseName("Roslyn.RoslynGenerated")
        );

        var className = $"{serviceName}Generated";
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
                        SyntaxFactory.Identifier("Client"),
                        null,
                        clientValue
                    )
                })))
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PrivateKeyword),
                SyntaxFactory.Token(SyntaxKind.StaticKeyword),
                SyntaxFactory.Token(SyntaxKind.ReadOnlyKeyword));


        var localHostValue = SyntaxFactory.EqualsValueClause(
            SyntaxFactory.ParseExpression("\"http://localhost:8080\"")
        );


        var localHostField = SyntaxFactory.FieldDeclaration(SyntaxFactory.VariableDeclaration(
                SyntaxFactory.ParseTypeName("string"),
                SyntaxFactory.SeparatedList(new[]
                {
                    SyntaxFactory.VariableDeclarator(
                        SyntaxFactory.Identifier("LocalHost"),
                        null,
                        localHostValue
                    )
                })))
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PrivateKeyword),
                SyntaxFactory.Token(SyntaxKind.ConstKeyword));


        var classDeclaration = SyntaxFactory.ClassDeclaration(className)
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                SyntaxFactory.Token(SyntaxKind.StaticKeyword))
            .AddMembers(clientField, localHostField);


        methodsDeclaration.ForEach(method =>
        {
            var methodBody = MethodBody(method);

            var parameterList = MethodParameters(method);


            var methodDeclaration = SyntaxFactory.MethodDeclaration(
                    SyntaxFactory.ParseTypeName("Task<HttpResponseMessage>"), method.MethodName!)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                    SyntaxFactory.Token(SyntaxKind.StaticKeyword),
                    SyntaxFactory.Token(SyntaxKind.AsyncKeyword))
                .WithBody(SyntaxFactory.Block(methodBody))
                .WithParameterList(parameterList)
                .NormalizeWhitespace();

            classDeclaration = classDeclaration.AddMembers(methodDeclaration);
        });

        namespaceDeclaration = namespaceDeclaration.AddMembers(classDeclaration)
            .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.QualifiedName(
                SyntaxFactory.IdentifierName("System"),
                SyntaxFactory.IdentifierName("Web")
                )))
            .NormalizeWhitespace();

        File.WriteAllText($"../../../RoslynGenerated/{className}.cs", namespaceDeclaration.ToFullString());
        return namespaceDeclaration.ToFullString();
    }

    private static StatementSyntax[] MethodBody(MethodDeclaration method)
    {
        StatementSyntax[] methodBody;
        if (Equals(method.MethodName, "getStudent"))
        {
            methodBody = new[]
            {
                SyntaxFactory.ParseStatement(
                    $"return await Client.{method.HttpMethodName}Async($\"{{LocalHost}}{method.Url}\");"),
            };
        }
        else if (Equals(method.MethodName, "getStudents"))
        {
            
            var returnUrlStatement = SyntaxFactory.ParseStatement("return await Client.GetAsync(url);");
            methodBody = new []
            {
                SyntaxFactory.ParseStatement("var url = $\"{LocalHost}/api/student\";"),
                SyntaxFactory.IfStatement(SyntaxFactory.ParseExpression("!ids.Any()"),
                    SyntaxFactory.Block(returnUrlStatement)),
                SyntaxFactory.ParseStatement("var uriBuilder = new UriBuilder(url);"),
                SyntaxFactory.ParseStatement("var query = HttpUtility.ParseQueryString(uriBuilder.Query);"),
                SyntaxFactory.ParseStatement("ids.ForEach(id => query.Add(\"id\",  $\"{id}\"));"),
                SyntaxFactory.ParseStatement("uriBuilder.Query = query.ToString();"),
                SyntaxFactory.ParseStatement("url = uriBuilder.Uri.ToString();"),
                returnUrlStatement
            };
        }
        else
        {
            methodBody = new[]
            {
                SyntaxFactory.ParseStatement(
                    $"return await Client.{method.HttpMethodName}Async($\"{{LocalHost}}{method.Url}\", content);")
            };
        }

        return methodBody;
    }

    private static ParameterListSyntax MethodParameters(MethodDeclaration method)
    {
        ParameterListSyntax methodParameters;
        if (Equals(method.HttpMethodName, "Post"))
        {
            methodParameters = SyntaxFactory.ParameterList()
                .AddParameters(SyntaxFactory.Parameter(SyntaxFactory.Identifier("content"))
                    .WithType(SyntaxFactory.ParseTypeName("StringContent")));
        }
        else
        {
            methodParameters = method.Arguments!.Aggregate(SyntaxFactory.ParameterList(), (current, arg) =>
                current.AddParameters(SyntaxFactory.Parameter(SyntaxFactory.Identifier(arg.ArgumentType))
                    .WithType(SyntaxFactory.ParseTypeName(arg.ArgumentName))));
        }
        
        return methodParameters;
    }
}

