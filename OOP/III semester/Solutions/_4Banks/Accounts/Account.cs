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

        protected DateTime PrevCalcDate { get; set; }

        /* For approve operations */

        // NEED COMMANDS!!!!!!!!!
        public abstract bool Check(ILimit limit);

        public abstract bool ApproveTopUp(ILimit limit); // fake

        public abstract bool ApproveWithDraw(ILimit limit); // fake

        public abstract bool ApproveTransfer(ILimit limit); // fake

        /* Operations */
        public abstract Account TopUp(ILimit limit); // fake

        public abstract Account WithDraw(ILimit limit); // fake

        public abstract Account Transfer(ILimit limit); // fake


        /* For checks */
        public abstract void Print();

        /* For TimeMachine */
        public abstract Account Calculate(ILimit limit, DateTime dateTime);

        protected abstract Account CalculateWithMethod(ILimit limit, DateTime dateTime, Action<int> addToPayment);
    }
}