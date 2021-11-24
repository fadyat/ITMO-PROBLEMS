using System.Collections.Generic;

namespace Banks.Banks.Limits
{
    public class SimpleBankLimit : ILimit
    {
        public SimpleBankLimit(
            double debitPercent,
            SortedDictionary<int, double> depositPercent,
            int creditLimit,
            double creditCommission)
        {
            DebitPercent = debitPercent;
            CreditLimit = creditLimit;
            CreditCommission = creditCommission;
            DepositPercent = depositPercent;
        }

        public double DebitPercent { get; }

        public SortedDictionary<int, double> DepositPercent { get; }

        public int CreditLimit { get; }

        public double CreditCommission { get; }
    }
}