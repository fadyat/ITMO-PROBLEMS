namespace Roslyn;

// Analyzer:
// 
// - if in all methods: 'case1 = case1Value' before usage of 'case1'
//                      'case1Value' is a parameter
//
// * find first expression 'case1 = case1Value' and first usage of 'case1'
//
//
// Codefixer:
//
// - remove('case1 = case1Value') => remove(field 'case1') => replace('case1', 'case1Value') 
// 

public class Program
{
    private int _privateField1;
    private int _privateField2;
    private int _privateField3 = 322;
    private int _privateField4 = 228;
    
    public void Method1(int value)
    {
        _privateField1 = value;
        Console.WriteLine("Value: " + _privateField1);
    }

    public void Method2(int value)
    {
        _privateField1 = value % 10;
        Console.WriteLine("Mod: " + _privateField1);
        _privateField2 = value / 10;
        Console.WriteLine("Div: " + _privateField2);
    }

    public void Method3(int value)
    {
        Console.WriteLine("Incorrect, used previous field value: " + _privateField3);
        _privateField3 = value;
    }

    public void Method4(int value)
    {
        value *= 2;
        _privateField4 = value; 
        Console.WriteLine("Incorrect, parameter is changing: " + _privateField4);
    }

    public static void Main()
    {
    }
}