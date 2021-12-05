using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using Banks.Accounts;
using Banks.Banks.Limits;
using Banks.Clients;

namespace Banks.Banks
{
    public interface IBank
    {
        string Name { get; }

        Limit Limit { get; }

        Guid Id { get; }

        ImmutableDictionary<Guid, ReadOnlyCollection<Account>> Accounts { get; }

        IEnumerable<IClient> Clients { get; }

        void AddClient(IClient client);

        void RegisterAccount(IClient client, Account account);

        IClient GetClient(Guid id);

        Account GetAccount(Guid clientId, Guid accountId);

        bool ContainsClient(Guid id);

        void TopUp(IClient client, Account account, double amount);

        void WithDraw(IClient client, Account account, double amount);

        void Transfer(IClient client, Account from, Account too, double amount);

        void UpdateLimit(Limit limit);

        Account Calculate(IClient client, Account account, DateTime inDate);
    }
}