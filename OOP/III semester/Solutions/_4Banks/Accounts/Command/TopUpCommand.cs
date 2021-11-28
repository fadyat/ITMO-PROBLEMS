namespace Banks.Accounts.Command
{
    public class TopUpCommand : AccountCommand
    {
        public TopUpCommand(Account account, double amount)
            : base(account, amount)
        {
        }

        public override void Execute()
        {
            Account.TopUp(Amount);
        }

        public override void Undo()
        {
            Account.TopUp(-Amount);
        }
    }
}