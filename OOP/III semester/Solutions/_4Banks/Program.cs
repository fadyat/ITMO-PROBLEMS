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
                switch (mainType.IsInterface)
                {
                    case true:
                        return Assembly.GetAssembly(mainType)?
                            .GetTypes()
                            .Where(type => mainType.IsAssignableFrom(type) && mainType != type)
                            .Select(type => type.Name)
                            .ToList();
                }

                switch (mainType.IsAbstract)
                {
                    case true:
                        return Assembly.GetAssembly(mainType)?
                            .GetTypes()
                            .Where(type => type.IsSubclassOf(mainType))
                            .Select(type => type.Name)
                            .ToList();
                    default:
                        return null;
                }
            });

            var findPublicMethodsInClass = new Func<Type, List<string>>((mainType) =>
            {
                return mainType.GetTypeInfo()
                    .GetMethods()
                    .Where(method => method.IsPublic &&
                                     !method.Name.Contains("get_") &&
                                     !method.Name.Contains("set_"))
                    .Select(type => type.Name)
                    .ToList();
            });

            Type bankInterface = typeof(IBank);
            List<string> bankTypes = findInheritedClasses(bankInterface);
            List<string> bankMethods = findPublicMethodsInClass(bankInterface);

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

            /*
            // CreateBank -- Type of bank
            // |
            // |____ CreateLimit -- Type of limit
            //
            // CreateClient -- Type of client -> to
            // CentralBank (get bank)
            // ...
            // CurrentBankOperations (if picked)
            // Exit
            */

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
                    .Title("What type of bank you want to create?")
                    .AddChoices(bankTypes));

                switch (bankType)
                {
                    case nameof(Bank):
                    {
                        string name = AnsiConsole.Ask<string>("What's your bank name?");
                        Limit limit = LimitCreation();
                        return new Bank(name, Guid.NewGuid(), limit);
                    }

                    default:
                        return null;
                }
            }

            Limit LimitCreation()
            {
                string limitType = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("What type of limit you want to create?")
                    .AddChoices(limitTypes));

                switch (limitType)
                {
                    case nameof(SimpleBankLimit):
                    {
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
                    }

                    default:
                        return null;
                }
            }

            IClient ClientCreation()
            {
                string clientType = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("What type of client you want to create?")
                    .AddChoices(clientTypes));

                switch (clientType)
                {
                    case nameof(Client):
                    {
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
                    }

                    default:
                        return null;
                }
            }

            var allClients = new List<IClient>();
            var allBanks = new List<IBank>();

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
                        string bankMethodName = AnsiConsole.Prompt(new SelectionPrompt<string>()
                            .Title("What operation you want to do?")
                            .AddChoices(bankMethods));

                        break;
                    }

                    case "CentralBank":
                    {
                        CentralBank.Print();
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