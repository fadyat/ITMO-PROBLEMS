using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace AnalyzerTemplate
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AnalyzerTemplateAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "PrivateFieldAnalyzer";

        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Naming";

        private static readonly DiagnosticDescriptor Rule = new (DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.RegisterSyntaxNodeAction(AnalyzeClass, SyntaxKind.ClassDeclaration);
        }

        private static void AnalyzeClass(SyntaxNodeAnalysisContext c)
        {
            var classDeclaration = c.Node as ClassDeclarationSyntax;
            var fieldsDeclaration = classDeclaration?.DescendantNodes()
                .OfType<FieldDeclarationSyntax>()
                .ToImmutableList();

            var privateFields = fieldsDeclaration?
                .Select(field =>
                {
                    var containsPrivateKeyword = field.Modifiers.Any(modifierType => modifierType.IsKind(SyntaxKind.PrivateKeyword));
                    if (containsPrivateKeyword)
                    {
                        var diagnostics = Diagnostic.Create(Rule, field.GetLocation(), "uwu");
                        c.ReportDiagnostic(diagnostics);
                    }
                    return containsPrivateKeyword ? field : null;
                })
                .ToImmutableList();

            var methodsDeclaration = classDeclaration?.DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .ToImmutableList();

            var methodsWithPrivateFields = methodsDeclaration?
                .Select(method =>
                {
                    var methodStatements = method.Body.Statements;
                    var variablesAssigment =
                        method.DescendantNodes().OfType<AssignmentExpressionSyntax>().ToImmutableList();
                    
                    /*
                    var containsPrivateFields = privateFields?.Any(field =>
                    {
                        var fieldName = field.Declaration.Variables.Select(x => x.Identifier).First();
                        return methodStatements.Any(statement => statement.Contains(fieldName));
                    });*/

                    var containsPrivateFields = !variablesAssigment.IsEmpty;
                    
                    if (containsPrivateFields == true)
                    {
                        var diagnostics = Diagnostic.Create(Rule, method.GetLocation(), "uwu");
                        c.ReportDiagnostic(diagnostics);
                    }

                    return containsPrivateFields == true ? method : null;
                })
                .ToImmutableList();
        }
    }
}
