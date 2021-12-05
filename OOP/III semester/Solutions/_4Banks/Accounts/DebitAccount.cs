using System;
using Banks.Banks.Limits;
using Banks.Exceptions;

namespace Banks.Accounts
{
    public class DebitAccount : Account
    {
        public DebitAccount(double balance, DateTime date)
            : base(balance, date)
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

        public override string ToString()
        {
            return "(" + nameof(DebitAccount) + ", " +
                   Balance.ToString(System.Globalization.CultureInfo.InvariantCulture) + ", " + Id + ")";
        }

        public override Account Calculate(Limit limit, DateTime dateTime)
        {
            if (dateTime < Date)
            {
                throw new BankException("Can't travel to the past!");
            }

            var nextAccountStatus = (DebitAccount)MemberwiseClone();

            void AddToPayment(int days) =>
                nextAccountStatus.Payment += nextAccountStatus.Balance * (limit.DebitPercent / 100 / 365) * days;

            return CalculateWithMethod(nextAccountStatus, dateTime, AddToPayment);
        }

        protected static Account CalculateWithMethod(
            DebitAccount nextAccountStatus,
            DateTime dateTime,
            Action<int> addToPayment)
        {
            DateTime closesDayPayment = nextAccountStatus.PrevCalcDate
                .AddMonths(1)
                .AddDays(nextAccountStatus.Date.Day - nextAccountStatus.PrevCalcDate.Day);

            int payAfter = closesDayPayment.Subtract(nextAccountStatus.PrevCalcDate).Days;
            int diff = dateTime.Subtract(nextAccountStatus.PrevCalcDate).Days;
            if (diff < payAfter)
            {
                addToPayment(diff);
                nextAccountStatus.PrevCalcDate = dateTime;
                return nextAccountStatus;
            }

            diff -= payAfter;
            addToPayment(payAfter);
            nextAccountStatus.Balance += nextAccountStatus.Payment;
            nextAccountStatus.PrevCalcDate = closesDayPayment;
            nextAccountStatus.Payment = 0;

            while (diff > 0)
            {
                int daysInMonth = DateTime.DaysInMonth(
                    nextAccountStatus.PrevCalcDate.Year,
                    nextAccountStatus.PrevCalcDate.Month);

                if (diff >= daysInMonth)
                {
                    addToPayment(daysInMonth);
                    diff -= daysInMonth;
                    nextAccountStatus.Balance += nextAccountStatus.Payment;
                    nextAccountStatus.Payment = 0;
                    nextAccountStatus.PrevCalcDate = nextAccountStatus.PrevCalcDate.AddMonths(1);
                }
                else
                {
                    addToPayment(diff);
                    diff = 0;
                    nextAccountStatus.PrevCalcDate = nextAccountStatus.PrevCalcDate.AddDays(diff);
                }
            }

            return nextAccountStatus;
        }
    }
}