using Banks.Accounts;
using Banks.Banks.Limits;
using Spectre.Console;

namespace Banks.Banks.Chain
{
    public class AccountWithDrawHandler : Handler
    {
        private readonly Account _account;
        private readonly Limit _limit;

        public AccountWithDrawHandler(Account account, Limit limit, Handler successor = null)
            : base(successor)
        {
            _account = account;
            _limit = limit;
        }

        public override bool HandlerRequest()
        {
            if (!_account.ApprovedWithDraw(_limit))
            {
                AnsiConsole.WriteLine("[red]WithDraw don't approved![/]");
                return false;
            }

            return base.HandlerRequest();
        }
    }
}