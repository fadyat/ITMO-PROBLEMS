using Antlr4.Runtime;

namespace AntlrCSharp.Analysis;

public class ParsingSetup
{
    private readonly ServerVisitor _visitor;
    private readonly ServerParser _parser;
    
    public ParsingSetup(string controllerPath)
    {
        var stream = CharStreams
            .fromString(File.ReadAllText(controllerPath));
        var lexer = new ServerLexer(stream);
        var tokens = new CommonTokenStream(lexer);
        _parser = new ServerParser(tokens);
        _visitor = new ServerVisitor();
    }

    public void Run()
    {
        _visitor.Visit(_parser.root());
    }
}