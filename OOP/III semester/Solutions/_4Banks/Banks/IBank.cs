using System;
using Banks.Accounts;
using Banks.Banks.Limits;
using Banks.Clients;

namespace Banks.Banks
{
    public interface IBank
    {
        ILimit Limit { get; }

        Guid Id { get; }

        void AddClient(IClient client);

        void RegisterAccount(IClient client, IAccount account);

        IClient GetClient(Guid id);

        void Print(); // remove
    }
}