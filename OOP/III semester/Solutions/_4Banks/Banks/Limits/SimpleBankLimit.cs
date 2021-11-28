using System.Collections.Generic;

namespace Banks.Banks.Limits
{
    public class SimpleBankLimit : Limit
    {
        public SimpleBankLimit(
            double debitPercent,
            SortedDictionary<int, double> depositPercent,
            (double, double) creditLimit,
            double creditCommission,
            double withDrawLimit,
            double transferLimit)
        {
            DebitPercent = debitPercent;
            CreditLimit = creditLimit;
            CreditCommission = creditCommission;
            DepositPercent = depositPercent;
            WithDrawLimit = withDrawLimit;
            TransferLimit = transferLimit;
        }
    }
}