namespace Banks.Banks.Factory
{
    public class SimpleBankFactory : IBankFactory
    {
        public IBank CreateBank(string name)
        {
            return new Bank();
        }
    }
}