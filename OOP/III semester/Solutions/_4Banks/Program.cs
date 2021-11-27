using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Banks.Accounts;
using Banks.Banks;
using Banks.Banks.Limits;
using Banks.Clients;
using Spectre.Console;

namespace Banks
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var findInheritedClasses = new Func<Type, List<string>>((mainType) =>
            {
                if (mainType.IsInterface)
                {
                    return Assembly.GetAssembly(mainType)?
                        .GetTypes()
                        .Where(type => mainType.IsAssignableFrom(type) && mainType != type)
                        .Select(type => type.Name)
                        .ToList();
                }

                if (mainType.IsAbstract)
                {
                    return Assembly.GetAssembly(mainType)?
                        .GetTypes()
                        .Where(type => type.IsSubclassOf(mainType))
                        .Select(type => type.Name)
                        .ToList();
                }

                return null;
            });

            Type bankInterface = typeof(IBank);
            List<string> bankTypes = findInheritedClasses(bankInterface);

            Type accountAbstract = typeof(Account);
            List<string> accountTypes = findInheritedClasses(accountAbstract);

            Type clientInterface = typeof(IClient);
            List<string> clientTypes = findInheritedClasses(clientInterface);

            Type limitAbstract = typeof(Limit);
            List<string> limitTypes = findInheritedClasses(limitAbstract);

            if (bankTypes == null || accountTypes == null ||
                clientTypes == null || limitTypes == null)
            {
                return;
            }

            string[] StringToPairParser(string message)
            {
                string pair = AnsiConsole.Prompt(
                    new TextPrompt<string>(message)
                        .Validate(ap =>
                        {
                            string[] split = ap.Split(' ');
                            return split.Length switch
                            {
                                <2 => ValidationResult.Error("[red]not enough args[/]"),
                                >2 => ValidationResult.Error("[red]too much args[/]"),
                                _ => ValidationResult.Success(),
                            };
                        }));

                return pair.Split(' ');
            }

            IBank BankCreation()
            {
                string bankType = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("What's type of bank you want to create?")
                    .AddChoices(bankTypes));

                switch (bankType)
                {
                    case nameof(Bank):
                        string name = AnsiConsole.Ask<string>("What's your bank name?");
                        Limit limit = LimitCreation();
                        return new Bank(name, Guid.NewGuid(), limit);

                    default:
                        return null;
                }
            }

            Limit LimitCreation()
            {
                string limitType = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("What's type of limit you want to create?")
                    .AddChoices(limitTypes));

                switch (limitType)
                {
                    case nameof(SimpleBankLimit):
                        double debitPercent = AnsiConsole.Ask<double>("Debit percent (%): ");
                        var depositPercent = new SortedDictionary<int, double>();
                        while (depositPercent.Count < 3)
                        {
                            string[] depositPercentPair =
                                StringToPairParser($"{depositPercent.Count + 1}# amount percent:");

                            int key = Convert.ToInt32(depositPercentPair[0]);
                            double value = Convert.ToDouble(depositPercentPair[1]);
                            if (!depositPercent.ContainsKey(key))
                            {
                                depositPercent.Add(key, value);
                            }
                            else
                            {
                                depositPercent[key] = value;
                            }
                        }

                        string[] creditLimitPair = StringToPairParser("Credit limit (down, up): ");
                        var creditLimit = (Convert.ToInt32(creditLimitPair[0]), Convert.ToInt32(creditLimitPair[1]));
                        double creditCommission = AnsiConsole.Ask<double>("Credit commission: ");
                        double withDrawLimit = AnsiConsole.Ask<double>("With draw limit for incomplete clients: ");
                        double transferLimit = AnsiConsole.Ask<double>("Transfer limit for incomplete clients: ");
                        return new SimpleBankLimit(
                            debitPercent,
                            depositPercent,
                            creditLimit,
                            creditCommission,
                            withDrawLimit,
                            transferLimit);

                    default:
                        return null;
                }
            }

            IClient ClientCreation()
            {
                string clientType = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("What's type of client you want to create?")
                    .AddChoices(clientTypes));

                switch (clientType)
                {
                    case nameof(Client):
                        string name = AnsiConsole.Ask<string>("What's your client name?");
                        string surname = AnsiConsole.Ask<string>("What's your client surname?");
                        string passport = AnsiConsole.Prompt(new TextPrompt<string>("What's your client passport?")
                            .AllowEmpty());
                        string address = AnsiConsole.Prompt(new TextPrompt<string>("What's your client address?")
                            .AllowEmpty());
                        return new Client.ClientBuilder().WithName(name)
                            .WithSurname(surname)
                            .WithId(Guid.NewGuid())
                            .WithPassport(passport)
                            .WithAddress(address)
                            .Build();

                    default:
                        return null;
                }
            }

            Account AccountCreation()
            {
                string accountType = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("What's type of account you want to create?")
                    .AddChoices(accountTypes));

                switch (accountType)
                {
                    case nameof(DebitAccount):
                        double balance = AnsiConsole.Ask<double>("How much do you invest in a bank account?");
                        DateTime date = DateTime.Now;
                        return new DebitAccount(balance, date);

                    case nameof(DepositAccount):
                        balance = AnsiConsole.Ask<double>("How much do you invest in a bank account?");
                        date = DateTime.Now;
                        int month = AnsiConsole.Ask<int>("For how many month do you open an account?");
                        return new DepositAccount(balance, date, date.AddYears(month));

                    case nameof(CreditAccount):
                        balance = AnsiConsole.Ask<double>("How much do you invest in a bank account?");
                        date = DateTime.Now;
                        return new CreditAccount(balance, date);

                    default:
                        return null;
                }
            }

            IBank BankSelection()
            {
                SelectionPrompt<(string name, Guid id)> selection = new SelectionPrompt<(string, Guid)>()
                    .Title("What bank you want to pick?");

                if (!CentralBank.AllBanks.Any())
                {
                    return null;
                }

                foreach (IBank b in CentralBank.AllBanks)
                {
                    selection.AddChoice((b.Name, b.Id));
                }

                (string name, Guid id) bank = AnsiConsole.Prompt(selection);

                return CentralBank.GetBank(bank.id);
            }

            IClient ClientSelection(List<IClient> clients)
            {
                SelectionPrompt<(string name, Guid id)> selection = new SelectionPrompt<(string, Guid)>()
                    .Title("What client you want to pick?");

                if (!clients.Any())
                {
                    return null;
                }

                foreach (IClient c in clients)
                {
                    selection.AddChoice((c.Name, c.Id));
                }

                (string name, Guid id) client = AnsiConsole.Prompt(selection);
                return clients.First(c => c.Id == client.id);
            }

            Account AccountSelection(List<Account> accounts)
            {
                SelectionPrompt<(string name, double balance, Guid id)> selection =
                    new SelectionPrompt<(string, double, Guid)>()
                        .Title("What account you want to pick?");

                if (!accounts.Any())
                {
                    return null;
                }

                foreach (Account a in accounts)
                {
                    selection.AddChoice((nameof(a), a.Balance, a.Id));
                }

                (string name, double balance, Guid id) account = AnsiConsole.Prompt(selection);
                return accounts.First(c => c.Id == account.id);
            }

            var allClients = new List<IClient>();

            while (true)
            {
                Console.Clear();
                string option = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("Basic options:")
                    .AddChoices(new List<string>
                    {
                        "CreateBank", "CreateClient", "BankOptions", "CentralBank", "Exit",
                    }));

                switch (option)
                {
                    case "CreateBank":
                    {
                        BankCreation();
                        break;
                    }

                    case "CreateClient":
                    {
                        allClients.Add(ClientCreation());
                        break;
                    }

                    case "BankOptions":
                    {
                        IBank pickedBank = BankSelection();
                        if (pickedBank == null)
                        {
                            AnsiConsole.Prompt(new TextPrompt<string>("[red]There are no banks :( [/]")
                                .AllowEmpty());
                            break;
                        }

                        string bankMethodName = AnsiConsole.Prompt(new SelectionPrompt<string>()
                            .Title("What operation you want to do?")
                            .AddChoices(new List<string>()
                            {
                                "AddClient", "RegisterAccount",
                                "TopUp", "WithDraw", "Transfer",
                                "Calculate",
                            }));

                        IClient pickedClient = bankMethodName switch
                        {
                            "AddClient" => ClientSelection(allClients),
                            _ => ClientSelection(pickedBank.Clients.ToList())
                        };

                        if (pickedClient == null)
                        {
                            AnsiConsole.Prompt(new TextPrompt<string>("[red]There are no clients :( [/]")
                                .AllowEmpty());
                            break;
                        }

                        switch (bankMethodName)
                        {
                            case "AddClient":
                                pickedBank.AddClient(pickedClient);
                                break;

                            case "RegisterAccount":
                                pickedBank.RegisterAccount(pickedClient, AccountCreation());
                                break;

                            default:
                            {
                                Account pickedAccount = AccountSelection(pickedBank.Accounts[pickedClient.Id]);
                                if (pickedAccount == null)
                                {
                                    AnsiConsole.Prompt(new TextPrompt<string>("[red]There are no accounts :( [/]")
                                        .AllowEmpty());
                                    break;
                                }

                                double amount = AnsiConsole.Ask<double>($"Balance to {bankMethodName}");
                                switch (bankMethodName)
                                {
                                    case "TopUp":
                                        pickedBank.TopUp(pickedClient, pickedAccount, amount);
                                        break;

                                    case "WithDraw":
                                        pickedBank.WithDraw(pickedClient, pickedAccount, amount);
                                        break;

                                    case "Transfer":
                                        Account pickedAccount2 = AccountSelection(pickedBank.Accounts[pickedClient.Id]);
                                        pickedBank.Transfer(pickedClient, pickedAccount, pickedAccount2, amount);
                                        break;

                                    case "Calculate":
                                        break;
                                }

                                break;
                            }
                        }

                        break;
                    }

                    case "CentralBank":
                    {
                        CentralBank.Print();
                        AnsiConsole.Prompt(new TextPrompt<string>(string.Empty)
                            .AllowEmpty());
                        break;
                    }
                }

                if (option == "Exit")
                {
                    break;
                }
            }
        }
    }
}