namespace Banks.Accounts.Command
{
    public class TransferCommand : AccountCommand
    {
        public TransferCommand(Account account, Account to, double amount)
            : base(account, amount)
        {
            ToAccount = to;
        }

        private Account ToAccount { get; }

        public override void Execute()
        {
            Account.Transfer(ToAccount, Amount);
        }

        public override void Undo()
        {
            ToAccount.Transfer(Account, Amount);
        }
    }
}