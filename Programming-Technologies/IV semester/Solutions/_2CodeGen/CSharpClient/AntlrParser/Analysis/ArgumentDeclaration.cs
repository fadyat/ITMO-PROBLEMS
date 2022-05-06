namespace AntlrParser.Analysis;

public class ArgumentDeclaration
{
    public string ArgumentType { get; }

    public string ArgumentName { get; }

    public ArgumentDeclaration(string argumentType, string argumentName)
    {
        ArgumentType = argumentType;
        ArgumentName = argumentName;
    }
}