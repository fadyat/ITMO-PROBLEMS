using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using Banks.Accounts;
using Banks.Accounts.Command;
using Banks.Banks.Chain;
using Banks.Banks.Limits;
using Banks.Clients;
using Banks.Exceptions;

namespace Banks.Banks
{
    public class Bank : IBank
    {
        private readonly Dictionary<Guid, List<Account>> _accounts;
        private readonly HashSet<IClient> _clients;

        public Bank(string name, Guid id, Limit limit)
        {
            Name = name;
            Id = id;
            Limit = limit;
            _clients = new HashSet<IClient>();
            _accounts = new Dictionary<Guid, List<Account>>();
            CentralBank.AddBank(this);
        }

        public Guid Id { get; }

        public IEnumerable<IClient> Clients => _clients;

        public string Name { get; }

        public Limit Limit { get; private set; }

        public ImmutableDictionary<Guid, ReadOnlyCollection<Account>> Accounts =>
            _accounts.ToImmutableDictionary(x => x.Key, x => x.Value.AsReadOnly());

        public void AddClient(IClient client)
        {
            if (ContainsClient(client.Id))
            {
                throw new BankException("This client is already in this bank!");
            }

            _clients.Add(client);
            _accounts[client.Id] = new List<Account>();
        }

        public void RegisterAccount(IClient client, Account account)
        {
            client = GetClient(client.Id);
            _accounts[client.Id].Add(account);
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
                throw new BankException("[red]Accounts can't be equal![/]");
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