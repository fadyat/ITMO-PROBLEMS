using System;
using System.Collections.Generic;
using Banks.Accounts;
using Banks.Banks.Limits;
using Banks.Clients;

namespace Banks.Banks
{
    public interface IBank
    {
        Limit Limit { get; }

        Guid Id { get; }

        public Dictionary<Guid, List<Account>> Accounts { get; }

        public HashSet<IClient> Clients { get; }

        void AddClient(IClient client);

        void RegisterAccount(IClient client, Account account);

        IClient GetClient(Guid id);

        void Print(); // remove
    }
}