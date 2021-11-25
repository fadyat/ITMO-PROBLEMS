using Banks.Accounts;
using Banks.Banks.Limits;

namespace Banks.Banks.Chain
{
    public class AccountWithDrawHandler : AccountHandler
    {
        public AccountWithDrawHandler(Account account, Limit limit, Handler successor = null)
            : base(account, limit, successor) { }

        public override bool HandlerRequest()
        {
            return Account.ApprovedWithDraw(Limit) && base.HandlerRequest();
        }
    }
}