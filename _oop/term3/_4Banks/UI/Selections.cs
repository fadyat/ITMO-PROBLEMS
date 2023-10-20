using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Accounts;
using Banks.Banks;
using Banks.Clients;
using Spectre.Console;

namespace Banks.UI
{
    public static class Selections
    {
        public static IBank BankSelection()
        {
            SelectionPrompt<(string name, Guid id)> selection = new SelectionPrompt<(string, Guid)>()
                .Title("What bank you want to pick?");

            if (!CentralBank.AllBanks.Any()) return null;

            foreach (IBank b in CentralBank.AllBanks)
            {
                selection.AddChoice((b.Name, b.Id));
            }

            (string name, Guid id) bank = AnsiConsole.Prompt(selection);

            return CentralBank.GetBank(bank.id);
        }

        public static IClient ClientSelection(IEnumerable<IClient> clients)
        {
            clients = clients.ToList();
            SelectionPrompt<(string name, Guid id)> selection = new SelectionPrompt<(string, Guid)>()
                .Title("What client you want to pick?");

            if (!clients.Any()) return null;

            foreach (IClient c in clients)
            {
                selection.AddChoice((c.Surname + " " + c.Name, c.Id));
            }

            (string name, Guid id) client = AnsiConsole.Prompt(selection);
            return clients.First(c => c.Id == client.id);
        }

        public static Account AccountSelection(IEnumerable<Account> accounts)
        {
            accounts = accounts.ToList();
            SelectionPrompt<(string name, double balance, Guid id)> selection =
                new SelectionPrompt<(string, double, Guid)>()
                    .Title("What account you want to pick?");

            if (!accounts.Any()) return null;

            foreach (Account a in accounts)
            {
                selection.AddChoice((nameof(a), a.Balance, a.Id));
            }

            (string name, double balance, Guid id) account = AnsiConsole.Prompt(selection);
            return accounts.First(c => c.Id == account.id);
        }
    }
}