using Antlr4.Runtime;

namespace AntlrParser.Analysis;

public static class ParsingSetup
{
    public static void Run(string controllerPath, IServerVisitor<object> visitor)
    {
        var stream = CharStreams.fromString(File.ReadAllText(controllerPath));
        var lexer = new ServerLexer(stream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new ServerParser(tokens);
        visitor.Visit(parser.root());
    }
}