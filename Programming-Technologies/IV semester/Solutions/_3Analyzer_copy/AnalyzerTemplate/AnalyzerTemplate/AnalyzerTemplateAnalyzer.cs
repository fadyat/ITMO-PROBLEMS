using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace AnalyzerTemplate;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class AnalyzerTemplateAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "PrivateFieldAnalyzer";
    private const string Category = "Naming";

    private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle),
        Resources.ResourceManager, typeof(Resources));

    private static readonly LocalizableString MessageFormat =
        new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager,
            typeof(Resources));

    private static readonly LocalizableString Description =
        new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager,
            typeof(Resources));

    private static readonly DiagnosticDescriptor Rule = new(DiagnosticId, Title, MessageFormat, Category,
        DiagnosticSeverity.Warning, true, Description);


    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze |
                                               GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.RegisterSyntaxNodeAction(AnalyzeClass, SyntaxKind.ClassDeclaration);
    }

    private static void AnalyzeClass(SyntaxNodeAnalysisContext c)
    {
        var classDeclaration = c.Node as ClassDeclarationSyntax;
        var fieldsDeclaration = classDeclaration?.DescendantNodes().OfType<FieldDeclarationSyntax>().ToImmutableList();

        var privateFields = fieldsDeclaration?.Select(field =>
        {
            var containsPrivateKeyword =
                field.Modifiers.Any(modifierType => modifierType.IsKind(SyntaxKind.PrivateKeyword));
            return containsPrivateKeyword ? field : null;
        }).Where(x => x != null).ToImmutableList();

        var methodsDeclaration =
            classDeclaration?.DescendantNodes().OfType<MethodDeclarationSyntax>().ToImmutableList();

        methodsDeclaration?.ForEach(method =>
        {
            var methodTokens = method.DescendantNodes().OfType<IdentifierNameSyntax>().ToImmutableList();
            var variablesAssigment = method.DescendantNodes().OfType<AssignmentExpressionSyntax>().ToImmutableList();

            privateFields?.ForEach(field =>
            {
                var fieldName = field.Declaration.Variables.FirstOrDefault();

                var firstFieldAssigment = variablesAssigment.Select(assigment =>
                {
                    var theyEquals = Equals(assigment.Left.ToString(), fieldName.Identifier.ToString());
                    return theyEquals ? assigment : null;
                }).FirstOrDefault(x => x != null);

                var firstFieldUsage = methodTokens.Select(token =>
                {
                    var theyEquals = Equals(token.Identifier.ToString(), fieldName.Identifier.ToString());
                    return theyEquals ? token : null;
                }).FirstOrDefault(x => x != null);

                if (firstFieldAssigment?.SpanStart <= firstFieldUsage?.SpanStart)
                {
                    Report(c, firstFieldAssigment.GetLocation());
                    Report(c, firstFieldUsage.GetLocation());
                }
            });
        });
    }

    private static void Report(SyntaxNodeAnalysisContext context, Location itemLocation)
    {
        var diagnostics = Diagnostic.Create(Rule, itemLocation, "uwu");
        context.ReportDiagnostic(diagnostics);
    }
}