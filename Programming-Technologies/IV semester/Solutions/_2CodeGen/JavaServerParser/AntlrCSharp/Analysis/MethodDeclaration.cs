namespace AntlrCSharp.Analysis;

public class MethodDeclaration
{
    private readonly string? _methodDeclaration;
    private readonly string? _returnType;
    private readonly List<ArgumentDeclaration>? _arguments;
    private readonly string? _url;
    private readonly string? _httpMethodName;

    private MethodDeclaration(string? methodDeclaration,
        string? returnType,
        List<ArgumentDeclaration>? arguments,
        string? url,
        string? httpMethodName)
    {
        _methodDeclaration = methodDeclaration;
        _returnType = returnType;
        _arguments = arguments;
        _url = url;
        _httpMethodName = httpMethodName;
    }

    public MethodDeclarationBuilder ToBuilder()
    {
        return new MethodDeclarationBuilder()
            .WithMethodDeclaration(_methodDeclaration)
            .WithReturnType(_returnType)
            .WithArguments(_arguments)
            .WithUrl(_url)
            .WithHttpMethodName(_httpMethodName);
    }

    public class MethodDeclarationBuilder
    {
        private string? _methodDeclaration;
        private string? _returnType;
        private List<ArgumentDeclaration>? _arguments;
        private string? _url;
        private string? _httpMethodName;

        public MethodDeclarationBuilder WithMethodDeclaration(string? methodDeclaration)
        {
            _methodDeclaration = methodDeclaration;
            return this;
        }

        public MethodDeclarationBuilder WithReturnType(string? returnType)
        {
            _returnType = returnType;
            return this;
        }

        public MethodDeclarationBuilder WithArguments(List<ArgumentDeclaration>? arguments)
        {
            _arguments = arguments;
            return this;
        }

        public MethodDeclarationBuilder WithUrl(string? url)
        {
            _url = url;
            return this;
        }

        public MethodDeclarationBuilder WithHttpMethodName(string? httpMethodName)
        {
            _httpMethodName = httpMethodName;
            return this;
        }

        public MethodDeclaration Build()
        {
            return new MethodDeclaration(_methodDeclaration,
                _returnType,
                _arguments,
                _url,
                _httpMethodName);
        }
    }
}