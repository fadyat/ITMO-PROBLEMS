using Banks.Accounts;
using Banks.Banks.Limits;
using Banks.UI;
using Spectre.Console;

namespace Banks.Banks.Chain
{
    public class AccountTopUpHandler : Handler
    {
        private readonly Account _account;
        private readonly Limit _limit;

        public AccountTopUpHandler(Account account, Limit limit, Handler successor = null)
            : base(successor)
        {
            _account = account;
            _limit = limit;
        }

        public override bool HandlerRequest()
        {
            if (!_account.ApprovedTopUp(_limit))
            {
                AnsiConsole.WriteLine("[red]TopUp don't approved![/]");
                return false;
            }

            return base.HandlerRequest();
        }
    }
}