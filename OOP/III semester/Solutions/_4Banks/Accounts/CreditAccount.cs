using System;
using Banks.Banks.Limits;
using Banks.Exceptions;

namespace Banks.Accounts
{
    public class CreditAccount : Account
    {
        public CreditAccount(double balance, DateTime date)
            : base(balance, date)
        {
        }

        public override bool ApprovedTopUp(Limit limit)
        {
            return Balance <= limit.CreditLimit[1];
        }

        public override bool ApprovedWithDraw(Limit limit)
        {
            return Balance >= limit.CreditLimit[0];
        }

        public override string ToString()
        {
            return "(" + nameof(CreditAccount) + ", " +
                   Balance.ToString(System.Globalization.CultureInfo.InvariantCulture) + ", " + Id + ")";
        }

        public override Account Calculate(Limit limit, DateTime dateTime)
        {
            if (dateTime < Date)
            {
                throw new BankException("Can't travel to the past!");
            }

            var nextAccountStatus = (CreditAccount)MemberwiseClone();

            void AddToPayment(int days) =>
                nextAccountStatus.Balance -= limit.CreditCommission * days;

            if (nextAccountStatus.Balance >= 0)
            {
                return nextAccountStatus;
            }

            int daysToPay = dateTime.Subtract(nextAccountStatus.PrevCalcDate).Days;
            AddToPayment(daysToPay);
            nextAccountStatus.PrevCalcDate = dateTime;

            return nextAccountStatus;
        }
    }
}