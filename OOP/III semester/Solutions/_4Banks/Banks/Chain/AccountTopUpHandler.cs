using Banks.Accounts;
using Banks.Banks.Limits;

namespace Banks.Banks.Chain
{
    public class AccountTopUpHandler : AccountHandler
    {
        public AccountTopUpHandler(Account account, Limit limit, Handler successor = null)
            : base(account, limit, successor) { }

        public override bool HandlerRequest()
        {
            return Account.ApprovedTopUp(Limit) && base.HandlerRequest();
        }
    }
}