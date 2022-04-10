namespace AntlrCSharp.Analysis;

public class ServerVisitor : ServerBaseVisitor<object>
{
    private MethodDeclaration? _currentMethodDeclaration;
    private List<MethodDeclaration> _previousMethodDeclarations;
    private string _absolutePath;

    public ServerVisitor()
    {
        _currentMethodDeclaration = null;
        _absolutePath = string.Empty;
        _previousMethodDeclarations = new List<MethodDeclaration>();
    }

    public override object VisitModel_annotation(ServerParser.Model_annotationContext context)
    {
        Console.WriteLine("model annotation " + context.GetText());
        return base.VisitModel_annotation(context);
    }

    public override object VisitFunction_annotation(ServerParser.Function_annotationContext context)
    {
        Console.WriteLine("function annotation " + context.GetText());
        return base.VisitFunction_annotation(context);
    }

    public override object VisitReturn_type(ServerParser.Return_typeContext context)
    {
        Console.WriteLine("function return type " + context.GetText());
        return base.VisitReturn_type(context);
    }

    public override object VisitFunction_arg(ServerParser.Function_argContext context)
    {
        Console.WriteLine("function arg " +
                          context.var_type().GetText() + " " +
                          context.var().GetText());
        return base.VisitFunction_arg(context);
    }

    public override object VisitFunction_body(ServerParser.Function_bodyContext context)
    {
        Console.WriteLine();
        return base.VisitFunction_body(context);
    }
}