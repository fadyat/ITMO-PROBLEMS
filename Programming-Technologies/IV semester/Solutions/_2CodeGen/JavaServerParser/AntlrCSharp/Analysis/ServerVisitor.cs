using System.Collections.Immutable;

namespace AntlrCSharp.Analysis;

public class ServerVisitor : ServerBaseVisitor<object>
{
    private MethodDeclaration _currentMethodDeclaration;
    private readonly List<MethodDeclaration> _previousMethodDeclarations;

    public ImmutableList<MethodDeclaration> PreviousMethodDeclarations =>
        _previousMethodDeclarations.ToImmutableList();

    private string? _absolutePath;

    public ServerVisitor()
    {
        _currentMethodDeclaration = new MethodDeclaration
                .MethodDeclarationBuilder()
            .Build();
        _absolutePath = string.Empty;
        _previousMethodDeclarations = new List<MethodDeclaration>();
    }

    public override object VisitModel_annotation(ServerParser.Model_annotationContext context)
    {
        var requestHeader = context.annotation()
            .annotation_header()
            .GetText()
            .Equals("@RequestMapping");

        _absolutePath = requestHeader
            ? context.annotation()
                .annotation_arguments()?
                .GetText()
            : null;

        _absolutePath = _absolutePath != null
            ? _absolutePath.Substring(2, _absolutePath.Length - 4)
            : string.Empty;

        return base.VisitModel_annotation(context);
    }

    public override object VisitFunction_annotation(ServerParser.Function_annotationContext context)
    {
        Console.WriteLine(_currentMethodDeclaration);
        return base.VisitFunction_annotation(context);
    }

    public override object VisitReturn_type(ServerParser.Return_typeContext context)
    {
        _currentMethodDeclaration = _currentMethodDeclaration.ToBuilder()
            .WithReturnType(context.GetText())
            .Build();

        Console.WriteLine(_currentMethodDeclaration);
        return base.VisitReturn_type(context);
    }

    public override object VisitFunction_arg(ServerParser.Function_argContext context)
    {
        Console.WriteLine(_currentMethodDeclaration);
        return base.VisitFunction_arg(context);
    }

    public override object VisitFunction_body(ServerParser.Function_bodyContext context)
    {
        if (!_currentMethodDeclaration.NullOrEmpty())
        {
            _previousMethodDeclarations.Add(_currentMethodDeclaration);
        }
        else
        {
            Console.WriteLine("1");
        }
        
        _currentMethodDeclaration = new MethodDeclaration
                .MethodDeclarationBuilder()
            .Build();
        return base.VisitFunction_body(context);
    }
}