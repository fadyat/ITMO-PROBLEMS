using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Accounts;
using Banks.Clients;

namespace Banks.Banks
{
    public static class CentralBank
    {
        private static readonly List<IBank> Banks;

        static CentralBank()
        {
            Banks = new List<IBank>();
        }

        public static void AddBank(IBank bank)
        {
            Banks.Add(bank);
        }

        public static void AddClient(IBank bank, IClient client)
        {
            bank = GetBank(bank.Id);
            bank.Clients.Add(client);
        }

        public static IBank GetBank(Guid id)
        {
            return Banks.Single(b => Equals(b.Id, id));
        }

        public static void RegisterAccount(IBank bank, IClient client, Account account)
        {
            bank = GetBank(bank.Id);
            client = bank.GetClient(client.Id);
            if (!bank.Accounts.ContainsKey(client.Id))
            {
                bank.Accounts[client.Id] = new List<Account>();
            }

            bank.Accounts[client.Id].Add(account);
        }

        public static void Print()
        {
            foreach (IBank b in Banks)
                b.Print();
        }
    }
}