using System;
using System.Reflection.Metadata;
using Banks.Accounts;
using Banks.Banks;
using Banks.Banks.Chain;
using Banks.Banks.Factory;
using Banks.Banks.Limits;
using Banks.Clients;
using Banks.Clients.Passport;

namespace Banks
{
    public static class Program
    {
        public static void Main()
        {
            // var centralBank = new CentralBank();
            // var bankFactory = new SimpleBankFactory();
            var pr = new SimpleBankLimit(
                100 * 365,
                null,
                (-10000, 100000),
                100,
                0);
/*
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

            // DateTime now = DateTime.Now;
            centralBank.RegisterAccount(bank, lol, new CreditAccount(10, now));
            centralBank.RegisterAccount(bank, lol1, new DepositAccount(10, now));
            centralBank.RegisterAccount(bank, lol, new DebitAccount(10, now));
            centralBank.Print(); */
            DateTime now = DateTime.Now;
            var tmp = new DebitAccount(10, now);

            // Console.WriteLine(now.AddDays(24));
            Console.WriteLine(now);
            Console.WriteLine(tmp.Calculate(pr, now.AddDays(61)).Balance);
            var a = new Client("1", "2", passport: new PassportRu(1111, 111111));
            var b = new Client("1", "2", "b", new PassportRu(1111, 111111));
            var c = new Client("1", "2", "c", new PassportRu(1234, 567890));

            var h3 = new ClientHandler(c);
            var h2 = new ClientHandler(b, h3);
            var h1 = new ClientHandler(a, h2);
            var cl = new Handler();

            cl.SetHandler(h1);
            bool ans = cl.HandlerRequest();
            Console.WriteLine(ans);
            /*
             * Chain Of Responsibility for accounts
             */
        }
    }
}