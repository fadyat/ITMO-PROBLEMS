using System;

namespace Banks.Clients
{
    public interface IClientBuilder
    {
        IClientBuilder WithId(Guid id);

        IClientBuilder WithName(string name);

        IClientBuilder WithSurname(string surname);

        IClientBuilder WithAddress(string address);

        IClientBuilder WithPassport(string passport);

        IClient Build();
    }
}