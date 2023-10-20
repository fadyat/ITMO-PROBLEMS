using System;

namespace Banks.Clients
{
    public class Client : IClient
    {
        public Client(string surname, string name, Guid id, string address = null, string passport = null)
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

        public string Address { get; private set; }

        public string Passport { get; private set; }

        public Client WithAddress(string address)
        {
            Address = address;
            return this;
        }

        public Client WithPassport(string passport)
        {
            Passport = passport;
            return this;
        }

        public override string ToString()
        {
            string address = Address;
            if (Address == null) address = "-";

            string passport = Passport;
            if (Passport == null) passport = "-";

            return "(" + Surname + " " + Name + ", " + address + ", " + passport + ")";
        }
    }
}