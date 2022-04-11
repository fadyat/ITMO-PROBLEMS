using Antlr4.Runtime;

namespace AntlrParser.Analysis;

public class ParsingSetup
{
    private readonly IServerVisitor<object> _visitor;
    private readonly ServerParser _parser;

    public ParsingSetup(string controllerPath, IServerVisitor<object> visitor)
    {
        var stream = CharStreams
            .fromString(File.ReadAllText(controllerPath));
        var lexer = new ServerLexer(stream);
        var tokens = new CommonTokenStream(lexer);
        _parser = new ServerParser(tokens);
        _visitor = visitor;
    }

    public ParsingSetup Run()
    {
        _visitor?.Visit(_parser.root());
        return this;
    }
}