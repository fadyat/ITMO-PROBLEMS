using System;
using Banks.Banks.Limits;

namespace Banks.Accounts
{
    public class DebitAccount : Account
    {
        public DebitAccount(int money, DateTime date)
            : base(money, date)
        {
            Payment = 0;
        }

        protected double Payment { get; set; }

        public override bool ApprovedTopUp(Limit limit)
        {
            return true;
        }

        public override bool ApprovedWithDraw(Limit limit)
        {
            return Balance >= 0;
        }

        public override void Print()
        {
            Console.Write("\t A: debit");
        }

        public override Account Calculate(Limit limit, DateTime dateTime)
        {
            void AddToPayment(int days) =>
                Payment += Balance * (limit.DebitPercent / 100 / 365) * days;

            return CalculateWithMethod(dateTime, AddToPayment);
        }

        protected Account CalculateWithMethod(DateTime dateTime, Action<int> addToPayment)
        {
            DateTime closesDayPayment = PrevCalcDate
                .AddMonths(1)
                .AddDays(Date.Day - PrevCalcDate.Day);

            int payAfter = closesDayPayment.Subtract(PrevCalcDate).Days;
            int diff = dateTime.Subtract(PrevCalcDate).Days;
            if (diff < payAfter)
            {
                addToPayment(diff);
                PrevCalcDate = dateTime;
                return this;
            }

            diff -= payAfter;
            addToPayment(payAfter);
            Balance += Payment;
            PrevCalcDate = closesDayPayment;
            Payment = 0;

            while (diff > 0)
            {
                int daysInMonth = DateTime.DaysInMonth(PrevCalcDate.Year, PrevCalcDate.Month);
                if (diff >= daysInMonth)
                {
                    addToPayment(daysInMonth);
                    diff -= daysInMonth;
                    Balance += Payment;
                    Payment = 0;
                    PrevCalcDate = PrevCalcDate.AddMonths(1);
                }
                else
                {
                    addToPayment(diff);
                    diff = 0;
                    PrevCalcDate = PrevCalcDate.AddDays(diff);
                }
            }

            return this;
        }
    }
}