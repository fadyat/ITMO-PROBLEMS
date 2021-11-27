using System;

namespace Banks.Clients
{
    public class Client : IClient
    {
        private Client(
            string surname,
            string name,
            Guid id,
            string address = null,
            string passport = null)
        {
            Surname = surname;
            Name = name;
            Address = address;
            Passport = passport;
            Id = id;
        }

        public string Name { get; }

        public string Surname { get; }

        public Guid Id { get; }

        public string Address { get; }

        public string Passport { get; }

        public IClientBuilder ToBuilder()
        {
            IClientBuilder builder = new ClientBuilder()
                .WithSurname(Surname)
                .WithName(Name)
                .WithId(Id)
                .WithPassport(Passport)
                .WithAddress(Address);

            return builder;
        }

        public void Print()
        {
            Console.WriteLine($"\t C: {Surname}");
        }

        public class ClientBuilder : IClientBuilder
        {
            private Guid _id;
            private string _name;
            private string _surname;
            private string _address;
            private string _passport;

            public IClientBuilder WithId(Guid id)
            {
                _id = id;
                return this;
            }

            public IClientBuilder WithName(string name)
            {
                _name = name;
                return this;
            }

            public IClientBuilder WithSurname(string surname)
            {
                _surname = surname;
                return this;
            }

            public IClientBuilder WithAddress(string address)
            {
                _address = address;
                return this;
            }

            public IClientBuilder WithPassport(string passport)
            {
                _passport = passport;
                return this;
            }

            public IClient Build()
            {
                var finallyClient = new Client(_surname, _name, _id, _address, _passport);
                return finallyClient;
            }
        }
    }
}