using System;
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
            var pr = new SimpleBankLimit(
                1,
                null,
                (-10000, 100000),
                1,
                5);

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

            DateTime now = DateTime.Now;
            centralBank.RegisterAccount(bank, lol, new CreditAccount(10, now));
            centralBank.RegisterAccount(bank, lol1, new DepositAccount(10, now));
            centralBank.RegisterAccount(bank, lol, new DebitAccount(10, now));
            centralBank.Print();
        }
    }
}