using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Accounts;
using Banks.Accounts.Command;
using Banks.Banks.Chain;
using Banks.Banks.Limits;
using Banks.Clients;
using Banks.Exceptions;

namespace Banks.Banks
{
    public static class CentralBank
    {
        private static readonly List<IBank> Banks;
        private static readonly Stack<AccountCommand> Operations;

        static CentralBank()
        {
            Banks = new List<IBank>();
            Operations = new Stack<AccountCommand>();
        }

        public static IEnumerable<IBank> AllBanks => Banks;

        public static void AddBank(IBank bank)
        {
            Banks.Add(bank);
        }

        public static void AddClient(IBank bank, IClient client)
        {
            bank = GetBank(bank.Id);
            if (bank.ContainsClient(client.Id))
            {
                throw new ClientException("This client is already in this bank!");
            }

            bank.Clients.Add(client);
            bank.Accounts[client.Id] = new List<Account>();
        }

        public static IBank GetBank(Guid id)
        {
            return Banks.Single(b => Equals(b.Id, id));
        }

        public static IBank FindBank(Guid id)
        {
            return Banks.FirstOrDefault(b => Equals(b.Id, id));
        }

        public static void RegisterAccount(IBank bank, IClient client, Account account)
        {
            bank = GetBank(bank.Id);
            client = bank.GetClient(client.Id);

            bank.Accounts[client.Id].Add(account);
        }

        public static void Operation(AccountCommand accountCommand, Handler check)
        {
            accountCommand.Execute();
            Operations.Push(accountCommand);
            if (check.HandlerRequest())
            {
                return;
            }

            Operations.Pop()
                .Undo();
        }

        public static Account Calculate(Account account, Limit limit, DateTime inDate)
        {
            return account.Calculate(limit, inDate);
        }

        public static void Print()
        {
            foreach (IBank b in Banks)
                b.Print();
        }
    }
}