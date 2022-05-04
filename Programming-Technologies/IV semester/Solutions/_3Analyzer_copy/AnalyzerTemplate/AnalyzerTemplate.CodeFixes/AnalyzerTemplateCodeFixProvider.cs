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

[Shared]
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AnalyzerTemplateCodeFixProvider))]
public class AnalyzerTemplateCodeFixProvider : CodeFixProvider
{
    public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(AnalyzerTemplateAnalyzer.DiagnosticId);

    public sealed override FixAllProvider GetFixAllProvider()
    {
        return WellKnownFixAllProviders.BatchFixer;
    }

    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;
        var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<FieldDeclarationSyntax>().First();

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
        var fieldName = fieldDeclaration.Declaration.Variables.First();
        var root = await document.GetSyntaxRootAsync(cancellationToken);
        var markedMethods = root.DescendantNodes().OfType<MethodDeclarationSyntax>().ToImmutableList();

        foreach (var markedMethod in markedMethods)
        {
            var replaceOn = FindAssignedParameter(fieldName, markedMethod);

            if (replaceOn == null) continue;
            
            var fieldUsages = markedMethod.DescendantNodes().OfType<IdentifierNameSyntax>()
                .Where(x => Equals(x.Identifier.ToString(), fieldName.Identifier.ToString()))
                .ToImmutableList();
            
            var correctMarkedMethod = root.DescendantNodes().OfType<MethodDeclarationSyntax>()
                .FirstOrDefault(x => Equals(x.Identifier.ToString(), markedMethod.Identifier.ToString()));
            
            var updatedCorrectMarkedMethod = correctMarkedMethod;
            foreach (var correctPrivateField in fieldUsages.Select(usage =>
                         updatedCorrectMarkedMethod?.DescendantNodesAndSelf().OfType<IdentifierNameSyntax>()
                             .FirstOrDefault(x => Equals(usage.Identifier.ToString(), x.Identifier.ToString()))))
            {
                updatedCorrectMarkedMethod = updatedCorrectMarkedMethod.ReplaceToken(correctPrivateField!.Identifier, replaceOn.Identifier);
            }

            var replacedAssignment = updatedCorrectMarkedMethod?.DescendantNodesAndSelf().OfType<AssignmentExpressionSyntax>()
                .FirstOrDefault(x =>
                    Equals(x.Left.ToString(), replaceOn.Identifier.ToString()) &&
                    Equals(x.Right.ToString(), replaceOn.Identifier.ToString()))?
                .AncestorsAndSelf()
                .OfType<ExpressionStatementSyntax>().FirstOrDefault();

            if (replacedAssignment != null)
            {
                updatedCorrectMarkedMethod = updatedCorrectMarkedMethod.RemoveNode(replacedAssignment, SyntaxRemoveOptions.KeepTrailingTrivia)
                    .NormalizeWhitespace();
            }
            
            root = root.ReplaceNode(correctMarkedMethod, updatedCorrectMarkedMethod).NormalizeWhitespace();
        }

        var fieldToRemove = root.DescendantNodes().OfType<FieldDeclarationSyntax>().FirstOrDefault(x =>
            Equals(x.Declaration.Variables.First().Identifier.ToString(), fieldName.Identifier.ToString()));
        
        root = root.RemoveNode(fieldToRemove, SyntaxRemoveOptions.KeepTrailingTrivia).NormalizeWhitespace();
        return document.WithSyntaxRoot(root);
    }

    private static ParameterSyntax FindAssignedParameter(VariableDeclaratorSyntax fieldName, BaseMethodDeclarationSyntax markedMethod)
    {
        var variablesAssignment = markedMethod.DescendantNodes().OfType<AssignmentExpressionSyntax>().ToImmutableList();
        var methodArgs = markedMethod.ParameterList.Parameters.ToImmutableList();
        var potentialLeftAssignment = fieldName.Identifier.ToString();

        var replaceOn = methodArgs.FirstOrDefault(arg =>
        {
            return variablesAssignment.Any(assignment =>
            {
                var leftAssignment = assignment.Left.ToString();
                var leftEquals = Equals(leftAssignment, potentialLeftAssignment);
                var potentialRightAssignment = arg.Identifier.ToString();
                var rights = assignment.Right.DescendantNodesAndSelf().OfType<IdentifierNameSyntax>().ToImmutableList();

                return rights.Any(x =>
                {
                    var rightAssignment = x.Identifier.ToString();
                    var rightEquals = Equals(rightAssignment, potentialRightAssignment);
                    return leftEquals & rightEquals;
                });
            });
        });

        return replaceOn;
    }
}