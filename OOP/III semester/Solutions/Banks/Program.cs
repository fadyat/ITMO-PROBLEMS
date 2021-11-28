using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Banks.Accounts;
using Banks.Banks;
using Banks.Banks.Limits;
using Banks.Clients;
using Banks.UI;
using Spectre.Console;

namespace Banks
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            List<string> FindInheritedClasses(Type mainType)
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
            }

            List<string> bankTypes = FindInheritedClasses(typeof(IBank));
            List<string> accountTypes = FindInheritedClasses(typeof(Account));
            List<string> clientTypes = FindInheritedClasses(typeof(IClient));
            List<string> limitTypes = FindInheritedClasses(typeof(Limit));

            while (true)
            {
                Console.Clear();
                string option = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("Basic options: ")
                    .AddChoices(new List<string>
                    {
                        "CreateBank", "BankOptions",
                        "CentralBank", "Exit",
                    }));

                if (option == "Exit" && !AnsiConsole.Confirm("Continue?"))
                {
                    break;
                }

                switch (option)
                {
                    case "CreateBank":
                    {
                        Creations.BankCreation(bankTypes, limitTypes);
                        break;
                    }

                    case "BankOptions":
                    {
                        IBank pickedBank = Selections.BankSelection();
                        if (pickedBank == null)
                        {
                            AnsiConsole.Prompt(
                                new TextPrompt<string>("[red]No banks[/]").AllowEmpty());
                            break;
                        }

                        string bankMethodName = AnsiConsole.Prompt(new SelectionPrompt<string>()
                            .Title("What operation you want to do?")
                            .AddChoices(new List<string>()
                            {
                                "AddClient", "RegisterAccount",
                                "TopUp", "WithDraw", "Transfer",
                                "ChangeLimit", "Calculate",
                                "EnterClientData", "Exit",
                            }));

                        if (bankMethodName == "Exit") break;

                        if (bankMethodName == "ChangeLimit")
                        {
                            Limit newLimit = Creations.LimitCreation(limitTypes);
                            pickedBank.UpdateLimit(newLimit);
                            break;
                        }

                        if (bankMethodName == "AddClient")
                        {
                            pickedBank.AddClient(Creations.ClientCreation(pickedBank, clientTypes));
                            break;
                        }

                        IClient pickedClient = Selections.ClientSelection(pickedBank.Clients);
                        if (pickedClient == null)
                        {
                            AnsiConsole.Prompt(
                                new TextPrompt<string>("[red]No clients[/]").AllowEmpty());
                            break;
                        }

                        switch (bankMethodName)
                        {
                            case "RegisterAccount":
                                pickedBank.RegisterAccount(pickedClient, Creations.AccountCreation(accountTypes));
                                break;

                            case "EnterClientData":
                            {
                                if (pickedClient.Passport == null || pickedClient.Address == null)
                                {
                                    if (pickedClient.Passport == null)
                                    {
                                        string passport =
                                            AnsiConsole.Prompt(new TextPrompt<string>("Passport: ").AllowEmpty());
                                        if (passport != string.Empty)
                                        {
                                            pickedClient.WithPassport(passport);
                                        }
                                    }

                                    if (pickedClient.Address == null)
                                    {
                                        string address =
                                            AnsiConsole.Prompt(new TextPrompt<string>("Address: ").AllowEmpty());
                                        if (address != string.Empty)
                                        {
                                            pickedClient.WithAddress(address);
                                        }
                                    }
                                }
                                else
                                {
                                    AnsiConsole.Prompt(
                                        new TextPrompt<string>("[red]Already full data[/]").AllowEmpty());
                                }

                                break;
                            }

                            default:
                            {
                                Account pickedAccount =
                                    Selections.AccountSelection(pickedBank.Accounts[pickedClient.Id]);
                                if (pickedAccount == null)
                                {
                                    AnsiConsole.Prompt(
                                        new TextPrompt<string>("[red]No accounts[/]").AllowEmpty());
                                    break;
                                }

                                if (bankMethodName == "Calculate")
                                {
                                    int days = AnsiConsole.Ask<int>("How many days ahead do we count?");
                                    DateTime inTime = pickedAccount.Date.AddDays(days);
                                    Account accountBe = pickedBank.Calculate(pickedClient, pickedAccount, inTime);
                                    AnsiConsole.Prompt(
                                        new TextPrompt<string>(
                                            $"Balance {pickedAccount.Balance} -> {accountBe.Balance}").AllowEmpty());
                                    break;
                                }

                                double amount = AnsiConsole.Ask<double>($"Balance to {bankMethodName}: ");
                                switch (bankMethodName)
                                {
                                    case "TopUp":
                                        pickedBank.TopUp(pickedClient, pickedAccount, amount);
                                        break;

                                    case "WithDraw":
                                        pickedBank.WithDraw(pickedClient, pickedAccount, amount);
                                        break;

                                    case "Transfer":
                                        IClient toClient =
                                            Selections.ClientSelection(CentralBank.GetAllClients());
                                        Account pickedAccount2 =
                                            Selections.AccountSelection(
                                                CentralBank.GetAllAccounts(toClient));
                                        if (pickedAccount2 == null)
                                        {
                                            AnsiConsole.Prompt(
                                                new TextPrompt<string>("[red]No accounts[/]").AllowEmpty());
                                            break;
                                        }

                                        pickedBank.Transfer(pickedClient, pickedAccount, pickedAccount2, amount);
                                        break;
                                }

                                break;
                            }
                        }

                        break;
                    }

                    case "CentralBank":
                    {
                        string viewType = AnsiConsole.Prompt(new SelectionPrompt<string>()
                            .Title("What operation you want to do?")
                            .AddChoices(new List<string>()
                            {
                                "BankTree", "LimitTree",
                                "Exit",
                            }));

                        if (viewType == "Exit") break;

                        switch (viewType)
                        {
                            case "BankTree":
                                Messages.BankTree();
                                break;
                            case "LimitTree":
                                Messages.LimitTree();
                                break;
                        }

                        AnsiConsole.Prompt(new TextPrompt<string>("Continue...")
                            .AllowEmpty());

                        break;
                    }
                }
            }
        }
    }
}