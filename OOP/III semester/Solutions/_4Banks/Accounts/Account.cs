using System;
using Banks.Banks.Limits;

namespace Banks.Accounts
{
    public abstract class Account
    {
        public Account(double money, DateTime date)
        {
            Balance = money;
            Date = date;
            PrevCalcDate = date;
        }

        public double Balance { get; protected set; }

        public DateTime Date { get; }

        public DateTime PrevCalcDate { get; protected set; }

        // void ?
        public abstract Account Calculate(ILimit limit, DateTime dateTime);

        /*
            public abstract Account TopUp(ILimit limit);

            public abstract Account WithDraw(ILimit limit);

            public abstract Account Transfer(ILimit limit);
        */

        public abstract void Print(); // remove

        protected abstract Account CalculateWithMethod(ILimit limit, DateTime dateTime, Action<int> addToPayment);
    }
}