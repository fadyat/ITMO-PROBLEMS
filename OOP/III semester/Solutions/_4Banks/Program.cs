using Banks.Accounts;
using Banks.Banks;
using Banks.Banks.Factory;
using Banks.Banks.Limits;
using Banks.Clients;

namespace Banks
{
    public static class Program
    {
        public static void Main()
        {
            var centralBank = new CentralBank();
            var bankFactory = new SimpleBankFactory();
            var pr = new SimpleBankLimit(1, null, 1, 1);

            IBank bank = bankFactory.CreateBank(pr);
            IBank bank1 = bankFactory.CreateBank(pr);
            centralBank.AddBank(bank);
            centralBank.AddBank(bank1);
            centralBank.Print();

            var lol = new Client("lol");
            var lol1 = new Client("lol1");
            centralBank.AddClient(bank, lol);
            centralBank.AddClient(bank, lol1);
            centralBank.AddClient(bank1, lol);
            centralBank.Print();

            centralBank.RegisterAccount(bank, lol, new CreditAccount());
            centralBank.RegisterAccount(bank, lol1, new DepositAccount());
            centralBank.RegisterAccount(bank, lol, new DebitAccount());
            centralBank.Print();
        }
    }
}