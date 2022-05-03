namespace Roslyn;

public class Program
{
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

    private int _case1;
    private string _case2 = "123";
    
    public void SetCase1(int case1Value)
    {
        _case1 = case1Value;
        Console.WriteLine(_case1);
    }

    public void SetCase2(int case2Value)
    {
        _case2 = Convert.ToString(case2Value);
    }

    public static void Main()
    {
    }
}