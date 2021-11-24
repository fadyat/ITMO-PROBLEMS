using System;
using Banks.Accounts;

namespace Banks.Clients
{
    public interface IClient
    {
        Guid Id { get; }

        void Print();
    }
}