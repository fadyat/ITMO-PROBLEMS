using System.Collections.Immutable;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static System.String;

namespace AntlrParser.Analysis;

public class MethodDeclaration
{
    public string? MethodName { get; }

    public string? ReturnType { get; }

    public ImmutableList<ArgumentDeclaration>? Arguments { get; }

    public string? Url { get; }

    public string? HttpMethodName { get; }

    private MethodDeclaration(string? methodDeclaration,
        string? returnType,
        ImmutableList<ArgumentDeclaration>? arguments,
        string? url,
        string? httpMethodName)
    {
        MethodName = methodDeclaration;
        ReturnType = returnType;
        Arguments = arguments;
        Url = url;
        HttpMethodName = httpMethodName;
    }

    public MethodDeclarationBuilder ToBuilder()
    {
        return new MethodDeclarationBuilder()
            .WithMethodName(MethodName)
            .WithReturnType(ReturnType)
            .WithArguments(Arguments)
            .WithUrl(Url)
            .WithHttpMethodName(HttpMethodName);
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented, new StringEnumConverter());
    }

    public class MethodDeclarationBuilder
    {
        private string? _methodDeclaration;
        private string? _returnType;
        private ImmutableList<ArgumentDeclaration>? _arguments;
        private string? _url;
        private string? _httpMethodName;

        public MethodDeclarationBuilder()
        {
            _methodDeclaration = Empty;
            _returnType = Empty;
            _arguments = null;
            _url = Empty;
            _httpMethodName = Empty;
        }

        public MethodDeclarationBuilder WithMethodName(string? methodDeclaration)
        {
            _methodDeclaration = methodDeclaration;
            return this;
        }

        public MethodDeclarationBuilder WithReturnType(string? returnType)
        {
            _returnType = returnType;
            return this;
        }

        public MethodDeclarationBuilder WithArguments(ImmutableList<ArgumentDeclaration>? arguments)
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

    public bool NullOrEmpty()
    {
        return GetType().GetProperties()
                   .Where(f => f.PropertyType == typeof(string))
                   .Select(s => (string) s.GetValue(this)!)
                   .Any(IsNullOrEmpty) && Arguments?.Any() == false;
    }
}