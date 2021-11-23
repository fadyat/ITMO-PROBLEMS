namespace Banks.Banks.Factory
{
    public interface IBankFactory
    {
        IBank CreateBank(string name);
    }
}