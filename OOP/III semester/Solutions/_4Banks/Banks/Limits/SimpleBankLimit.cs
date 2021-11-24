using System.Collections.Generic;

namespace Banks.Banks.Limits
{
    public class SimpleBankLimit : ILimit
    {
        public SimpleBankLimit(
            double debitPercent,
            SortedDictionary<int, double> depositPercent,
            (int, int) creditLimit,
            int creditCommission,
            int daysForRepayment)
        {
            DebitPercent = debitPercent;
            CreditLimit = creditLimit;
            CreditCommission = creditCommission;
            DepositPercent = depositPercent;
            DaysForRepayment = daysForRepayment;
        }
    }
}