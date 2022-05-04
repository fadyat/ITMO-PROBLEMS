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

    private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));

    private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));

    private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));

    private static readonly DiagnosticDescriptor Rule = new(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, true, Description);

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
        var fieldsDeclaration = classDeclaration?.DescendantNodes().OfType<FieldDeclarationSyntax>();

        var privateFields = fieldsDeclaration?.Where(field =>
        {
            var havePrivateKeyword = field.Modifiers.Any(modifierType => modifierType.IsKind(SyntaxKind.PrivateKeyword));
            return havePrivateKeyword;
        }).ToImmutableList();

        var methodsDeclaration = classDeclaration?.DescendantNodes().OfType<MethodDeclarationSyntax>().ToImmutableList();

        // if method have multiple assignment for private field -- works incorrect
        privateFields?.ForEach(field =>
        {
            var fieldName = field.Declaration.Variables.FirstOrDefault();
            var fieldUsedInRightAssignment = classDeclaration.DescendantNodesAndSelf().OfType<AssignmentExpressionSyntax>()
                .Any(x =>
                {
                    var rights = x.Right.DescendantNodesAndSelf().OfType<IdentifierNameSyntax>();
                    return rights.Any(y => Equals(y.Identifier.ToString(), fieldName.Identifier.ToString()));
                });
            
            if (fieldUsedInRightAssignment) return;
            
            var wasIncorrectMethods = false;
            var used = false;
            
            methodsDeclaration?.ForEach(method =>
            {
                var firstFieldAssignment = FindAssignedParameter(method, fieldName);

                var firstFieldUsage = method.DescendantNodes().OfType<IdentifierNameSyntax>().FirstOrDefault(x =>
                    Equals(x.Identifier.ToString(), fieldName.Identifier.ToString()));

                used |= !Equals(firstFieldUsage, null);
                wasIncorrectMethods |= (Equals(firstFieldAssignment, null) ^ Equals(firstFieldUsage, null)) |
                                       (firstFieldUsage?.SpanStart < firstFieldAssignment?.SpanStart);
            });

            if (wasIncorrectMethods | !used) return;
            
            var diagnostics = Diagnostic.Create(Rule, field.GetLocation(), "private field can be local");
            c.ReportDiagnostic(diagnostics);
        });
    }

    private static AssignmentExpressionSyntax FindAssignedParameter(BaseMethodDeclarationSyntax method, VariableDeclaratorSyntax fieldName)
    {
        var variablesAssignment = method.DescendantNodes().OfType<AssignmentExpressionSyntax>();
        var methodArgs = method.ParameterList.Parameters.Where(arg => variablesAssignment.All(assignment => 
            !Equals(assignment.Left.ToString(), arg.Identifier.ToString())));
        
        // if privateField type are different with return type -- works incorrect
        return variablesAssignment.FirstOrDefault(assignment =>
        {
            var leftAssignment = assignment.Left.ToString();
            var potentialLeftAssignment = fieldName.Identifier.ToString();
            var leftEquals = Equals(leftAssignment, potentialLeftAssignment);
            var rights = assignment.Right.DescendantNodesAndSelf().OfType<IdentifierNameSyntax>().ToImmutableList();

            return methodArgs.Any(arg => rights.Any(x => 
                    Equals(x.Identifier.ToString(), arg.Identifier.ToString()))
            ) & leftEquals;
        });
    }
}