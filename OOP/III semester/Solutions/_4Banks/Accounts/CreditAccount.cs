using System;
using System.Collections.Generic;
using Banks.Banks.Limits;

namespace Banks.Accounts
{
    public class CreditAccount : Account
    {
        public CreditAccount(int money, DateTime date)
            : base(money, date) { }

        public override Account Calculate(Limit limit, DateTime dateTime)
        {
            void AddToPayment(int days) =>
                Balance -= limit.CreditCommission * days;

            if (Balance >= 0)
            {
                return this;
            }

            int daysToPay = dateTime.Subtract(PrevCalcDate).Days;
            AddToPayment(daysToPay);
            PrevCalcDate = dateTime;

            return this;
        }

        public override bool ApprovedTopUp(Limit limit)
        {
            return Balance <= limit.CreditLimit.up;
        }

        public override bool ApprovedWithDraw(Limit limit)
        {
            return Balance >= limit.CreditLimit.down;
        }

        public override bool ApprovedTransfer(Account toAccount, double amount, Limit limit)
        {
            // ???
            throw new NotImplementedException();
        }

        public override void Print()
        {
            Console.Write("\t A: credit");
        }
    }
}