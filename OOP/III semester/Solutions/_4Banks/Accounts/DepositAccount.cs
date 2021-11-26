using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Banks.Limits;

namespace Banks.Accounts
{
    public class DepositAccount : DebitAccount
    {
        public DepositAccount(double balance, DateTime date, DateTime duration)
            : base(balance, date)
        {
            Duration = duration;
        }

        private DateTime Duration { get; }

        public override bool ApprovedWithDraw(Limit limit)
        {
            return PrevCalcDate >= Duration && base.ApprovedWithDraw(limit);
        }

        public override bool ApprovedTransfer(Account toAccount, Limit limit)
        {
            return PrevCalcDate >= Duration && base.ApprovedTransfer(toAccount, limit);
        }

        public override void Print()
        {
            Console.Write("\t A: deposit");
        }

        public override Account Calculate(Limit limit, DateTime dateTime)
        {
            var nextAccountStatus = (DepositAccount)MemberwiseClone();

            double FindPercent()
            {
                int maxBalance = 0;
                SortedDictionary<int, double> depositPercent = limit.DepositPercent;
                foreach (int balance in depositPercent.Keys
                    .Where(balance => balance <= nextAccountStatus.Balance))
                {
                    maxBalance = balance;
                }

                return limit.DepositPercent[maxBalance];
            }

            dateTime = (nextAccountStatus.Duration.CompareTo(dateTime) > 0) ? dateTime : nextAccountStatus.Duration;

            void AddToPayment(int days) =>
                nextAccountStatus.Payment += nextAccountStatus.Balance * (FindPercent() / 100 / 365) * days;

            return CalculateWithMethod(nextAccountStatus, dateTime, AddToPayment);
        }
    }
}