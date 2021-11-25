using System;
using Banks.Accounts;
using Banks.Clients.Passport;

namespace Banks.Clients
{
    public interface IClient
    {
        Guid Id { get; }

        string Address { get; }

        IPassport Passport { get; }

        void Print();
    }
}