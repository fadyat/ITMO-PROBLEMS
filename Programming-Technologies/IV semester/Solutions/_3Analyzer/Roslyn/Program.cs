namespace Roslyn;

public static class Program
{
    public static void Main()
    {
        const string statement = "if_statement_example";
        if (statement.Contains("if"))
        {
            Console.WriteLine(statement + " it is a string!");
            Console.WriteLine("i'm idiot!");
        }
        else
        {
            throw new Exception(statement);
        }

        const string idiot = "me";
        Console.WriteLine(idiot);
    }
}