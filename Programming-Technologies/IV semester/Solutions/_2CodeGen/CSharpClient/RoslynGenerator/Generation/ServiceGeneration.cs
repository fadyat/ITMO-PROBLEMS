using System.Collections.Immutable;
using AntlrParser.Analysis;
using AntlrParser.Visitors;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RoslynGenerator.Generation;

public static class ServiceGeneration
{
    public static void Generate(ImmutableList<MethodDeclaration> methodsDeclaration, string serviceName)
    {
        var namespaceDeclaration = SyntaxFactory.NamespaceDeclaration(
            SyntaxFactory.ParseName("RoslynGenerator.RoslynGenerated")
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

            var parameterList = method.Arguments!.Aggregate(SyntaxFactory.ParameterList(), (current, arg) =>
                current.AddParameters(SyntaxFactory.Parameter(SyntaxFactory.Identifier(arg.ArgumentType))
                    .WithType(SyntaxFactory.ParseTypeName(Transformer.ReturnTypeWithNull(arg.ArgumentName)!))));

            var methodDeclaration = SyntaxFactory
                .MethodDeclaration(SyntaxFactory.ParseTypeName($"{Transformer.ReturnTypeWithNull(method.ReturnType)}"), method.MethodName!)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                    SyntaxFactory.Token(SyntaxKind.StaticKeyword))
                .WithBody(SyntaxFactory.Block(methodBody))
                .WithParameterList(parameterList)
                .NormalizeWhitespace();

            classDeclaration = classDeclaration.AddMembers(methodDeclaration);
        });

        var usingDirective = new[]
        {
            SyntaxFactory.UsingDirective(SyntaxFactory.QualifiedName(
                SyntaxFactory.IdentifierName("System"),
                SyntaxFactory.IdentifierName("Web"))),
            SyntaxFactory.UsingDirective(SyntaxFactory.QualifiedName(
                SyntaxFactory.IdentifierName("Newtonsoft"),
                SyntaxFactory.IdentifierName("Json"))),
            SyntaxFactory.UsingDirective(SyntaxFactory.QualifiedName(
                SyntaxFactory.IdentifierName("System"),
                SyntaxFactory.IdentifierName("Text")))
        };

        namespaceDeclaration = namespaceDeclaration.AddMembers(classDeclaration)
            .AddUsings(usingDirective)
            .NormalizeWhitespace();

        File.WriteAllText($"../../../RoslynGenerated/{className}.cs", namespaceDeclaration.ToFullString());
    }

    private static StatementSyntax[] MethodBody(MethodDeclaration method)
    {
        return method.MethodName switch
        {
            "getStudent" => new[]
            {
                SyntaxFactory.ParseStatement("var url = $\"{LocalHost}/api/student/{id}\";"),
                SyntaxFactory.ParseStatement("var response = Client.GetAsync(url);"),
                SyntaxFactory.ParseStatement("var responseString = response.Result.Content.ReadAsStringAsync().Result;"),
                SyntaxFactory.ParseStatement("return JsonConvert.DeserializeObject<Student>(responseString);")
            },
            "getStudents" => new[]
            {
                SyntaxFactory.ParseStatement("var url = $\"{LocalHost}/api/student\";"),
                SyntaxFactory.ParseStatement("var uriBuilder = new UriBuilder(url);"),
                SyntaxFactory.ParseStatement("var query = HttpUtility.ParseQueryString(uriBuilder.Query);"),
                SyntaxFactory.ParseStatement("ids?.ForEach(id => query.Add(\"id\", $\"{id}\"));"),
                SyntaxFactory.ParseStatement("uriBuilder.Query = query.ToString();"),
                SyntaxFactory.ParseStatement("url = uriBuilder.Uri.ToString();"),
                SyntaxFactory.ParseStatement("var response = Client.GetAsync(url);"),
                SyntaxFactory.ParseStatement("var responseString = response.Result.Content.ReadAsStringAsync().Result;"),
                SyntaxFactory.ParseStatement("return JsonConvert.DeserializeObject<List<Student>>(responseString);"),
            },
            "saveStudent" => new[]
            {
                SyntaxFactory.ParseStatement("var content = new StringContent(JsonConvert.SerializeObject(student), Encoding.UTF8, \"application/json\");"),
                SyntaxFactory.ParseStatement("Client.PostAsync($\"{LocalHost}/api/student\", content);")
            },
            _ => Array.Empty<StatementSyntax>()
        };
    }
}