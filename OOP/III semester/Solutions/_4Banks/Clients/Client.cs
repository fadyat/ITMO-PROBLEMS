using System;
using System.Collections.Generic;
using Banks.Accounts;
using Banks.Clients.Passport;

namespace Banks.Clients
{
    public class Client : IClient
    {
        private readonly string _name;
        private string _secondName;

        public Client(
            string secondName,
            string name,
            string address = null,
            IPassport passport = null)
        {
            _secondName = secondName;
            _name = name;
            Address = address;
            Passport = passport;
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }

        public string Address { get; }

        public IPassport Passport { get; }

        public void Print()
        {
            Console.WriteLine($"\t C: {_name}");
        }

        // ... builder
    }
}