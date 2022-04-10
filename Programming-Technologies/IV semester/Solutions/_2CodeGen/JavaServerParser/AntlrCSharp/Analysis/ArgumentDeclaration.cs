namespace AntlrCSharp.Analysis;

public class ArgumentDeclaration
{
    private string _argumentType;
    private string _argumentName;

    public ArgumentDeclaration(string argumentType, string argumentName)
    {
        _argumentType = argumentType;
        _argumentName = argumentName;
    }
}