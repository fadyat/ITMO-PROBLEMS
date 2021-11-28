using System;
using Banks.Accounts;
using Banks.Banks.Limits;
using Banks.UI;

namespace Banks.Banks.Chain
{
    public class AccountTransferHandler : Handler
    {
        private readonly Account _account;
        private readonly Account _toAccount;
        private readonly Limit _limit;

        public AccountTransferHandler(Account account, Account toAccount, Limit limit, Handler successor = null)
            : base(successor)
        {
            _account = account;
            _toAccount = toAccount;
            _limit = limit;
        }

        public override bool HandlerRequest()
        {
            if (!_account.ApprovedTransfer(_toAccount, _limit))
            {
                Messages.EmptyPrompt("[red]Transfer don't approved![/]");
                return false;
            }

            return base.HandlerRequest();
        }
    }
}