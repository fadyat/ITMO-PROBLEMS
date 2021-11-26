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
            var bankFactory = new SimpleBankFactory();
            var limits = new SimpleBankLimit(
                1,
                null,
                (-100, 100),
                10,
                100);

            IBank bank1 = bankFactory.CreateBank(limits);
            var me = new Client("Fadeyev", "Artyom");
            bank1.AddClient(me);
            var debitAccount = new DebitAccount(1000, DateTime.Now);
            bank1.RegisterAccount(me, debitAccount);
        }
    }
}