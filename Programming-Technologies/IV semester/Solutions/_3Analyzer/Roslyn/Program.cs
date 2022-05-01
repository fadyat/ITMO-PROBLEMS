namespace Roslyn;

public class Program
{
    public static void Main()
    {
        var statement = "if_statement_example";
        if (statement is string)
        {
            Console.WriteLine(statement, "it is a string!");
        }
        else
        {
            throw new Exception(statement);
        }
    }
}