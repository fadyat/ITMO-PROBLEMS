using Banks.Accounts;
using Banks.Banks;
using Banks.Clients;
using Spectre.Console;

namespace Banks.UI
{
    public static class Messages
    {
        public static void EmptyPrompt(string message)
        {
            AnsiConsole.Prompt(new TextPrompt<string>(message).AllowEmpty());
        }

        public static void BuildTree()
        {
            var root = new Tree(nameof(CentralBank));
            foreach (IBank bank in CentralBank.AllBanks)
            {
                TreeNode bankNode = root.AddNode(bank.ToString() ?? string.Empty);
                foreach (IClient client in bank.Clients)
                {
                    TreeNode clientNode = bankNode.AddNode(client.ToString() ?? string.Empty);
                    foreach (Account account in bank.Accounts[client.Id])
                    {
                        clientNode.AddNode(account.ToString());
                    }
                }
            }

            AnsiConsole.Write(root);
        }
    }
}