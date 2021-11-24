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

        public override Account Calculate(ILimit limit, DateTime dateTime)
        {
            void AddToPayment(int days) =>
                Payment += Balance * (limit.DebitPercent / 100 / 365) * days;

            return CalculateWithMethod(limit, dateTime, AddToPayment);
        }

        public override void Print()
        {
            Console.Write("\t A: debit");
        }

        protected override Account CalculateWithMethod(
            ILimit limit,
            DateTime dateTime,
            Action<int> addToPayment)
        {
            DateTime closesDayPayment = PrevCalcDate;
            if (PrevCalcDate.Day > Date.Day)
                closesDayPayment = closesDayPayment.AddMonths(1);

            closesDayPayment = closesDayPayment.AddDays(Date.Day - PrevCalcDate.Day);
            int payAfter = closesDayPayment.Day;

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
                int daysToPay = Math.Min(daysInMonth, diff);

                addToPayment(daysToPay);
                if (daysToPay != diff)
                {
                    Balance += Payment;
                    Payment = 0;
                    PrevCalcDate = PrevCalcDate.AddMonths(1);
                }
                else
                {
                    PrevCalcDate = PrevCalcDate.AddDays(diff);
                }

                diff -= daysToPay;
            }

            return this;
        }
    }
}