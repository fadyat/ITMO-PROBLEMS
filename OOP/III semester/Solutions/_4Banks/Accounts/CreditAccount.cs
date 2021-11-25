using System;
using System.Collections.Generic;
using Banks.Banks.Limits;

namespace Banks.Accounts
{
    public class CreditAccount : Account
    {
        public CreditAccount(int money, DateTime date)
            : base(money, date)
        {
            NegativeBalanceDays = new Queue<(DateTime, int)>();
        }

        public Queue<(DateTime negDay, int sum)> NegativeBalanceDays { get; }

        public override Account Calculate(ILimit limit, DateTime dateTime)
        {
            void AddToPayment(int days) => Balance -= limit.CreditCommission * days;

            return CalculateWithMethod(limit, dateTime, AddToPayment);
        }

        public override bool Check(ILimit limit)
        {
            throw new NotImplementedException();
        }

        public override void Print()
        {
            Console.Write("\t A: credit");
        }

        protected override Account CalculateWithMethod(
            ILimit limit,
            DateTime dateTime,
            Action<int> addToPayment)
        {
            DateTime first = NegativeBalanceDays.Peek().negDay;
            int diff = dateTime.Subtract(first).Days;
            if (Balance >= 0 || diff <= limit.DaysForRepayment)
            {
                return this;
            }

            int daysToPay = dateTime.Subtract(PrevCalcDate).Days;
            addToPayment(daysToPay);
            PrevCalcDate = dateTime;

            return this;
        }
    }
}