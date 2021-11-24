using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Banks.Limits;

namespace Banks.Accounts
{
    public class DepositAccount : Account
    {
        public DepositAccount(int money, DateTime date)
            : base(money, date)
        {
        }

        public double Payment { get; private set; }

        public override Account Calculate(ILimit limit, DateTime dateTime)
        {
            double FindPercent()
            {
                int maxBalance = 0;
                SortedDictionary<int, double> depositPercent = limit.DepositPercent;
                foreach (int balance in depositPercent.Keys
                    .Where(balance => balance <= Balance))
                {
                    maxBalance = balance;
                }

                return limit.DepositPercent[maxBalance];
            }

            void AddToPayment(int days)
            {
                Payment += Balance * (FindPercent() / 100 / 365) * days;
            }

            DateTime closesDayPayment = PrevCalcDate;
            if (PrevCalcDate.Day > Date.Day)
                closesDayPayment = closesDayPayment.AddMonths(1);

            closesDayPayment = closesDayPayment.AddDays(Date.Day - PrevCalcDate.Day);
            int payAfter = closesDayPayment.Day;

            int diff = dateTime.Subtract(PrevCalcDate).Days;

            if (diff < payAfter)
            {
                AddToPayment(diff);
                PrevCalcDate = dateTime;
                return this;
            }

            diff -= payAfter;
            AddToPayment(payAfter);
            Balance += Payment;
            PrevCalcDate = closesDayPayment;
            Payment = 0;

            while (diff > 0)
            {
                int daysInMonth = DateTime.DaysInMonth(PrevCalcDate.Year, PrevCalcDate.Month);
                int daysToPay = Math.Min(daysInMonth, diff);

                AddToPayment(daysToPay);
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

        public override void Print()
        {
            Console.Write("\t A: deposit");
        }
    }
}