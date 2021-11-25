using System;
using Banks.Accounts;
using Banks.Banks.Limits;

namespace Banks.Banks.Chain
{
    public abstract class AccountHandler : Handler
    {
        public AccountHandler(Account account, Limit limit, Handler successor = null)
            : base(successor)
        {
            Account = account;
            Limit = limit;
        }

        protected Account Account { get; }

        protected Limit Limit { get; }
    }
}