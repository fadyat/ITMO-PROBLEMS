namespace Banks.Accounts.Command
{
    public class WithDrawCommand : AccountCommand
    {
        public WithDrawCommand(Account account, double amount)
            : base(account, amount)
        {
        }

        public override void Execute()
        {
            Account.WithDraw(Amount);
        }

        public override void Undo()
        {
            Account.WithDraw(-Amount);
        }
    }
}