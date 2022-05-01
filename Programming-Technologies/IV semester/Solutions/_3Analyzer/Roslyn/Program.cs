namespace ProjectForAnalyzer;

public class Program
{
    public static void Main()
    {
        var statement = "if_statement_example";
        if (statement is string && statement.Contains("if"))
        {
            Console.WriteLine(statement, "it is a string!");
            Console.WriteLine("i'm idiot!");
        }
        else
        {
            throw new Exception(statement);
        }
 
        var idiot = "me";
        Console.WriteLine(idiot);
    }
}