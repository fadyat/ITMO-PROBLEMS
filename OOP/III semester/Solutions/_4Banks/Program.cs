using System;
using Banks.Banks;
using Banks.Banks.Factory;
using Banks.Clients.Passport;

namespace Banks
{
    public static class Program
    {
        public static void Main()
        {
            var centralBank = new CentralBank();
            var bankFactory = new SimpleBankFactory();
            IBank bank = bankFactory.CreateBank("123");
            centralBank.AddBank(bank);
        }
    }
}