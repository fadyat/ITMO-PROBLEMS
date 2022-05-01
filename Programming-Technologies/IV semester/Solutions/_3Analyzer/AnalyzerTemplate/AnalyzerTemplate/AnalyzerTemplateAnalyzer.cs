﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace AnalyzerTemplate
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AnalyzerTemplateAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "AnalyzerTemplate";

        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Naming";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.RegisterSyntaxNodeAction(AnalyzeIfInversion, SyntaxKind.IfStatement);
        }

        private void AnalyzeIfInversion(SyntaxNodeAnalysisContext c)
        {
            var ifStatement = c.Node as IfStatementSyntax;
            var elseStatement = ifStatement.Else;

            if (elseStatement == null) return;

            var elseBlock = elseStatement.Statement as BlockSyntax;

            if (elseBlock == null) return;

            var blockStatements = elseBlock.Statements;

            foreach (var blockStatement in blockStatements)
            {
                if (blockStatement.IsKind(SyntaxKind.ReturnStatement) || blockStatement.IsKind(SyntaxKind.ThrowStatement))
                {
                    var diagnostics = Diagnostic.Create(Rule, ifStatement.GetLocation(), "Can invert if statement");
                    c.ReportDiagnostic(diagnostics);
                }
            }
        }
    }
}
