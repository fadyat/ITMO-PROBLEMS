using AntlrParser.Analysis;
using AntlrParser.Visitors;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace RoslynGenerator.Generation;

public static class ModelGeneration
{
    public static void Generate(ModelDeclaration modelDeclaration)
    {
        var namespaceDeclaration = SyntaxFactory.NamespaceDeclaration(
            SyntaxFactory.ParseName("RoslynGenerator.RoslynGenerated")
        );

        var classDeclaration = SyntaxFactory.ClassDeclaration($"{modelDeclaration.ModelName}")
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

        var propertiesDeclaration = modelDeclaration.Arguments.Select(argument =>
            SyntaxFactory.PropertyDeclaration(
                    SyntaxFactory.ParseTypeName($"{Transformer.MakeCorrectTypes(argument.ArgumentType)}"),
                    argument.ArgumentName
                )
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddAccessorListAccessors(
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                        .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                        .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                )
        );

        classDeclaration = propertiesDeclaration.Aggregate(
            classDeclaration, (current, property) => current.AddMembers(property)
        );


        namespaceDeclaration = namespaceDeclaration.AddMembers(classDeclaration)
            .NormalizeWhitespace();

        File.WriteAllText($"../../../RoslynGenerated/{modelDeclaration.ModelName}.cs", namespaceDeclaration.ToFullString());
    }
}