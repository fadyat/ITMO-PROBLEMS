namespace Roslyn;

public class Program
{
    // if in all constructions 'case1 = case1Value' before usage of 'case1'
    //
    //                               or
    //
    // if in all constructions 'case1Value', before 'case1 = case1Value'
    //
    //                               do
    //
    //  remove('case1 = case1Value') => remove(field 'case1') => replace('case1', 'case1Value') 
    // 

    private int _case1;

    public void SetCase1(int case1Value)
    {
        _case1 = case1Value;
    }

    public static void Main()
    {
        
    }
}