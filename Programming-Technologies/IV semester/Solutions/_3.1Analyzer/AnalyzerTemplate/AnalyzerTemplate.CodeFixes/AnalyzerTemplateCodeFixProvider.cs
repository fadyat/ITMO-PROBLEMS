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

namespace AnalyzerTemplate
{
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
            var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<IfStatementSyntax>().First();

            context.RegisterCodeFix(
                CodeAction.Create(
                    title: CodeFixResources.CodeFixTitle,
                    createChangedDocument: c => FixIfInversion(context.Document, declaration, c),
                    equivalenceKey: nameof(CodeFixResources.CodeFixTitle)),
                diagnostic);
        }

        private static async Task<Document> FixIfInversion(Document document, IfStatementSyntax ifStatement, CancellationToken cancellationToken)
        {
            var ifBlock = ifStatement.Statement as BlockSyntax;
            var elseBlock = ifStatement.Else.Statement as BlockSyntax;  

            var invertedIfCondition = SyntaxFactory.ParseExpression("!(" + ifStatement.Condition.GetText() + ")");
            var invertedIfStatement = SyntaxFactory.IfStatement(invertedIfCondition, elseBlock);

            var root = await document.GetSyntaxRootAsync(cancellationToken);
            var newRoot = root.InsertNodesAfter(ifStatement, ifBlock?.Statements);
            newRoot = newRoot.ReplaceNode(newRoot.FindNode(ifStatement.Span), invertedIfStatement).NormalizeWhitespace();
            var newDocument = document.WithSyntaxRoot(newRoot);

            return newDocument;
        }
    }
}
