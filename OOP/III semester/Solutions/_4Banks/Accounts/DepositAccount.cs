using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Banks.Limits;

namespace Banks.Accounts
{
    public class DepositAccount : DebitAccount
    {
        public DepositAccount(int money, DateTime date)
            : base(money, date)
        {
        }

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

            void AddToPayment(int days) =>
                Payment += Balance * (FindPercent() / 100 / 365) * days;

            return CalculateWithMethod(limit, dateTime, AddToPayment);
        }

        public override void Print()
        {
            Console.Write("\t A: deposit");
        }
    }
}