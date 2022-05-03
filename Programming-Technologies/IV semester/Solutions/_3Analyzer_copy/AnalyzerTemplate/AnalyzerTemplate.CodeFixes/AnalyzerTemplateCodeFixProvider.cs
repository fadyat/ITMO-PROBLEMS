using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
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
        var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<FieldDeclarationSyntax>().First();

        context.RegisterCodeFix(
           CodeAction.Create(
              title: CodeFixResources.CodeFixTitle,
              createChangedDocument: c => ChangeOnLocalVariable(context.Document, declaration, c),
              equivalenceKey: nameof(CodeFixResources.CodeFixTitle)),
         diagnostic);
    }

    private static async Task<Document> ChangeOnLocalVariable(Document document, BaseFieldDeclarationSyntax fieldDeclaration,
        CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken);
        
        var methodsDeclaration = fieldDeclaration.Parent.AncestorsAndSelf()
            .OfType<MethodDeclarationSyntax>()
            .ToImmutableList();

        var fieldName = fieldDeclaration.Declaration.Variables.First();

        root = root.RemoveNode(fieldDeclaration, SyntaxRemoveOptions.KeepExteriorTrivia).NormalizeWhitespace();

        /*var classDeclaration = fieldDeclaration.Parent.AncestorsAndSelf().OfType<ClassDeclarationSyntax>().First();
        int i = 0;
        while (i++ < 10)
        {
            var replaceThis = classDeclaration.DescendantNodes()
                .OfType<IdentifierNameSyntax>()
                .FirstOrDefault(x => Equals(x.Identifier.ToString(), fieldName.Identifier.ToString()));

            if (replaceThis == null) break;

            root = root.ReplaceNode(root.FindNode(replaceThis.Span), SyntaxFactory.IdentifierName("uwu"));
        }
        */

        /*
        methodsDeclaration.ForEach(method =>
        {
            var variablesAssignment = method.DescendantNodes().OfType<AssignmentExpressionSyntax>().ToImmutableList();
            var methodArgs = method.ParameterList.Parameters.ToImmutableList();

            var potentialLeftAssignment = fieldName.Identifier.ToString();
            var assignedArgument = methodArgs.FirstOrDefault(arg =>
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
                        return leftEquals && rightEquals;
                    });
                });
            });

            var methodTokens = method.DescendantNodes().OfType<IdentifierNameSyntax>()
                .Where(token =>
                {
                    var isPrivateField = Equals(token.Identifier.ToString(), fieldName.Identifier.ToString());
                    return isPrivateField;
                })
                .ToImmutableList();

            if (assignedArgument != null && methodTokens.Any())
            {
                root = root.ReplaceNode(methodTokens.FirstOrDefault(), assignedArgument);
            }
        });*/
        
        var newDocument = document.WithSyntaxRoot(root); 
        
        return newDocument;
    }
}