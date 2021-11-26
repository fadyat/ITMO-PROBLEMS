namespace Banks.Accounts.Command
{
    public abstract class AccountCommand
    {
        protected AccountCommand(Account account, double amount)
        {
            Account = account;
            Amount = amount;
        }

        protected Account Account { get; }

        protected double Amount { get; }

        public abstract void Execute();

        public abstract void Undo();
    }
}