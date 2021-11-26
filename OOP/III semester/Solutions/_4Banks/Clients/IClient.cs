using System;
using Banks.Clients.Passport;

namespace Banks.Clients
{
    public interface IClient
    {
        string Name { get; }

        string Surname { get; }

        Guid Id { get; }

        string Address { get; }

        IPassport Passport { get; }

        IClientBuilder ToBuilder();

        void Print();
    }
}