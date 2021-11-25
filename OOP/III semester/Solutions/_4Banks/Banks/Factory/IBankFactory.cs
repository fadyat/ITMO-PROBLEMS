using Banks.Banks.Limits;

namespace Banks.Banks.Factory
{
    public interface IBankFactory
    {
        IBank CreateBank(Limit limit);
    }
}