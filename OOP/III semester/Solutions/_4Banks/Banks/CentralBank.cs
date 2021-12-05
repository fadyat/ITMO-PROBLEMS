using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Banks.Accounts;
using Banks.Accounts.Command;
using Banks.Banks.Chain;
using Banks.Banks.Limits;
using Banks.Clients;

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
            bank.AddClient(client);
        }

        public static IBank GetBank(Guid id)
        {
            return Banks.Single(b => Equals(b.Id, id));
        }

        public static void RegisterAccount(IBank bank, IClient client, Account account)
        {
            bank = GetBank(bank.Id);
            bank.RegisterAccount(client, account);
        }

        public static IEnumerable<IClient> GetAllClients()
        {
            return Banks.SelectMany(bank => bank.Clients).Distinct();
        }

        public static IEnumerable<Account> GetAllAccounts(IClient client)
        {
            var accounts = new List<Account>();
            foreach (ReadOnlyCollection<Account> accountInThisBank in Banks
                .SelectMany(b => b.Clients.Where(c => client.Id == c.Id)
                    .Select(c => b.Accounts[c.Id])))
            {
                accounts.AddRange(accountInThisBank);
            }

            return accounts;
        }

        public static void Operation(AccountCommand accountCommand, Handler check)
        {
            accountCommand.Execute();
            Operations.Push(accountCommand);
            if (check.HandlerRequest()) return;

            Operations.Pop().Undo();
        }

        public static Account Calculate(Account account, Limit limit, DateTime inDate)
        {
            return account.Calculate(limit, inDate);
        }
    }
}