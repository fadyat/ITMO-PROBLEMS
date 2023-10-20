using System.Collections.Immutable;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AntlrParser.Analysis;

public class ModelDeclaration
{
    private readonly List<ArgumentDeclaration> _arguments;
    public string ModelName { get; }

    public ImmutableList<ArgumentDeclaration> Arguments => _arguments.ToImmutableList();

    private ModelDeclaration(List<ArgumentDeclaration> arguments, string modelName)
    {
        _arguments = arguments;
        ModelName = modelName;
    }

    public ModelDeclarationBuilder ToBuilder()
    {
        return new ModelDeclarationBuilder()
            .WithArguments(_arguments)
            .WithModelName(ModelName);
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(
            this,
            Formatting.Indented,
            new StringEnumConverter());
    }

    public class ModelDeclarationBuilder
    {
        private List<ArgumentDeclaration> _arguments;
        private string _modelName;

        public ModelDeclarationBuilder()
        {
            _arguments = new List<ArgumentDeclaration>();
            _modelName = string.Empty;
        }

        public ModelDeclarationBuilder WithModelName(string modelName)
        {
            _modelName = modelName;
            return this;
        }

        public ModelDeclarationBuilder WithArguments(List<ArgumentDeclaration> arguments)
        {
            _arguments = arguments;
            return this;
        }

        public ModelDeclaration Build()
        {
            return new ModelDeclaration(_arguments, _modelName);
        }
    }
}