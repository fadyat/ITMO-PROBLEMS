using System;
using System.Collections.Generic;
using Banks.Accounts;
using Banks.Clients.Passport;

namespace Banks.Clients
{
    public class Client : IClient
    {
        private readonly string _name;

        public Client(string name)
        {
            _name = name;
            Id = Guid.NewGuid();
        }

        // private string _secondName;
        // private Guid _id;
        // private List<Account> _accounts;
        // private string _address; // could be class but I don't want
        // private IPassport _passport;
        // public class ClientBuilder
        // {
            // private string _name;
            // private string _secondName;
            // private Guid _id;
            // private List<Account> _accounts;
            // private string _address; // could be class but I don't want
            // private IPassport _passport;
        // }
        public Guid Id { get; }

        public void Print()
        {
            Console.WriteLine($"\t C: {_name}");
        }
    }
}