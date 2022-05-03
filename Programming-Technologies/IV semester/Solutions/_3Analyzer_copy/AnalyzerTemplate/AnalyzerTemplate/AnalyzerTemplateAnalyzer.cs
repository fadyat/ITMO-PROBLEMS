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


        // now I ignore situations where in method can be assigned many variables
        privateFields?.ForEach(field =>
        {
            var fieldName = field.Declaration.Variables.FirstOrDefault();
            var canBeLocal = true;

            methodsDeclaration?.ForEach(method =>
            {
                var methodTokens = method.DescendantNodes().OfType<IdentifierNameSyntax>().ToImmutableList();
                var variablesAssignment =
                    method.DescendantNodes().OfType<AssignmentExpressionSyntax>().ToImmutableList();
                var methodArgs = method.ParameterList.Parameters.ToImmutableList();

                var firstFieldAssigment = variablesAssignment.Where(assigment =>
                {
                    var leftAssignment = assigment.Left.ToString();
                    var potentialLeftAssignment = fieldName.Identifier.ToString();
                    var leftEquals = Equals(leftAssignment, potentialLeftAssignment);

                    var rights = assigment.Right.DescendantNodesAndSelf().OfType<IdentifierNameSyntax>().ToImmutableList();
                    var rightEquals = false;
                    methodArgs.ForEach(arg =>
                    {
                        var potentialRightAssignment = arg.Identifier.ToString();
                        rights.ForEach(x =>
                        {
                            var rightAssignment = x.Identifier.ToString();
                            rightEquals |= Equals(rightAssignment, potentialRightAssignment);
                        });
                    });

                    return leftEquals & rightEquals;
                }).FirstOrDefault();

                var firstFieldUsage = methodTokens.Where(token =>
                {
                    var containsField = Equals(token.Identifier.ToString(), fieldName.Identifier.ToString());
                    return containsField;
                }).FirstOrDefault();

                canBeLocal &= (firstFieldAssigment?.SpanStart <= firstFieldUsage?.SpanStart) |
                              (Equals(firstFieldAssigment, null) & Equals(firstFieldUsage, null));
            });

            if (canBeLocal) Report(c, field.GetLocation());
        });
    }

    private static void Report(SyntaxNodeAnalysisContext context, Location itemLocation)
    {
        var diagnostics = Diagnostic.Create(Rule, itemLocation, "uwu");
        context.ReportDiagnostic(diagnostics);
    }
}