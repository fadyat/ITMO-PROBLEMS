using System;
using Banks.Banks.Limits;

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
            return Balance <= limit.CreditLimit.up;
        }

        public override bool ApprovedWithDraw(Limit limit)
        {
            return Balance >= limit.CreditLimit.down;
        }

        public override string ToString()
        {
            return "(" + nameof(CreditAccount) + ", " +
                   Balance.ToString(System.Globalization.CultureInfo.InvariantCulture) + ", " + Id + ")";
        }

        public override Account Calculate(Limit limit, DateTime dateTime)
        {
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