using System.Collections.Generic;

namespace Banks.Banks.Limits
{
    public class SimpleBankLimit : Limit
    {
        public SimpleBankLimit(
            double debitPercent,
            SortedDictionary<int, double> depositPercent,
            (int, int) creditLimit,
            int creditCommission)
        {
            DebitPercent = debitPercent;
            CreditLimit = creditLimit;
            CreditCommission = creditCommission;
            DepositPercent = depositPercent;
        }
    }
}