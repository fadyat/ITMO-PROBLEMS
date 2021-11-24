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
        private readonly HashSet<IClient> _clients;
        private readonly Dictionary<Guid, List<IAccount>> _accounts;

        public Bank(Guid id, ILimit limit)
        {
            Id = id;
            Limit = limit;
            _clients = new HashSet<IClient>();
            _accounts = new Dictionary<Guid, List<IAccount>>();
        }

        public Guid Id { get; }

        public ILimit Limit { get; }

        public void AddClient(IClient client)
        {
            _clients.Add(client);
        }

        public void RegisterAccount(IClient client, IAccount account)
        {
            client = GetClient(client.Id);
            if (!_accounts.ContainsKey(client.Id))
            {
                _accounts[client.Id] = new List<IAccount>();
            }

            _accounts[client.Id].Add(account);
        }

        public IClient GetClient(Guid id)
        {
            return _clients.Single(c => Equals(c.Id, id));
        }

        public void Print()
        {
            Console.WriteLine($"# B: {Id}");
            foreach (IClient c in _clients)
                c.Print();

            foreach ((Guid key, List<IAccount> value) in _accounts)
            {
                Console.Write("C: " + key + " ");
                foreach (IAccount aa in value)
                    aa.Print();

                Console.Write("\n");
            }
        }
    }
}