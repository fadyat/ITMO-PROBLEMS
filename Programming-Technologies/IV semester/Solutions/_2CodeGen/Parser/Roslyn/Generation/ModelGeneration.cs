using AntlrParser.Analysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Roslyn.Generation;

public static class ModelGeneration
{
    public static string Generate(ModelDeclaration modelDeclaration)
    {
        var namespaceDeclaration = SyntaxFactory.NamespaceDeclaration(
            SyntaxFactory.ParseName($"{modelDeclaration.ModelName}Generated")
        );


        var classDeclaration = SyntaxFactory.ClassDeclaration($"{modelDeclaration.ModelName}")
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));


        var propertiesDeclaration = modelDeclaration.Arguments.Select(argument =>
            SyntaxFactory.PropertyDeclaration(
                    SyntaxFactory.ParseTypeName($"{argument.ArgumentType}"), argument.ArgumentName
                )
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddAccessorListAccessors(
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                        .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                        .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                )
        ).ToList();

        classDeclaration = propertiesDeclaration.Aggregate(
            classDeclaration, (current, property) => current.AddMembers(property)
        );


        namespaceDeclaration = namespaceDeclaration.AddMembers(classDeclaration)
            .NormalizeWhitespace();

        return namespaceDeclaration.ToFullString();
    }
}