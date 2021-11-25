using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Accounts;
using Banks.Banks.Limits;
using Banks.Clients;

namespace Banks.Banks
{
    public class Bank : IBank
    {
        public Bank(Guid id, Limit limit)
        {
            Id = id;
            Limit = limit;
            Clients = new HashSet<IClient>();
            Accounts = new Dictionary<Guid, List<Account>>();
        }

        public Guid Id { get; }

        public Limit Limit { get; }

        public Dictionary<Guid, List<Account>> Accounts { get; }

        public HashSet<IClient> Clients { get; }

        public void AddClient(IClient client)
        {
            CentralBank.AddClient(this, client);
        }

        public void RegisterAccount(IClient client, Account account)
        {
            CentralBank.RegisterAccount(this, client, account);
        }

        public IClient GetClient(Guid id)
        {
            return Clients.Single(c => Equals(c.Id, id));
        }

        public void Print()
        {
            Console.WriteLine($"# B: {Id}");
            foreach (IClient c in Clients)
                c.Print();

            foreach ((Guid key, List<Account> value) in Accounts)
            {
                Console.Write("C: " + key + " ");
                foreach (Account aa in value)
                    aa.Print();

                Console.Write("\n");
            }
        }
    }
}