using System.Collections.Generic;
using Banks.Exceptions;

namespace Banks.Banks.Limits
{
    public class SimpleBankLimit : Limit
    {
        public SimpleBankLimit(
            double debitPercent,
            SortedDictionary<int, double> depositPercent,
            double[] creditLimit,
            double creditCommission,
            double withDrawLimit,
            double transferLimit)
        {
            if (creditLimit.Length != 2)
            {
                throw new BankException("Invalid data!");
            }

            DebitPercent = debitPercent;
            CreditLimit = creditLimit;
            CreditCommission = creditCommission;
            DepositPercent = depositPercent;
            WithDrawLimit = withDrawLimit;
            TransferLimit = transferLimit;
        }
    }
}