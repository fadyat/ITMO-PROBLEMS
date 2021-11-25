using Banks.Accounts;
using Banks.Banks.Limits;

namespace Banks.Banks.Chain
{
    public class AccountTransferHandler : AccountHandler
    {
        public AccountTransferHandler(Account account, Account to, Limit limit, Handler successor = null)
            : base(account, limit, successor)
        {
            ToAccount = to;
        }

        private Account ToAccount { get; }
        public override bool HandlerRequest()
        {
            return Account.ApprovedTransfer(ToAccount, Limit) && base.HandlerRequest();
        }
    }
}