using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Accounts;
using Banks.Accounts.Command;
using Banks.Banks.Chain;
using Banks.Banks.Limits;
using Banks.Clients;
using Banks.UI;
using Spectre.Console;

namespace Banks.Banks
{
    public class Bank : IBank
    {
        public Bank(string name, Guid id, Limit limit)
        {
            Name = name;
            Id = id;
            Limit = limit;
            Clients = new HashSet<IClient>();
            Accounts = new Dictionary<Guid, List<Account>>();
            CentralBank.AddBank(this);
        }

        public Guid Id { get; }

        public Dictionary<Guid, List<Account>> Accounts { get; }

        public HashSet<IClient> Clients { get; }

        public string Name { get; }

        public Limit Limit { get; private set; }

        public IClient AddClient(string surname, string name, string passport, string address)
        {
            var registeredClient = new Client(surname, name, Guid.NewGuid(), address, passport);
            CentralBank.AddClient(this, registeredClient);
            return registeredClient;
        }

        public void RegisterAccount(IClient client, Account account)
        {
            client = GetClient(client.Id);
            CentralBank.RegisterAccount(this, client, account);
        }

        public IClient GetClient(Guid id)
        {
            return Clients.Single(c => Equals(c.Id, id));
        }

        public Account GetAccount(Guid clientId, Guid accountId)
        {
            return Accounts[clientId].Single(a => Equals(a.Id, accountId));
        }

        public bool ContainsClient(Guid id)
        {
            return Clients.Any(c => c.Id == id);
        }

        public void TopUp(IClient client, Account account, double amount)
        {
            account = GetAccount(client.Id, account.Id);

            var accountCheck = new AccountTopUpHandler(account, Limit);
            var check = new Handler(accountCheck);

            CentralBank.Operation(new TopUpCommand(account, amount), check);
        }

        public void WithDraw(IClient client, Account account, double amount)
        {
            account = GetAccount(client.Id, account.Id);

            var accountCheck = new AccountWithDrawHandler(account, Limit);
            var clientCheck = new ClientWithDrawHandler(client, Limit, amount, accountCheck);
            var check = new Handler(clientCheck);

            CentralBank.Operation(new WithDrawCommand(account, amount), check);
        }

        public void Transfer(IClient client, Account from, Account too, double amount)
        {
            if (from.Id == too.Id)
            {
                AnsiConsole.WriteLine("[red]Accounts can't be equal![/]");
                return;
            }

            from = GetAccount(client.Id, from.Id);

            var accountCheck = new AccountTransferHandler(from, too, Limit);
            var clientCheck = new ClientTransferHandler(client, Limit, amount, accountCheck);
            var check = new Handler(clientCheck);

            CentralBank.Operation(new TransferCommand(from, too, amount), check);
        }

        public void UpdateLimit(Limit limit)
        {
            Limit = limit;
            AnsiConsole.WriteLine("[red]Attention! Bank limit has changed![/]");
        }

        public Account Calculate(IClient client, Account account, DateTime inDate)
        {
            client = GetClient(client.Id);
            account = GetAccount(client.Id, account.Id);
            return CentralBank.Calculate(account, Limit, inDate);
        }

        public override string ToString()
        {
            return "(" + Name + ", " + Id + ")";
        }
    }
}