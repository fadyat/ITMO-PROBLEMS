using System;
using Banks.Banks.Limits;

namespace Banks.Accounts
{
    public abstract class Account
    {
        protected Account(double balance, DateTime date)
        {
            Balance = balance;
            Date = date;
            PrevCalcDate = date;
            Id = Guid.NewGuid();
        }

        public double Balance { get; protected set; }

        public DateTime Date { get; }

        public Guid Id { get; }

        protected DateTime PrevCalcDate { get; set; }

        /* For Approved operations */
        public abstract bool ApprovedTopUp(Limit limit);

        public abstract bool ApprovedWithDraw(Limit limit);

        public virtual bool ApprovedTransfer(Account toAccount, Limit limit)
        {
            return ApprovedWithDraw(limit) && toAccount.ApprovedTopUp(limit);
        }

        /* Operations */
        public void TopUp(double amount)
        {
            Balance += amount;
        }

        public void WithDraw(double amount)
        {
            Balance -= amount;
        }

        public void Transfer(Account toAccount, double amount)
        {
            WithDraw(amount);
            toAccount.TopUp(amount);
        }

        /* For TimeMachine */
        public abstract Account Calculate(Limit limit, DateTime dateTime);
    }
}