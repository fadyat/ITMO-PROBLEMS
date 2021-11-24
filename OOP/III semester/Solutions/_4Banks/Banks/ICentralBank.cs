using System;
using Banks.Accounts;
using Banks.Clients;

namespace Banks.Banks
{
    public interface ICentralBank
    {
        void AddBank(IBank bank);

        void AddClient(IBank bank, IClient client);

        IBank GetBank(Guid id);

        void RegisterAccount(IBank bank, IClient client, Account account);

        void Print(); // remove
    }
}