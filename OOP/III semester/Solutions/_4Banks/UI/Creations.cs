using System;
using System.Collections.Generic;
using Banks.Accounts;
using Banks.Banks;
using Banks.Banks.Limits;
using Banks.Clients;
using Spectre.Console;

namespace Banks.UI
{
    public static class Creations
    {
        public static IBank BankCreation(IEnumerable<string> bankTypes, IEnumerable<string> limitTypes)
        {
            string bankType = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("What's type of bank you want to create?")
                .AddChoices(bankTypes));

            switch (bankType)
            {
                case nameof(Bank):
                {
                    string name = AnsiConsole.Ask<string>("Enter bank name:");
                    Limit limit = LimitCreation(limitTypes);
                    return new Bank(name, Guid.NewGuid(), limit);
                }

                default:
                    return null;
            }
        }

        public static Limit LimitCreation(IEnumerable<string> limitTypes)
        {
            string[] StringToPairParser(string message)
            {
                string pair = AnsiConsole.Prompt(
                    new TextPrompt<string>(message)
                        .Validate(ap =>
                        {
                            string[] split = ap.Split(' ');
                            return split.Length switch
                            {
                                2 => ValidationResult.Success(),
                                _ => ValidationResult.Error("[red]Invalid data[/]"),
                            };
                        }));

                return pair.Split(' ');
            }

            string limitType = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("What's type of limit you want to create?")
                .AddChoices(limitTypes));

            switch (limitType)
            {
                case nameof(SimpleBankLimit):
                {
                    double debitPercent = AnsiConsole.Ask<double>("Debit percent (%): ");
                    var depositPercent = new SortedDictionary<int, double>();
                    AnsiConsole.WriteLine("Deposit percents (amount, percent) x3: ");
                    while (depositPercent.Count < 3)
                    {
                        string[] depositPercentPair =
                            StringToPairParser($"{depositPercent.Count + 1}#: ");

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
                    double[] creditLimit = { Convert.ToDouble(creditLimitPair[0]), Convert.ToDouble(creditLimitPair[1]) };
                    double creditCommission = AnsiConsole.Ask<double>("Credit commission: ");
                    double withDrawLimit = AnsiConsole.Ask<double>("WithDraw limit: ");
                    double transferLimit = AnsiConsole.Ask<double>("Transfer limit: ");
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

        public static IClient ClientCreation(IBank bank, IEnumerable<string> clientTypes)
        {
            string clientType = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("What's type of client you want to create?")
                .AddChoices(clientTypes));

            switch (clientType)
            {
                case nameof(Client):
                {
                    string name = AnsiConsole.Ask<string>("Name: ");
                    string surname = AnsiConsole.Ask<string>("Surname: ");
                    string passport = AnsiConsole.Prompt(new TextPrompt<string>("Passport: ").AllowEmpty());
                    if (passport == string.Empty) passport = null;
                    string address = AnsiConsole.Prompt(new TextPrompt<string>("Address: ").AllowEmpty());
                    if (address == string.Empty) address = null;
                    return new Client(surname, name, Guid.NewGuid(), address, passport);
                }

                default:
                    return null;
            }
        }

        public static Account AccountCreation(IEnumerable<string> accountTypes)
        {
            string accountType = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("What's type of account you want to create?")
                .AddChoices(accountTypes));

            double balance = AnsiConsole.Ask<double>("Investments: ");
            switch (accountType)
            {
                case nameof(DebitAccount):
                    DateTime date = DateTime.Now;
                    return new DebitAccount(balance, date);

                case nameof(DepositAccount):
                    date = DateTime.Now;
                    int month = AnsiConsole.Ask<int>("How many month: ");
                    return new DepositAccount(balance, date, date.AddYears(month));

                case nameof(CreditAccount):
                    date = DateTime.Now;
                    return new CreditAccount(balance, date);

                default:
                    return null;
            }
        }
    }
}