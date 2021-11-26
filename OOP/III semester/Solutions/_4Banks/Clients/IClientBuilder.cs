using System;
using Banks.Clients.Passport;

namespace Banks.Clients
{
    public interface IClientBuilder
    {
        IClientBuilder WithId(Guid id);

        IClientBuilder WithName(string name);

        IClientBuilder WithSurname(string surname);

        IClientBuilder WithAddress(string address);

        IClientBuilder WithPassport(IPassport passport);

        IClient Build();
    }
}