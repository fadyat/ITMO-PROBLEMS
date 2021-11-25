using Banks.Accounts;
using Banks.Banks.Limits;

namespace Banks.Banks.Chain
{
    public class AccountHandler : Handler
    {
        private readonly Account _account;
        private readonly ILimit _limit;

        public AccountHandler(Account account, ILimit limit, Handler successor = null)
            : base(successor)
        {
            _account = account;
            _limit = limit;
        }

        public override bool HandlerRequest()
        {
            _account.Check(_limit);

            // need check for all kinds of operations
            // for +
            // for -
            // for ->
            return base.HandlerRequest();
        }
    }
}