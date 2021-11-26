using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Accounts;
using Banks.Accounts.Command;
using Banks.Banks.Chain;
using Banks.Banks.Limits;
using Banks.Clients;

namespace Banks.Banks
{
    public class Bank : IBank
    {
        public Bank(Guid id, Limit limit)
        {
            Id = id;
            Limit = limit;
            Clients = new HashSet<IClient>();
            Accounts = new Dictionary<Guid, List<Account>>();
        }

        public Guid Id { get; }

        public Dictionary<Guid, List<Account>> Accounts { get; }

        public HashSet<IClient> Clients { get; }

        public Limit Limit { get; }

        public void AddClient(IClient client)
        {
            CentralBank.AddClient(this, client);
        }

        public void RegisterAccount(IClient client, Account account)
        {
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
            from = GetAccount(client.Id, from.Id);

            var accountCheck = new AccountTransferHandler(from, too, Limit);
            var clientCheck = new ClientTransferHandler(client, Limit, amount, accountCheck);
            var check = new Handler(clientCheck);

            CentralBank.Operation(new TransferCommand(from, too, amount), check);
        }

        public void Print()
        {
            Console.WriteLine($"# B: {Id}");
            foreach (IClient c in Clients)
                c.Print();

            foreach ((Guid key, List<Account> value) in Accounts)
            {
                Console.Write("C: " + key + " ");
                foreach (Account aa in value)
                    aa.Print();

                Console.Write("\n");
            }
        }
    }
}