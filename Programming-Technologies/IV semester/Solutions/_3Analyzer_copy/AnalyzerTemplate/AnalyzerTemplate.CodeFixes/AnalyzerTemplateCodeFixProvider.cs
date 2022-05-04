using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AnalyzerTemplate;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AnalyzerTemplateCodeFixProvider))]
[Shared]
public class AnalyzerTemplateCodeFixProvider : CodeFixProvider
{
    public sealed override ImmutableArray<string> FixableDiagnosticIds =>
        ImmutableArray.Create(AnalyzerTemplateAnalyzer.DiagnosticId);

    public sealed override FixAllProvider GetFixAllProvider()
    {
        return WellKnownFixAllProviders.BatchFixer;
    }

    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;
        var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf()
            .OfType<FieldDeclarationSyntax>().First();

        context.RegisterCodeFix(
            CodeAction.Create(
                CodeFixResources.CodeFixTitle,
                c => ChangeOnLocalVariable(context.Document, declaration, c),
                nameof(CodeFixResources.CodeFixTitle)),
            diagnostic);
    }

    private static async Task<Document> ChangeOnLocalVariable(Document document,
        BaseFieldDeclarationSyntax fieldDeclaration,
        CancellationToken cancellationToken)
    {
        // var root = await document.GetSyntaxRootAsync(cancellationToken);
        // root = root.RemoveNode(fieldDeclaration, SyntaxRemoveOptions.KeepExteriorTrivia)
        //    .NormalizeWhitespace();

        // document = document.WithSyntaxRoot(root);

        var fieldName = fieldDeclaration.Declaration.Variables.First();

        /*var semanticModel = await document.GetSemanticModelAsync(cancellationToken);
        var typeSymbol = semanticModel.GetDeclaredSymbol(fieldName, cancellationToken);
        
        var newSolution = await Renamer.RenameSymbolAsync(
                document.Project.Solution, typeSymbol, newName,
                document.Project.Solution.Workspace.Options,
                cancellationToken)
            .ConfigureAwait(false);

        document = newSolution.GetDocument(document.Id);
        
        var root = await document!.GetSyntaxRootAsync(cancellationToken)!;
        var fieldToRemove = root.FindNode(fieldDeclaration.Span);
        root = root.RemoveNode(fieldToRemove, SyntaxRemoveOptions.KeepExteriorTrivia).NormalizeWhitespace();
        document = document.WithSyntaxRoot(root);
        newSolution = document.Project.Solution;*/


        var root = await document.GetSyntaxRootAsync(cancellationToken);

        var markedMethods = root.DescendantNodes()
            .OfType<MethodDeclarationSyntax>()
            .ToImmutableList();

        markedMethods.ForEach(markedMethod =>
        {
            var variablesAssignment =
                markedMethod.DescendantNodes().OfType<AssignmentExpressionSyntax>().ToImmutableList();
            var methodArgs = markedMethod.ParameterList.Parameters.ToImmutableList();
            var potentialLeftAssignment = fieldName.Identifier.ToString();

            var replaceOn = methodArgs.FirstOrDefault(arg =>
            {
                return variablesAssignment.Any(assignment =>
                {
                    var leftAssignment = assignment.Left.ToString();
                    var leftEquals = Equals(leftAssignment, potentialLeftAssignment);
                    var potentialRightAssignment = arg.Identifier.ToString();
                    var rights = assignment.Right.DescendantNodesAndSelf().OfType<IdentifierNameSyntax>()
                        .ToImmutableList();

                    return rights.Any(x =>
                    {
                        var rightAssignment = x.Identifier.ToString();
                        var rightEquals = Equals(rightAssignment, potentialRightAssignment);
                        return leftEquals && rightEquals;
                    });
                });
            });

            if (replaceOn == null) return;

            var methodToUpdate = root.DescendantNodes().OfType<MethodDeclarationSyntax>()
                .FirstOrDefault(x => Equals(x.Identifier.ToString(), markedMethod.Identifier.ToString()));

            var replaceThis = methodToUpdate!.DescendantNodes().OfType<IdentifierNameSyntax>()
                .Where(x => Equals(x.Identifier.ToString(), fieldName.Identifier.ToString()))
                .ToImmutableList();

            var updatedMethod = methodToUpdate;
            replaceThis.ForEach(x =>
            {
                var xx = updatedMethod.DescendantNodesAndSelf().OfType<IdentifierNameSyntax>()
                    .FirstOrDefault(y => Equals(x.Identifier.ToString(), y.Identifier.ToString()));

                updatedMethod = updatedMethod.ReplaceToken(xx!.Identifier, replaceOn.Identifier);
            });

            var removeThis = updatedMethod.DescendantNodesAndSelf().OfType<AssignmentExpressionSyntax>()
                .FirstOrDefault(x =>
                    Equals(x.Left.ToString(), replaceOn.Identifier.ToString()) &&
                    Equals(x.Right.ToString(), replaceOn.Identifier.ToString()))?
                .AncestorsAndSelf()
                .OfType<ExpressionStatementSyntax>().FirstOrDefault();

            if (removeThis != null)
                updatedMethod = updatedMethod.RemoveNode(removeThis, SyntaxRemoveOptions.KeepTrailingTrivia)
                    .NormalizeWhitespace();
            
            root = root.ReplaceNode(methodToUpdate, updatedMethod).NormalizeWhitespace();
        });

        var removeThis = root.DescendantNodes().OfType<FieldDeclarationSyntax>().FirstOrDefault(x =>
            Equals(x.Declaration.Variables.First().Identifier.ToString(), fieldName.Identifier.ToString()));


        root = root.RemoveNode(removeThis, SyntaxRemoveOptions.KeepTrailingTrivia).NormalizeWhitespace();

        return document.WithSyntaxRoot(root);
    }
}