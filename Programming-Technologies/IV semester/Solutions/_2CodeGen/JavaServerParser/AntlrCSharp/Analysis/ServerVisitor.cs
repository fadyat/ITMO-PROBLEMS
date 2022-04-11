using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace AntlrCSharp.Analysis;

public class ServerVisitor : ServerBaseVisitor<object>
{
    private static readonly List<string> HttpMethods = new()
        {"GET", "POST", "PUT", "HEAD", "DELETE", "PATCH", "OPTIONS", "CONNECT", "TRACE"};

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
        var requestHeader = context
            .annotation()
            .annotation_header()
            .GetText()
            .Equals("@RequestMapping");

        _absolutePath = requestHeader
            ? context
                .annotation()
                .annotation_arguments()?
                .GetText()
            : null;

        _absolutePath = _absolutePath != null
            ? _absolutePath.Substring(2, _absolutePath.Length - 4)
            : string.Empty;

        _absolutePath = (!_absolutePath.StartsWith("/") ? "/" : string.Empty) + _absolutePath;

        return base.VisitModel_annotation(context);
    }

    public override object VisitFunction_name(ServerParser.Function_nameContext context)
    {
        _currentMethodDeclaration = _currentMethodDeclaration
            .ToBuilder()
            .WithMethodName(context.GetText())
            .Build();

        return base.VisitFunction_name(context);
    }

    public override object VisitFunction_annotation(ServerParser.Function_annotationContext context)
    {
        var httpMethodName = HttpMethods
            .FirstOrDefault(httpMethod => context
                .GetText()
                .ToUpper()
                .Contains(httpMethod));

        var regex = new Regex("\"(.*?)\"");
        var match = regex.Match(context.GetText())
            .Value;
        var url = match != string.Empty
            ? match.Substring(1, match.Length - 2)
            : string.Empty;

        var fullUrl = _absolutePath +
                      (!_absolutePath!.EndsWith("/") && !url.StartsWith("/") ? "/" : string.Empty) +
                      url;
        _currentMethodDeclaration = _currentMethodDeclaration
            .ToBuilder()
            .WithHttpMethodName(httpMethodName)
            .WithUrl(fullUrl)
            .Build();

        return base.VisitFunction_annotation(context);
    }

    public override object VisitReturn_type(ServerParser.Return_typeContext context)
    {
        _currentMethodDeclaration = _currentMethodDeclaration
            .ToBuilder()
            .WithReturnType(context.GetText())
            .Build();

        return base.VisitReturn_type(context);
    }

    public override object VisitFunction_args(ServerParser.Function_argsContext context)
    {
        var args = context
            .function_arg()
            .Select(arg => new ArgumentDeclaration(
                arg.var().GetText(),
                arg.var_type().GetText()))
            .ToList();

        _currentMethodDeclaration = _currentMethodDeclaration
            .ToBuilder()
            .WithArguments(args)
            .Build();

        return base.VisitFunction_args(context);
    }

    public override object VisitFunction_body(ServerParser.Function_bodyContext context)
    {
        if (!_currentMethodDeclaration.NullOrEmpty())
        {
            _previousMethodDeclarations.Add(_currentMethodDeclaration);
        }

        _currentMethodDeclaration = new MethodDeclaration
                .MethodDeclarationBuilder()
            .Build();

        return base.VisitFunction_body(context);
    }
}