using AntlrParser.Analysis;

namespace AntlrParser.Visitors;

public class ServerModelVisitor : ServerBaseVisitor<object>
{
    public ModelDeclaration ModelDeclaration { get; private set; }

    public ServerModelVisitor()
    {
        ModelDeclaration = new ModelDeclaration.ModelDeclarationBuilder()
            .Build();
    }

    public override object VisitModel_header(ServerParser.Model_headerContext context)
    {
        ModelDeclaration = ModelDeclaration
            .ToBuilder()
            .WithModelName(context.model_name().GetText())
            .Build();

        return base.VisitModel_header(context);
    }

    public override object VisitModel_attribute(ServerParser.Model_attributeContext context)
    {
        var arguments = ModelDeclaration.Arguments?.Add(
            new ArgumentDeclaration(
                context.var_type().GetText(),
                context.var().GetText()
            )
        );

        ModelDeclaration = ModelDeclaration
            .ToBuilder()
            .WithArguments(arguments!.ToList())
            .Build();

        return base.VisitModel_attribute(context);
    }
}