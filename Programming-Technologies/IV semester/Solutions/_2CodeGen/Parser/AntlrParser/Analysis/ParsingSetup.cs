using System.Collections.Immutable;
using Antlr4.Runtime;

namespace AntlrParser.Analysis;

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

    public ImmutableList<MethodDeclaration> Run()
    {
        _visitor.Visit(_parser.root());
        return _visitor.PreviousMethodDeclarations;
    }
}