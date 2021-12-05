using Banks.Accounts;
using Banks.Banks;
using Banks.Banks.Limits;
using Banks.Clients;
using Spectre.Console;

namespace Banks.UI
{
    public static class Messages
    {
        public static void BankTree()
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
                        clientNode.AddNode(account.ToString() ?? string.Empty);
                    }
                }
            }

            AnsiConsole.Write(root);
        }

        public static void LimitTree()
        {
            var root = new Tree(nameof(CentralBank));
            foreach (IBank bank in CentralBank.AllBanks)
            {
                TreeNode bankNode = root.AddNode(bank.ToString() ?? string.Empty);
                Limit limit = bank.Limit;
                bankNode.AddNode($"DebitPercent: {limit.DebitPercent}");
                TreeNode depositPercent = bankNode.AddNode("Deposit percent: ");
                foreach ((int key, double value) in limit.DepositPercent)
                {
                    depositPercent.AddNode(
                        $"{key}, {value.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
                }

                bankNode.AddNode(
                    $"CreditLimit: {limit.CreditLimit[0].ToString(System.Globalization.CultureInfo.InvariantCulture)}," +
                    $"{limit.CreditLimit[1].ToString(System.Globalization.CultureInfo.InvariantCulture)}");
                bankNode.AddNode($"CreditCommission: {limit.CreditCommission}");
                bankNode.AddNode($"WithDrawLimit: {limit.WithDrawLimit}");
                bankNode.AddNode($"TransferLimit: {limit.TransferLimit}");
            }

            AnsiConsole.Write(root);
        }
    }
}