using System;

namespace Banks.Clients
{
    public interface IClient
    {
        string Name { get; }

        string Surname { get; }

        Guid Id { get; }

        string Address { get; }

        string Passport { get; }

        IClientBuilder ToBuilder();

        void Print();
    }
}