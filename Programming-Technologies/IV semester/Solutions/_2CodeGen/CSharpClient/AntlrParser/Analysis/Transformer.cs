using System.Globalization;

namespace AntlrParser.Analysis;

public static class Transformer
{
    private static readonly List<string> HttpMethods = new()
        {"get", "post", "put", "head", "delete", "patch", "options", "connect", "trace"};

    private static readonly Dictionary<string, string> PrimitiveTypesJavaToCSharp = new()
    {
        {"Integer", "int"}, {"String", "string"}
    };

    private static readonly TextInfo TextInfo = new CultureInfo("en-US", false).TextInfo;

    public static string MakeHttpMethodTitle(string context)
    {
        var httpMethodName = HttpMethods.FirstOrDefault(httpMethod =>
            context.ToLower().Contains(httpMethod)) ?? "";

        return TextInfo.ToTitleCase(httpMethodName);
    }

    public static string MakeCorrectTypes(string type)
    {
        foreach (var x in PrimitiveTypesJavaToCSharp.Where(x => type.Contains(x.Key)))
        {
            type = type.Replace(x.Key, x.Value);
        }

        return type;
    }

    public static string? ReturnTypeWithNull(string? returnType)
    {
        if (returnType != null && returnType.Last() != '?' && returnType != "void")
        {
            returnType = string.Concat(returnType, "?");
        }

        return returnType;
    }
}